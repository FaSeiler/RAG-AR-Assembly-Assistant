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

**Hint:** If you do not want to use Docker-Desktop, then install docker in WSL and run the docker container for the ingestor in WSL. Check the Subprocesses.py for the required docker commands. `docker run -p 5010:5001 ghcr.io/nlmatics/nlm-ingestor:latest`

   And start the RAG server:
   ```sh
  py Server.py
   - True: use previous ingestion
   - False or no input: Start new and ingest the PDF again. It is best if you delete the folder with name "chroma_db_et200sp_system_manual_en-US_en-US_stripped".
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

It is also possible to send a request to the flask server's IP address (that is received after the Server.py is called). When running on the same machine you can try the following: `localhost:5000/test`.

### WLAN and Firewall Setup

**WLAN**: Go to the properties of the connected WLAN and set the Network Profile to Private

**Firewall**: Add Inbound/Outbound rules to the firewall for port 5000. If this automatically does not work also disable the firewall completely.

### Unity Setup
- Open the project folder `AssemblyAssistant` in Unity
- Open Unity and click "ignore" when asked to enter safe mode
- Create a Vuforia Account: https://developer.vuforia.com
- In your Vuforia account, navigate to `Plan & Licenses` and generate a `Basic License`
- Download the Vuforia SDK unity package: https://developer.vuforia.com/downloads/sdk
- Import the Vuforia unity package
- Enter your Vuforia license key: For that, in the project hierarchy in Unity expand `Regular` and click on the GameObject `ARCamera`. Then open the `Vuforia Engine Configuration` on the Vuforia Behavior component and enter the key.
- Open the scene named `Main` and build using Android settings. Then you can upload the `AssemblyAssistant.apk` file to the Android device and install the app.
- In the `AssemblyAssistant` app's settings enter the IP address and the port of the flask server. 

