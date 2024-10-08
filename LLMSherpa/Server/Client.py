import requests
import json
import sys
import time


def send_query(query, pdf_file_name):
    url = "http://localhost:5000/query"
    headers = {"Content-Type": "application/json"}
    payload = {"query": query, "pdf_file_name": pdf_file_name}

    start_time = time.time()
    response = requests.post(url, headers=headers, data=json.dumps(payload))
    
    if response.status_code == 200:
        print("Response from server:")
        print(response.json())
        # Save the response to a file
        with open("RESPONSE.json", "w") as file:
            json.dump(response.json(), file)
    else:
        print(f"Failed to get a response. Status code: {response.status_code}")
        print(response.text)

    end_time = time.time()
    execution_time = end_time - start_time
    print(f"Execution time: {execution_time} seconds")  # Print the execution time


if __name__ == "__main__":
    pdf_file_name = "et200sp_system_manual_en-US_en-US_stripped.pdf"
    query = "What is the general number of I/O modules in an ET 200SP System?"

    if len(sys.argv) >= 2:
        query = sys.argv[1]

    send_query(query, pdf_file_name)
