import pandas as pd
import datetime
from IPython.core.display import display, HTML
import os

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
    with open(filepath, "w") as file:
        file.write(html_content)

def VisualizeRetrievedNodesToFile(nodes) -> None:
    timestamp = datetime.datetime.now().strftime("%Y-%m-%d_%H-%M-%S")
    filename = f"Retrieved_Nodes_{timestamp}.html"

    result_dicts = []
    for node in nodes:
        result_dict = {"Score": node.score, "Text": node.node.get_text(), "Metadata": node.metadata}
        result_dicts.append(result_dict)

    SaveAsHTML(pd.DataFrame(result_dicts), filename)