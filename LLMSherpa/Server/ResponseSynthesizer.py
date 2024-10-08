from llama_index.core import get_response_synthesizer

# Configure response synthesizer: https://docs.llamaindex.ai/en/stable/module_guides/querying/response_synthesizers/

# "compact" (default):  Similar to refine but compact (concatenate) the chunks beforehand, resulting in less LLM calls.
# "refine": Create and refine an answer by sequentially going through each retrieved text chunk. This makes a separate LLM call per Node/retrieved chunk.
# "tree_summarize": Given a set of Node objects and the query, recursively construct a tree and return the root node as the response. Good for summarization purposes.

# TODO: Test of tree_summarize is better
# response_mode = "compact"
# response_mode = "refine"
# response_mode = "tree_summarize"


def GetResponseSynthesizer(response_mode: str = "compact"):
    response_synthesizer = get_response_synthesizer(
        response_mode=response_mode, streaming=False
    )
    return response_synthesizer
