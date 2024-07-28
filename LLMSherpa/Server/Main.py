from DocumentParser import *
from ParserPostprocessor import *
from VectorStoreChromaDB import CreateIndex, LoadIndex
from RetrieverPostprocessor import *
from RAG import *
from Utility import *
from PDFImageParser import ExtractImagesFromPDF
import json
import os

data_dir = "../data/"
# pdf_file_name = "et200sp_system_manual_en-US_en-US_stripped.pdf"
# pdf_file_name = "Lebenslauf_Fabian_Seiler.pdf"

# pdf_url = data_dir + pdf_file_name


def Init(load_index=False):
    InitializeLlamaIndex()

    pdf_files = [file for file in os.listdir(data_dir) if file.endswith(".pdf")] # Find all PDF files in the data directory
    pdf_urls = [os.path.join(data_dir, pdf_file) for pdf_file in pdf_files] # Create a list of full paths to the PDF files
    pdf_data = {} # Dictionary to hold the query engine and image dictionary for each PDF file

    for pdf_url in pdf_urls:
        # Get a query engine and image dictionary for each PDF file
        query_engine, image_dict = InitPDFData(pdf_url, load_index)
        pdf_data[pdf_url] = {'query_engine': query_engine, 'image_dict': image_dict}
        
        print("-"*150)
        print()

    return pdf_data

def InitPDFData(pdf_url, load_index=False):
    pdf_file_name = os.path.basename(pdf_url)
    pdf_name = os.path.splitext(pdf_file_name)[0]

    if load_index:
        index = LoadIndex(pdf_name)
        image_dict = LoadImagesFromDirectory(pdf_url)
        print(f"Index for {pdf_url} loaded.")
    else:
        doc = ParsePDF(pdf_url=pdf_url)
        # PrintChunks(doc)
        custom_chunks = MergeChunks(doc)
        for chunk in custom_chunks:
            PrintCustomChunk(chunk)
        index = CreateIndex(pdf_name, custom_chunks)
        image_dict = ExtractImagesFromPDF(pdf_url)
        print(f"Index for {pdf_url} created.")

    
    query_engine = GetQueryEngine(index)

    return query_engine, image_dict
    

def main():
    load_index = True

    pdf_data = Init(load_index)

    # pdf_file_name = "et200sp_system_manual_en-US_en-US_stripped.pdf"
    pdf_file_name = "Lebenslauf_Fabian_Seiler.pdf"
    pdf_url = data_dir + pdf_file_name
    if pdf_url in pdf_data: # Check if the PDF data exists
        query_engine = pdf_data[pdf_url]['query_engine']
        image_dict = pdf_data[pdf_url]['image_dict']
    else:
        print("PDF data does not exist.")


    print()
    print("="*150)
    # query = "What are the steps for installing/mounting a BaseUnit? Include the page_numbers but no introductory sentences."
    query = "Wie alt is Fabian Seiler?"

    print(f"User Query:\n {query}")
    print("="*150)

    print()
    print("Querying the index...")
    print()

    
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

    