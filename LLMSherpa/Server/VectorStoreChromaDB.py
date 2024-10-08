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
    return index
