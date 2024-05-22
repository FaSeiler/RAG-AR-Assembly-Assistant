# from flask import Flask, request, jsonify
# import query_data

# app = Flask(__name__)

# @app.route('/query', methods=['POST'])
# def query():
#     data = request.get_json()
#     query_text = data.get('query_text')
#     if query_text is None:
#         return jsonify({'error': 'No query_text field provided.'}), 400
#     response_text = query_data.query_rag(query_text)
#     return jsonify({'response_text': response_text})

# if __name__ == '__main__':
#     app.run(host='0.0.0.0', port=5000) 

from flask import Flask, request, jsonify
import query_data

app = Flask(__name__)

@app.route('/query', methods=['POST'])
def query():
    data = request.get_json()
    query_text = data.get('query_text')
    ignoreContext = data.get('ignoreContext', False)
    if query_text is None:
        return jsonify({'error': 'No query_text field provided.'}), 400
    response_text = query_data.query_rag(query_text, not ignoreContext)
    return jsonify({'response_text': response_text})

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)