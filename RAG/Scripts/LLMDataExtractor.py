from langchain_community.llms.ollama import Ollama
from ModelConfig import llm_model
from CustomPromptTemplates import GetPromptTemplatePageNumberExtractor
import sys
import re

# Takes in a text and extracts data from it.
def CSVStringToList(csv_str):
    # Remove leading/trailing quotation marks and spaces from the whole string
    csv_str = csv_str.strip().strip('"').strip("'")

    # Check if the string is empty after stripping
    if not csv_str:
        return []

    # Split the string by commas
    parts = csv_str.split(",")
    numbers = []

    for part in parts:
        part = part.strip()  # Remove leading/trailing spaces
        if "-" in part:
            # Validate the hyphenated part
            if re.match(r"^\s*\d+\s*-\s*\d+\s*$", part):
                # Split by hyphen and convert to integers
                start, end = map(lambda x: int(x.strip()), part.split("-"))
                numbers.extend([start, end])
            else:
                # Invalid format, return empty list
                return []
        else:
            # Handle standalone number
            if re.match(r"^\d+$", part):
                numbers.append(int(part))
            else:
                # Invalid format, return empty list
                return []

    return numbers


def PageNumberExtractor(text):
    pageExtractorPrompt = GetPromptTemplatePageNumberExtractor()
    formatted_template = pageExtractorPrompt.format(
        text_str=text
    )  # Format the template with text_str
    print(formatted_template)  # Print the formatted template

    print("=" * 100)

    model = Ollama(model=llm_model)
    csvString = model.invoke(formatted_template)
    print(csvString)  # Print the formatted template

    values = CSVStringToList(csvString)

    return values


if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Please provide the text as an argument.")
    else:
        text = sys.argv[1]
        values = PageNumberExtractor(text)
        print(values)
        sum_values = sum(values)
        print("Sum of values:", sum_values)

        # Example text: "The information can be found on pages 5-10 and 15, 20, 25-30."
