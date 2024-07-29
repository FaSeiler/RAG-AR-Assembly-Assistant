import json
from llama_index.llms.ollama import Ollama
from llama_index.embeddings.huggingface import HuggingFaceEmbedding
from llama_index.core import Settings
from llama_index.core.query_engine import RetrieverQueryEngine
from Retriever import GetRetriever
from ResponseSynthesizer import GetResponseSynthesizer
from CustomPromptTemplates import *
from DocumentParser import *
from ParserPostprocessor import *
from VectorStoreChromaDB import CreateIndex, LoadIndex
from RetrieverPostprocessor import *
from Utility import *
from PDFImageParser import *

def InitializeLlamaIndex():
    llm = Ollama(model="llama3", request_timeout=120.0) #, output_parser=output_parser)
    Settings.llm = llm

    # embed_model = HuggingFaceEmbedding(model_name="BAAI/bge-small-en-v1.5")
    # embed_model = HuggingFaceEmbedding(model_name="BAAI/bge-base-en-v1.5")
    # embed_model = HuggingFaceEmbedding(model_name="Salesforce/SFR-Embedding-Mistral") # Too slow
    embed_model = HuggingFaceEmbedding(model_name="Alibaba-NLP/gte-large-en-v1.5", trust_remote_code=True)
    Settings.embed_model = embed_model



def GetQueryEngine(index):
    query_engine = RetrieverQueryEngine.from_args(
    retriever=GetRetriever(index), 
    response_synthesizer=GetResponseSynthesizer(), 
    node_postprocessors=GetNodePostprocessors(), 
    # output_cls=ManualExcerpt,
    )

    # Update RetrieverQueryEngine prompt template to match LLAMA3 format
    query_engine.update_prompts({"response_synthesizer:text_qa_template": GetPromptTemplateQA(),
                                "response_synthesizer:summary_template": GetPromptTemplateSummary(),
                                "response_synthesizer:refine_template": GetPromptTemplateRefine()})

    # DisplayPromptDict(query_engine)
    return query_engine


def GetFormattedJSONQueryResponse(response, image_dict, pdf_file_name, print_context_nodes=False):
    text = response.response
    page_numbers = ExtractPageNumbers(response.response)

    # Initialize parsed_dict
    parsed_dict = {}
    parsed_dict["text"] = text
    parsed_dict["page_numbers"] = page_numbers

    pdf_name = os.path.splitext(pdf_file_name)[0]
    image_file_paths = []

    # Get image file paths for each page number
    for page_nr in parsed_dict["page_numbers"]:
        image_file_paths.extend(GetImageFilePaths(image_dict=image_dict, pdf_name=pdf_name, page_idx=page_nr))
        
    # parsed_dict["image_file_paths"] = image_file_paths

    # Encode images to base64
    encoded_images = {}
    for image_file_path in image_file_paths:
        if os.path.exists(image_file_path):
            encoded_images[image_file_path] = GetBase64Image(image_file_path)

    # Add encoded images to the parsed_dict
    parsed_dict["encoded_images"] = encoded_images

    # print(image_file_paths)

    # for file_path in image_file_paths:
    #     Show_Image(pdf_name=pdf_name, file_path=file_path)

    # convert dictionary to JSON object
    json_object = json.dumps(parsed_dict)

    # if print_context_nodes:
    #     visualize_retrieved_nodes(response.source_nodes)

    return json_object


# =================================================================================================

data_dir = "../data/"

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
    
def SendQueryForPDF(query, pdf_file_name, pdf_data):
    pdf_url = data_dir + pdf_file_name

    # Check if the PDF data exists
    if pdf_url in pdf_data: 
        query_engine = pdf_data[pdf_url]['query_engine']
        image_dict = pdf_data[pdf_url]['image_dict']
    else:
        print("PDF data does not exist.")

    print()
    print("="*150)

    print(f"User Query:\n {query}") # Print the user query
    print("="*150)

    print()
    print("Querying the index...")
    print()

    response = query_engine.query(query) # Query the index
    VisualizeRetrievedNodesToFile(nodes=response.source_nodes) # Save the retrieved nodes to a file

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
    return json_response





