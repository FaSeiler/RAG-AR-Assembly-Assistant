import requests
import json

def send_query(query, pdf_file_name):
    url = 'http://localhost:5000/query'
    headers = {'Content-Type': 'application/json'}
    payload = {
        'query': query,
        'pdf_file_name': pdf_file_name
    }

    response = requests.post(url, headers=headers, data=json.dumps(payload))

    if response.status_code == 200:
        print("Response from server:")
        print(response.json())
        # Save the response to a file
        with open('RESPONSE.json', 'w') as file:
            json.dump(response.json(), file)
    else:
        print(f"Failed to get a response. Status code: {response.status_code}")
        print(response.text)

if __name__ == "__main__":
    query = "What are the steps for installing/mounting a BaseUnit? Include the page_numbers but no introductory sentences."
    pdf_file_name = "et200sp_system_manual_en-US_en-US_stripped.pdf"
    send_query(query, pdf_file_name)
