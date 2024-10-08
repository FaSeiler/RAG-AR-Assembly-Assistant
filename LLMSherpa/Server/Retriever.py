from llama_index.core.retrievers import VectorIndexRetriever
from RetrieverPostprocessor import CreateMetadataFilters

top_k_nodes = 25  # Number of nodes to retrieve from the index


def GetRetriever(index):
    # configure retriever
    retriever = VectorIndexRetriever(
        index=index,
        similarity_top_k=top_k_nodes,
        # filters=CreateMetadataFilters()
    )
    return retriever
