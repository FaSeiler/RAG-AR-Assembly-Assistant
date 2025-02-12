from flask import Flask, request, jsonify
from RAG import SendQueryForPDF, Init
from Subprocesses import run_docker, run_ollama
import time
import sys

app = Flask(__name__)

# Read the load_index argument from the command line
if len(sys.argv) > 1:
    load_index = sys.argv[1].lower() in ("true", "1", "t")
else:
    load_index = False  # Default value if no argument is provided

pdf_data = Init(load_index)


@app.route("/test", methods=["GET"])
def test_connection():
    return jsonify({"message": "Connection successful!"}), 200


@app.route("/query", methods=["POST"])
def handle_query():
    # Get data from the request
    data = request.get_json()
    query = data.get("query")
    pdf_file_name = data.get("pdf_file_name")

    # Check if the necessary fields are provided
    if not query or not pdf_file_name:
        return jsonify({"error": "query and pdf_file_name are required"}), 400

    # Perform the query
    response_json = SendQueryForPDF(
        query=query, pdf_file_name=pdf_file_name, pdf_data=pdf_data
    )

    return response_json


if __name__ == "__main__":
    app.run(host="0.0.0.0", port=5000)