from llama_index.llms.ollama import Ollama
from llama_index.embeddings.huggingface import HuggingFaceEmbedding
from llama_index.core import Settings
from llama_index.core.query_engine import RetrieverQueryEngine
from Retriever import GetRetriever
from ResponseSynthesizer import GetResponseSynthesizer
from RetrieverPostprocessor import GetNodePostprocessors
from CustomPromptTemplates import *


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





