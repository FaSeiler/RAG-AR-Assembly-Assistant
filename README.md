# Automatic AR Assembly Instructions using RAG and a LLM

This project parses PDF manuals of Siemens SIMATIC components and automatically generates assembly instructions through the use of a Retrieval Augmented Generation (RAG) system. Instructions are superimposed onto the real world using Augmented Reality (AR) and the Unity engine.

## Installation

1. **Clone the repository**

2. **Install the Python (3.9.10) dependencies:**

- Open the directory RAG, where the file `requirements.txt` is located, and run the following command:
   
   ```sh
    pip install --no-deps -r requirements.txt
    ```
- Run the following command to install Chromium with Playwright:
  
  ```sh
  playwright install
  ```
    
4. **Install the nlm-ingestor:**
   
    The nlm-ingestor is used to parse the PDF documents used as input for the instruction generation. Install the docker container as described here: [nlm-ingestor](https://github.com/nlmatics/nlm-ingestor)
    ```sh
    docker pull ghcr.io/nlmatics/nlm-ingestor:latest
    ```
   
6. **Install Ollama and download LLama3.1:**

   - Install Ollama from: [Ollama](https://ollama.com/)

   - Download [Llama3.1_8B](https://ollama.com/library/llama3.1) by running the following command:
 
     ```sh
      ollama pull llama3.1
      ```

8. **Open the Unity Project:**

   Open the project folder `AssemblyAssistant` in Unity



## Usage
### Starting the RAG Server

 **Automatic startup with Docker (Recommended):**

   If you used the docker image to install the nlm-ingestor you can start the docker container and the Ollama server by running the following command:
  ```sh
  py Subprocesses.py
  ```

 **Manual startup without Docker:**
    
  - If you didn't use the docker container to install the nlm-ingestor start it as described on the ingestor GitHub page:
    
    https://github.com/nlmatics/nlm-ingestor

  - And manually start the Ollama server with the following command:
     ```sh
      ollama serve
      ```
### Testing the RAG Server
You can test the running server by executing the `Client.py` script with a sample request:
```sh
py Client.py "What is the difference between a white and a grey base unit?"
```
