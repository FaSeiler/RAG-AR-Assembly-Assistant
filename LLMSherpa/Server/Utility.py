import pandas as pd
import datetime
from IPython.core.display import display, HTML
import os
import re


def PrettyPrint(dataFrame):
    return display(HTML(dataFrame.to_html().replace("\\n", "<br>")))

# Sample:
# visualize_retrieved_nodes(get_retrieved_nodes(index, "Tell me all the basic components of the et 200 sp?", vector_top_k=20, reranker_top_n=10, with_reranker=True))
def VisualizeRetrievedNodes(nodes) -> None:
    result_dicts = []
    for node in nodes:
        result_dict = {"Score": node.score, "Text": node.node.get_text(), "Metadata": node.metadata}
        result_dicts.append(result_dict)

    PrettyPrint(pd.DataFrame(result_dicts))

def SaveAsHTML(dataFrame, filename="nodes.html"):
    html_content = dataFrame.to_html().replace("\\n", "<br>")
    directory = "Logs"
    if not os.path.exists(directory):
        os.makedirs(directory)
    filepath = os.path.join(directory, filename)
    with open(filepath, "w", encoding="utf-8") as file:
        file.write(html_content)

def VisualizeRetrievedNodesToFile(nodes) -> None:
    timestamp = datetime.datetime.now().strftime("%Y-%m-%d_%H-%M-%S")
    filename = f"Retrieved_Nodes_{timestamp}.html"

    result_dicts = []
    for node in nodes:
        result_dict = {"Score": node.score, "Text": node.node.get_text(), "Metadata": node.metadata}
        result_dicts.append(result_dict)

    SaveAsHTML(pd.DataFrame(result_dicts), filename)

def ExtractPageNumbers(text):
    # Define the regex pattern to find different page number formats
    pattern = (
        r'page (\d+)|'                  # "page <int>"
        r'pages (\d+)-(\d+)|'           # "pages <int>-<int>"
        r'Page (\d+)|'                  # "Page <int>"
        r'Pages (\d+)-(\d+)|'           # "Pages <int>-<int>"
        r'page number: (\d+)|'          # "page number: <int>"
        r'Page number: (\d+)|'          # "Page number: <int>"
        r'Page Number: (\d+)|'          # "Page Number: <int>"
        r'Page Numbers: ([\d, ]+)|'     # "Page Numbers: <int>, <int>, ..."
        r'page numbers: ([\d, ]+)|'     # "page numbers: <int>, <int>, ..."
        r'page numbers are (\d+)|'      # "page numbers are <int>"
        r'page_number: (\d+)'           # "page_number: <int>"
        r'page_number: (\d+)-(\d+)'     # "page_number: <int>-<int>"
        r'Page Number: (\d+)-(\d+)'     # "Page Number: <int>-<int>"
    )
    
    # Find all matches in the text
    matches = re.findall(pattern, text, re.IGNORECASE)
    
    # Set to hold all page numbers (to avoid duplicates)
    page_numbers = set()
    
    # Process matches
    for match in matches:
        if match[0]:  # single page number, matched by "page <int>"
            page_numbers.add(int(match[0]))
        elif match[1] and match[2]:  # page range, matched by "pages <int>-<int>"
            start, end = int(match[1]), int(match[2])
            page_numbers.update(range(start, end + 1))
        elif match[3]:  # single page number, matched by "Page <int>"
            page_numbers.add(int(match[3]))
        elif match[4] and match[5]:  # page range, matched by "Pages <int>-<int>"
            start, end = int(match[4]), int(match[5])
            page_numbers.update(range(start, end + 1))
        elif match[6]:  # single page number, matched by "page number: <int>"
            page_numbers.add(int(match[6]))
        elif match[7]:  # single page number, matched by "Page number: <int>"
            page_numbers.add(int(match[7]))
        elif match[8]:  # single page number, matched by "Page Number: <int>"
            page_numbers.add(int(match[8]))
        elif match[9]:  # multiple page numbers, matched by "Page Numbers: <int>, <int>, ..."
            numbers = re.findall(r'\d+', match[9])  # Find all individual numbers in the string
            page_numbers.update(map(int, numbers))
        elif match[10]:  # multiple page numbers, matched by "page numbers: <int>, <int>, ..."
            numbers = re.findall(r'\d+', match[10])  # Find all individual numbers in the string
            page_numbers.update(map(int, numbers))
        elif match[11]:  # single page number, matched by "page numbers are <int>"
            page_numbers.add(int(match[11]))
        elif match[12]:  # single page number, matched by "page_number: <int>"
            page_numbers.add(int(match[12]))
        elif match[13] and match[14]:  # page range, matched by "page_number: <int>-<int>"
            start, end = int(match[13]), int(match[14])
            page_numbers.update(range(start, end + 1))
        elif match[15] and match[16]:  # page range, matched by "Page Number: <int>-<int>"
            start, end = int(match[15]), int(match[16])
            page_numbers.update(range(start, end + 1))
    
    # Convert the set to a sorted list
    page_numbers = sorted(page_numbers)
    
    return page_numbers