import subprocess
import os
import time

def Start_NLM_Ingestor_OllamaServe():
    # Define the relative paths
    jar_path = os.path.join("../", "nlm-ingestor-main", "jars", "tika-server-standard-nlm-modified-2.4.1_v6.jar")

    # Start the Java server
    print("Starting Java server...")
    subprocess.Popen(['cmd', '/k', f'java -jar {jar_path}'])
    time.sleep(2)  # Wait for 2 seconds

    # Start the Python ingestion daemon
    print("Starting Python ingestion daemon...")
    subprocess.Popen(['cmd', '/k', 'python -m nlm_ingestor.ingestion_daemon'])
    time.sleep(8)  # Wait for 6 seconds

    # Start the Ollama server
    print("Starting Ollama serve...")
    subprocess.Popen(['cmd', '/k', 'ollama serve'])

def Start_NLM_Ingestor_OllamaServe_New_Windows():
    # Define the relative paths
    jar_path = os.path.join("../", "nlm-ingestor-main", "jars", "tika-server-standard-nlm-modified-2.4.1_v6.jar")

    # Start the Java server
    print("Starting Java server...")
    subprocess.Popen(['start', 'cmd', '/k', f'java -jar {jar_path}'], shell=True)
    time.sleep(2)  # Wait for 2 seconds

    # Start the Python ingestion daemon
    print("Starting Python ingestion daemon...")
    subprocess.Popen(['start', 'cmd', '/k', 'python -m nlm_ingestor.ingestion_daemon'], shell=True)
    time.sleep(8)  # Wait for 6 seconds

    # Start the Ollama server
    print("Starting Ollama serve...")
    subprocess.Popen(['start', 'cmd', '/k', 'ollama serve'], shell=True)


if __name__ == "__main__":
    # Start_NLM_Ingestor_OllamaServe()
    Start_NLM_Ingestor_OllamaServe_New_Windows()


    
