from llama_index.core import PromptTemplate

# templateQA = """<|begin_of_text|><|start_header_id|>system<|end_header_id|>

# Given the context below answer the question without any introductory sentences. \
# Format your answer as a JSON object with three attributes: response (a string containing the answer), section_hierarchy (a string containing the section hierarchy)\
#       and page_nrs (a list of integers that represent the page numbers where this information was found)."<|eot_id|>
# <|start_header_id|>user<|end_header_id|>

# Question: {query_str}
# Context: {context_str}
# Answer: <|eot_id|>
# <|start_header_id|>assistant<|end_header_id|>"""

# templateQA = """<|begin_of_text|><|start_header_id|>system<|end_header_id|>

# Given the context below answer the question. \
# The output should be a single json formated in the following schema: \
# {{ \
#   'response' // A string containing the answer \
#   'section_hierarchy' // A string conatining the section hierarchy where the answer can be found\
#   'page_nrs' // A list of int with all the page number(s) where the answer can be found \
# }}<|eot_id|>
# <|start_header_id|>user<|end_header_id|>

# # Question: {query_str}
# # Context: {context_str}
# # Answer: <|eot_id|>
# # <|start_header_id|>assistant<|end_header_id|>"""

# templateQA_instruction_generation = """<|begin_of_text|><|start_header_id|>system<|end_header_id|>

# You are a helper in creating assembly instructions based on a technical manual. Use the following pieces of retrieved context from the manual and the question to produce instructions. \
# If you don't know the answer, just say that you don't know. Be as detailed as possible.<|eot_id|>
# <|start_header_id|>user<|end_header_id|>

# Question: {query_str}
# Context: {context_str}
# Answer: <|eot_id|>
# <|start_header_id|>assistant<|end_header_id|>"""

# templateQA = """<|begin_of_text|><|start_header_id|>system<|end_header_id|>

# You are an assistant for creating assembly instructions for electrical components based on a technical manual. Only use the retrieved context from the manual and the question to produce instructions. \
# Always include an Introduction, the Requirement, the Required Tools, the steps for mounting/installing the component and the parent_section.<|eot_id|>
# <|start_header_id|>user<|end_header_id|>

# Question: {query_str}
# Context: {context_str}
# Answer: <|eot_id|>
# <|start_header_id|>assistant<|end_header_id|>"""

def GetPromptTemplateQA():
    templateQA = """<|begin_of_text|><|start_header_id|>system<|end_header_id|>

    You are an assistant for answering questions about a technical manual. Use the following pieces of retrieved context from the manual to answer the question. \
    If you don't know the answer, just say that you don't know. Be as detailed as possible. Always include the 'page_number' and 'parent_hierarchy_section' from the metadata.<|eot_id|>
    <|start_header_id|>user<|end_header_id|>

    Question: {query_str}
    Context: {context_str}
    Answer: <|eot_id|>
    <|start_header_id|>assistant<|end_header_id|>"""
    qa_template = PromptTemplate(templateQA)
    return qa_template

def GetPromptTemplateSummary():
    templateSummary = """<|begin_of_text|><|start_header_id|>system<|end_header_id|>

    Context information from multiple sources is below. Given the information from multiple sources and not prior knowledge, answer the query.<|eot_id|>
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
    prompt = qa_template.format(query_str=..., context_str=...) #, existing_answer=...)
    print("Prompt:")
    print(prompt)

    print()
    print("="*100)
    print()

    # or easily convert to message prompts (for chat API)
    messages = qa_template.format_messages(query_str=..., context_str=...) #, existing_answer=...)
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

