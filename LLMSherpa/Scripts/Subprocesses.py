import subprocess
import time

def run_docker():
    """Starts the Docker container in a separate process and handles errors if the port is already in use."""
    docker_command = [
        "docker", "run", "-p", "5010:5001",
        "ghcr.io/nlmatics/nlm-ingestor:latest"
    ]
    try:
        # Start the Docker container in a non-blocking process
        process = subprocess.Popen(docker_command, stderr=subprocess.PIPE, stdout=subprocess.PIPE, text=True)
        
        # Check for errors immediately to handle "port is already allocated"
        stdout, stderr = process.communicate(timeout=5)  # Adjust timeout as needed for the command to initialize
        if process.returncode != 0:
            if "port is already allocated" in stderr:
                print("The specified port (5010) is already in use. Docker container might already be running.")
            else:
                print(f"An error occurred when starting the Docker container: {stderr}")
        else:
            print("Docker container started successfully.")
    except subprocess.TimeoutExpired:
        print("Docker container is still starting up; check manually if needed.")
    except subprocess.CalledProcessError as e:
        print(f"An unexpected error occurred: {e}")

def run_ollama():
    """Starts the Ollama server in a separate process."""
    ollama_command = ["ollama", "serve"]
    try:
        subprocess.Popen(ollama_command)  # Non-blocking
        print("Ollama server started successfully.")
    except subprocess.CalledProcessError as e:
        print(f"Error when starting the Ollama server: {e}")

if __name__ == "__main__":
    run_docker()
    time.sleep(5)
    run_ollama()