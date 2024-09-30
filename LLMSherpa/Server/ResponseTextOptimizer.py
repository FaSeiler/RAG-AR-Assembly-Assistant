from langchain_community.llms.ollama import Ollama
import sys
import re

# Takes in a text and removes the sections and page numbers

optimizerPrompt = """<|begin_of_text|><|start_header_id|>system<|end_header_id|>

    In the following text remove all information about the page_number and the parent_section_hierarchy but don't mention this in your answer. The rest of the content should remain the same.<|eot_id|>
    <|start_header_id|>user<|end_header_id|>

    Text: {text_str}
    Answer: <|eot_id|>
    <|start_header_id|>assistant<|end_header_id|>"""


def ResponseOptimizer(text):
    formatted_template = optimizerPrompt.format(text_str=text)  # Format the template with text_str
    print(formatted_template)  # Print the formatted template

    print("="*100)

    model = Ollama(model="llama3.1")
    optimizedText = model.invoke(formatted_template)
    print("After removing sections and page numbers:\n", optimizedText)  # Print the formatted template

    return optimizedText

# if __name__ == "__main__":
#     if len(sys.argv) < 2:
#         print("Please provide the text as an argument.")
#     else:
#         text = sys.argv[1]
#         values = ResponseOptimizer(text)
#         print(values)
#         sum_values = sum(values)
#         print("Sum of values:", sum_values)
        