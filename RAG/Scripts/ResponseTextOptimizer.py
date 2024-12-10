from langchain_community.llms.ollama import Ollama
from ModelConfig import llm_model
from CustomPromptTemplates import GetPromptTemplatePageSectionRemover
import sys
import re


# Takes in a text and removes the sections and page numbers
def ResponseOptimizer(text):
    optimizerPrompt = GetPromptTemplatePageSectionRemover()
    formatted_template = optimizerPrompt.format(
        text_str=text
    )  # Format the template with text_str
    print(formatted_template)  # Print the formatted template

    print("=" * 100)

    model = Ollama(model=llm_model)
    optimizedText = model.invoke(formatted_template)
    print(
        "After removing sections and page numbers:\n", optimizedText
    )  # Print the formatted template

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
