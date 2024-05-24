from flask import Flask, request, jsonify
import query_data

app = Flask(__name__)

@app.route('/hello', methods=['GET'])
def hello():
    return "Hello Fabian"

@app.route('/query', methods=['POST'])
def query():
    data = request.get_json()
    question = data.get('question')
    useContext = data.get('useContext', True)
    database = data.get('database')
    k_context = data.get('k_context', 5)
    
    if not all([question, database]):
        return jsonify({'error': 'Missing required parameters'}), 400

    result = query_data.query_rag(question, useContext, database, k_context)
    return jsonify(result)

if __name__ == '__main__':
    app.run(host='192.168.0.110', port=5000, debug=True)
    # app.run(host='127.0.0.1', port=5000, debug=True)
