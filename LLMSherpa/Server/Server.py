import os
import base64
import json
from flask import Flask, request, jsonify
from RAG import SendQueryForPDF, Init

app = Flask(__name__)

# Initialize load_index and pdf_data on startup
load_index = True
pdf_data = Init(load_index)




@app.route('/query', methods=['POST'])
def handle_query():
    # Get data from the request
    data = request.get_json()
    query = data.get('query')
    pdf_file_name = data.get('pdf_file_name')

    # Check if the necessary fields are provided
    if not query or not pdf_file_name:
        return jsonify({"error": "query and pdf_file_name are required"}), 400

    # Perform the query
    response_json = SendQueryForPDF(query=query, pdf_file_name=pdf_file_name, pdf_data=pdf_data)

    return response_json

if __name__ == "__main__":
    app.run(host='0.0.0.0', port=5000)
