from langchain_community.embeddings.ollama import OllamaEmbeddings

def get_embedding_function():
    # embeddings = BedrockEmbeddings(
    #     credentials_profile_name="default", region_name="us-east-1"
    # )
    embeddings = OllamaEmbeddings(model="BAAI/bge-small-en-v1.5", show_progress=True)
    # embeddings = OllamaEmbeddings(model="mxbai-embed-large", show_progress=True)
    return embeddings
