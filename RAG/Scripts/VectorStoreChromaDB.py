import chromadb
from llama_index.vector_stores.chroma import ChromaVectorStore
from llama_index.core import VectorStoreIndex
from llama_index.core.storage.storage_context import StorageContext
from llama_index.core import Document
from tqdm import tqdm


def CreateChromaDB(pdf_name):
    """
    Creates a ChromaDB and returns the chroma_collection.

    Returns:
        chroma_collection: The collection object representing the ChromaDB.
    """
    db = chromadb.PersistentClient(path="./chroma_db_" + pdf_name)
    chroma_collection = db.get_or_create_collection(
        pdf_name
    )  # For every PDF we create a new database
    return chroma_collection


def CreateVectorStore(pdf_name):
    """
    Creates a vector store and returns it.

    Returns:
        vector_store: The created vector store.
    """
    chroma_collection = CreateChromaDB(pdf_name)
    vector_store = ChromaVectorStore(chroma_collection=chroma_collection)
    return vector_store


def CreateStorageContext(pdf_name):
    """
    Creates a storage context for the vector store.

    Returns:
        storage_context (StorageContext): The created storage context.
    """
    vector_store = CreateVectorStore(pdf_name)
    storage_context = StorageContext.from_defaults(vector_store=vector_store)
    return storage_context


def CreateIndex(pdf_name, custom_chunks):
    """
    Creates a vector store index from a list of custom chunks.

    Args:
        custom_chunks (list): A list of custom chunks.

    Returns:
        VectorStoreIndex: The created vector store index.
    """
    # If there is no existing vector store create a new one and assign it to the index
    storage_context = CreateStorageContext(pdf_name)
    index = VectorStoreIndex([], storage_context=storage_context)

    documents = []
    for chunk_id, chunk in tqdm(
        enumerate(custom_chunks), total=len(custom_chunks), desc="Processing chunks"
    ):
        document = Document(
            text=chunk["text"],
            id_=chunk_id,
            metadata={
                # "block_idx": chunk.block_idx, # Not sure if needed
                # "tag": chunk.tag,
                # ,"hierarchy_level": chunk['level'] # Not sure if needed
                "page_number": chunk[
                    "page_nr"
                ],  # We add 1 to the page index to match the actual page number
                "parent_section": chunk["parent_section"],
                "parent_section_hierarchy": chunk["parent_section_hierarchy"],
            },
            metadata_seperator="\n",
            metadata_template="{key}: {value}",
            text_template="Metadata\n{metadata_str}\nContent:\n{content}",
        )
        documents.append(document)
        index.insert(document)
        # print(document.metadata)
        # print(document.text)

    return index


def LoadIndex(pdf_name):
    """
    Load the index from stored vectors on the disc.

    Returns:
        VectorStoreIndex: The loaded index.
    """
    index = VectorStoreIndex.from_vector_store(
        vector_store=CreateVectorStore(pdf_name),
        storage_context=CreateStorageContext(pdf_name),
    )

    # print(f"Index for {pdf_name} loaded.")
    # visualize_retrieved_nodes(get_retrieved_nodes("Tell me all the basic components of the et 200 sp?",
    #                                            vector_top_k=20, index=index, reranker_top_n=10, with_reranker=False))

    return index


#================================================================================================= TEST
from llama_index.core import QueryBundle
from llama_index.core.retrievers import VectorIndexRetriever
from llama_index.core.postprocessor import SimilarityPostprocessor,KeywordNodePostprocessor, MetadataReplacementPostProcessor, SentenceTransformerRerank


def get_retrieved_nodes(
    query_str, index, vector_top_k=10, reranker_top_n=5, with_reranker=False
):
    query_bundle = QueryBundle(query_str)
    # configure retriever
    retriever = VectorIndexRetriever(
        index=index,
        similarity_top_k=vector_top_k,
    )
    retrieved_nodes = retriever.retrieve(query_bundle)

    if with_reranker:
        # configure reranker
        postprocessor = SentenceTransformerRerank(model="cross-encoder/ms-marco-MiniLM-L-2-v2", top_n=reranker_top_n)       
        retrieved_nodes = postprocessor.postprocess_nodes(
            retrieved_nodes, query_bundle
        )

    return retrieved_nodes

import pandas as pd
from IPython.core.display import display, HTML

def pretty_print(df):
    return display(HTML(df.to_html().replace("\\n", "<br>")))


def visualize_retrieved_nodes(nodes) -> None:
    result_dicts = []
    for node in nodes:
        result_dict = {"Score": node.score, "Text": node.node.get_text(), "Metadata": node.metadata}
        result_dicts.append(result_dict)

    pretty_print(pd.DataFrame(result_dicts))


    # Convert to DataFrame
    df = pd.DataFrame(result_dicts)
     # Generate HTML string
    html_content = df.to_html(index=False, escape=False, classes='dataframe')
    output_file = "debugging_nodes.html"


    # Save HTML to file for external viewing
    with open(output_file, "w", encoding="utf-8") as file:
        file.write("""
        <html>
        <head>
            <style>
                body { font-family: Arial, sans-serif; }
                .dataframe { width: 100%; border-collapse: collapse; }
                .dataframe th, .dataframe td { padding: 8px; border: 1px solid #ddd; }
                .dataframe th { background-color: #f2f2f2; }
            </style>
        </head>
        <body>
            <h2>Retrieved Nodes</h2>
        """)
        file.write(html_content)
        file.write("""
        </body>
        </html>
        """)
    
    print(f"HTML file '{output_file}' has been created.")

