from llama_index.core.vector_stores.types import MetadataFilters, MetadataFilter
from llama_index.core import QueryBundle
from llama_index.core.retrievers import VectorIndexRetriever
from llama_index.core.postprocessor import SentenceTransformerRerank

top_k_reranked = 10  # Number of nodes to return after reranking


# Not used
def CreateMetadataFilters():
    """
    Create metadata filters based on included section keywords.

    Returns:
        MetadataFilters: The created metadata filters.
    """
    included_section_keywords = [
        "Introduction",
        "Requirement",
        "Required tools",
        "Installing",
        "Preparing",
        "Mounting",
        "Attaching",
        "Dismantling" "Removing",
        "Disassembling",
        "Uninstalling",
        "Tools required",
        "Required accessories",
        "Connecting",
        "Plugging",
        "Releasing",
        "Removing",
    ]

    filters = filters = [
        MetadataFilter(key="parent_section", value=section, operator="==")
        for section in included_section_keywords
    ]

    metadataFilters = MetadataFilters(filters=filters, condition="or")
    return metadataFilters


# Sample: VisualizeRetrievedNodesToFile(TestNodeReranking(index, "Tell me all the basic components of the et 200 sp?", vector_top_k=20, reranker_top_n=10, with_reranker=True))
def TestNodeReranking(
    index, query_str, vector_top_k=10, reranker_top_n=5, with_reranker=False
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
        postprocessor = SentenceTransformerRerank(
            model="cross-encoder/ms-marco-MiniLM-L-2-v2", top_n=reranker_top_n
        )
        retrieved_nodes = postprocessor.postprocess_nodes(retrieved_nodes, query_bundle)

    return retrieved_nodes


def GetNodePostprocessors():
    return [
        SentenceTransformerRerank(
            model="cross-encoder/ms-marco-MiniLM-L-2-v2", top_n=top_k_reranked
        )
        # KeywordNodePostprocessor(
        #     required_keywords=["Basic component"]#, exclude_keywords=["Italy"]
        #     # required_keywords=[]
        # ),
        # SimilarityPostprocessor(similarity_cutoff=0.4),
        # MetadataReplacementPostProcessor(target_metadata_key="window")
    ]
