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
   
    The nlm-ingestor is used to parse the PDF documents used as input for the instruction generation. Install the docker container as described here: [nlm-ingestor](https://github.com/nlmatics/nlm-ingestor) (v0.1.9)
    ```sh
    docker pull ghcr.io/nlmatics/nlm-ingestor:latest
    ```
   
6. **Install Ollama and download LLama3.1:**

   - Install Ollama from: [Ollama](https://ollama.com/)

   - Download [Llama3.1_8B](https://ollama.com/library/llama3.1) by running the following command:
 
     ```sh
      ollama pull llama3.1
      ```


## Usage
### Starting the RAG Server

 **Automatic startup with Docker (Recommended):**

   If you used the docker image to install the nlm-ingestor you can start the docker container and the Ollama server by running the following command:
  ```sh
  py Subprocesses.py
  ```
   And start the RAG server:
   ```sh
  py Server.py
  ```


 **Manual startup without Docker:**
    
  - If you didn't use the docker container to install the nlm-ingestor start it as described on the ingestor GitHub page:
    
    https://github.com/nlmatics/nlm-ingestor

  - And manually start the Ollama server with the following command:
     ```sh
      ollama serve
      ```
  - And start the RAG server:
   ```sh
  py Server.py
  ```
### Testing the RAG Server
You can test the running server by executing the `Client.py` script with a sample request:
```sh
py Client.py "What is the difference between a white and a grey base unit?"
```


### Unity Setup
- Open the project folder `AssemblyAssistant` in Unity
- Open Unity and click "ignore" when asked to enter safe mode
- Download the Vuforia SDK unity package: https://developer.vuforia.com/downloads/sdk
- Open the Vuforia unity package and import it
- Reference the extracted Vuforia .tgz file in the package `manifest.json`.
  For this, you must add the file path of the file `com.ptc.vuforia.engine-10.28.4` in the package manifest. You can find the manifest in the directory `AssemblyAssistant/Packages/`.
  The .tgz file is located in the directory `Assets/Editor/Migration/`. The referenced file path in the manifest can look like this:
  ```json
   "com.ptc.vuforia.engine": "file:C:/Users/User/Desktop/MasterThesisRepo/AssemblyAssistant/Assets/Editor/Migration/com.ptc.vuforia.engine-10.28.4.tgz",
   ```
- After that, confirm the Vuforia License agreement and add your license key

