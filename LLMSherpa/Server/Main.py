from DocumentParser import *
from ParserPostprocessor import *
from VectorStoreChromaDB import CreateIndex, LoadIndex
from RetrieverPostprocessor import *
from RAG import *
from Utility import *


def main():
    load_index = True
    InitializeLlamaIndex()

    if load_index:
        index = LoadIndex()
        print("Index loaded.")
    else:
        doc = ParsePDF()
        # PrintChunks(doc)
        custom_chunks = MergeChunks(doc)
        for chunk in custom_chunks:
            PrintCustomChunk(chunk)
        index = CreateIndex(custom_chunks)
        print("Index created.")


    # # Your code here
    # doc = ParsePDF()
    # PrintChunks(doc)

    # custom_chunks = MergeChunks(doc)
    # for chunk in custom_chunks:
    #     PrintCustomChunk(chunk)

    # InitializeLlamaIndex()
    # index = CreateIndex(custom_chunks)
    # # index = LoadIndex() # Load the index from disk if it already exists

    # VisualizeRetrievedNodesToFile(TestNodeReranking(index, "Tell me all the basic components of the et 200 sp?", vector_top_k=20, reranker_top_n=10, with_reranker=True))

    print()
    print("="*100)
    query = "What are the steps for installing/mounting a BaseUnit? Do not include any introductory sentences."
    print(f"User Query: {query}")
    print("="*100)

    print()
    print("Querying the index...")

    query_engine = GetQueryEngine(index)
    response = query_engine.query(query)
    # response = query_engine.query("Tell me the Website URL of Fabian Seiler.")

    VisualizeRetrievedNodesToFile(nodes=response.source_nodes)

    print()
    print("="*100)
    print("Response:")
    print(response)
    print("="*100)

if __name__ == "__main__":
    main()