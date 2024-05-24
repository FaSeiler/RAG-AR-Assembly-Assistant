import requests

BASE_URL = 'http://192.168.0.110:5000'

def say_hello():
    response = requests.get(f'{BASE_URL}/hello')
    return response.text

def send_query(question, database, useContext=True, k_context=5):
    data = {
        'question': question,
        'useContext': useContext,
        'database': database,
        'k_context': k_context
    }
    response = requests.post(f'{BASE_URL}/query', json=data)
    if response.status_code == 200:
        return response.json()
    else:
        print(f'Error: {response.json()["error"]}')
        return None

def main():
    print(say_hello())  # prints "Hello Fabian"

    result = send_query('What cables can I use to connect the base unit?', 'et200sp_system_manual_en-US_en-US_stripped')
    if result is not None:
        print(result)

if __name__ == "__main__":
    main()