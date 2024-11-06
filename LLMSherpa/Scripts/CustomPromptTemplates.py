from llama_index.core import PromptTemplate

def GetPromptTemplateQA():
    templateQA = """<|begin_of_text|><|start_header_id|>system<|end_header_id|>

    You are an assistant for answering questions about a technical manual. Use the following pieces of retrieved context from the manual to answer the question. \
    If you don't know the answer, just say that you don't know. Be as detailed and concise as possible. For every piece of information you provide include the 'page_number' and 'parent_hierarchy_section' from the metadata where you found this.<|eot_id|>
    <|start_header_id|>user<|end_header_id|>

    Question: {query_str}
    Context: {context_str}
    Answer: <|eot_id|>
    <|start_header_id|>assistant<|end_header_id|>"""
    qa_template = PromptTemplate(templateQA)
    return qa_template


def GetPromptTemplateSummary():
    templateSummary = """<|begin_of_text|><|start_header_id|>system<|end_header_id|>

    Context information from multiple sources is below. Given the information from multiple sources and no prior knowledge, answer the query.<|eot_id|>
    <|start_header_id|>user<|end_header_id|>

    Question: {query_str}
    Context: {context_str}
    Answer: <|eot_id|>
    <|start_header_id|>assistant<|end_header_id|>"""
    summary_template = PromptTemplate(templateSummary)
    return summary_template


def GetPromptTemplateRefine():
    templateRefine = """<|begin_of_text|><|start_header_id|>system<|end_header_id|>

    Based on the original question and an existing answer, refine the existing answer (only if needed) with the additional context below.<|eot_id|>
    <|start_header_id|>user<|end_header_id|>

    Original Question: {query_str}
    Existing Answer: {existing_answer}
    Context: {context_str}
    Answer: <|eot_id|>
    <|start_header_id|>assistant<|end_header_id|>"""

    refine_template = PromptTemplate(templateRefine)
    return refine_template


def PrintPromptTemplate():
    # you can create text prompt (for completion API)
    qa_template = GetPromptTemplateQA()
    prompt = qa_template.format(
        query_str=..., context_str=...
    )  # , existing_answer=...)
    print("Prompt:")
    print(prompt)

    print()
    print("=" * 100)
    print()

    # or easily convert to message prompts (for chat API)
    messages = qa_template.format_messages(
        query_str=..., context_str=...
    )  # , existing_answer=...)
    print("Messages:")
    print(messages)


# Helper function to view the prompt format
def DisplayPromptDict(query_engine):
    """
    Display the prompts dictionary obtained from the query engine.

    Args:
        query_engine: The query engine object used to retrieve the prompts.

    Returns:
        None
    """
    prompts_dict = query_engine.get_prompts()

    for k, p in prompts_dict.items():
        text_md = f"**Prompt Key**: {k} -> " f"**Text:**"
        print(text_md)
        print(p.get_template())
