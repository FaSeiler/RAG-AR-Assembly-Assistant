from DocumentParser import *
from ParserPostprocessor import *
from VectorStoreChromaDB import CreateIndex, LoadIndex
from RetrieverPostprocessor import *
from RAG import *
from Utility import *
from PDFImageParser import ExtractImagesFromPDF
import json

data_dir = "../data/"
pdf_file_name = "et200sp_system_manual_en-US_en-US_stripped.pdf"
# pdf_file_name = "Lebenslauf_Fabian_Seiler.pdf"

pdf_url = data_dir + pdf_file_name

def main():
    load_index = True
    InitializeLlamaIndex()

    image_dict = {}

    if load_index:
        index = LoadIndex()
        image_dict = LoadImagesFromDirectory(pdf_url)
        print("Index loaded.")
    else:
        doc = ParsePDF(pdf_url=pdf_url)
        # PrintChunks(doc)
        custom_chunks = MergeChunks(doc)
        for chunk in custom_chunks:
            PrintCustomChunk(chunk)
        index = CreateIndex(custom_chunks)
        image_dict = ExtractImagesFromPDF(pdf_url)
        print("Index created.")

    # VisualizeRetrievedNodesToFile(TestNodeReranking(index, "Tell me all the basic components of the et 200 sp?", vector_top_k=20, reranker_top_n=10, with_reranker=True))

    print()
    print("="*150)
    query = "What are the steps for installing/mounting a BaseUnit? Include the page_numbers but no introductory sentences."
    print(f"User Query:\n {query}")
    print("="*150)

    print()
    print("Querying the index...")
    print()

    # Query the index
    query_engine = GetQueryEngine(index)
    response = query_engine.query(query)
    VisualizeRetrievedNodesToFile(nodes=response.source_nodes)


    print("Response:")
    print(response)
    print()
    print("-"*150)

    # Format response to JSON
    json_response = GetFormattedJSONQueryResponse(response, image_dict, pdf_file_name)
    # print(json_response)
    key_values = json.loads(json_response).items()
    for key, value in key_values:
        print("<KEY> ", key)
        print("<VALUE> ", value)

if __name__ == "__main__":
    main()

    