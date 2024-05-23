import argparse
from langchain.vectorstores.chroma import Chroma
from langchain.prompts import ChatPromptTemplate
from langchain_community.llms.ollama import Ollama

from get_embedding_function import get_embedding_function

# CHROMA_PATH = "chroma_s71500_system_manual_en-US_en-US"
# CHROMA_PATH = "chroma_et200sp_system_manual_en-US_en-US"

# Stripped down version of the PDF
CHROMA_PATH = "et200sp_system_manual_en-US_en-US_stripped"


# PROMPT_TEMPLATE = """
# Answer the question based only on the following context:

# {context}

# ---

# Answer the question based on the above context: {question}
# """

# # Prompt
# PROMPT_TEMPLATE = ChatPromptTemplate(
#     template="""<|begin_of_text|><|start_header_id|>system<|end_header_id|> You are an assistant for question-answering tasks. 
#     Use the following pieces of retrieved context to answer the question. If you don't know the answer, just say that you don't know. 
#     Use three sentences maximum and keep the answer concise <|eot_id|><|start_header_id|>user<|end_header_id|>
#     Question: {question} 
#     Context: {context} 
#     Answer: <|eot_id|><|start_header_id|>assistant<|end_header_id|>""",
#     input_variables=["question", "document"],
# )

PROMPT_TEMPLATE = """<|begin_of_text|><|start_header_id|>system<|end_header_id|>

You are an assistant for question-answering tasks. Use the following pieces of retrieved context to answer the question. If you don't know the answer, just say that you don't know. Keep the answer concise and don't mention the context you are given in your response. <|eot_id|>
<|start_header_id|>user<|end_header_id|>

Question: {question} 
Context: {context} 
Answer: <|eot_id|>
<|start_header_id|>assistant<|end_header_id|>"""

def main():
    # Create CLI.
    parser = argparse.ArgumentParser()
    parser.add_argument("query_text", type=str, help="The query text.")
    parser.add_argument("--ignoreContext", action='store_true', help="Whether to ignore context in the query.")
    args = parser.parse_args()
    query_text = args.query_text
    ignoreContext = args.ignoreContext
    print(f"Ignore context: {ignoreContext}")
    query_rag(query_text, not ignoreContext)


def query_rag(query_text: str, useContext: bool = True, database: str = CHROMA_PATH, k_context: int = 5):
    if useContext:
        # Prepare the DB.
        embedding_function = get_embedding_function()
        db = Chroma(persist_directory=database, embedding_function=embedding_function)

        # Search the DB.
        results = db.similarity_search_with_score(query_text, k=k_context)

        context_text = "\n\n---\n\n".join([doc.page_content for doc, _score in results])
        prompt_template = ChatPromptTemplate.from_template(PROMPT_TEMPLATE)
        prompt = prompt_template.format(context=context_text, question=query_text)
        print(prompt)
    else:
        prompt = query_text

    model = Ollama(model="llama3")
    response_text = model.invoke(prompt)

    if useContext:
        sources = [doc.metadata.get("id", None) for doc, _score in results]
        formatted_response = f"Response: {response_text}\nSources: {sources}"
        print(formatted_response)
    else:
        print(f"Response: {response_text}")
    return response_text


if __name__ == "__main__":
    main()
