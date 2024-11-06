import subprocess
import os
from bs4 import BeautifulSoup

def convert_pdf_to_html(pdf_file_path):
    # Define paths
    pdf_directory = os.path.dirname(pdf_file_path) # e.g. "./Input/sample16.pdf" -> "./Input"
    pdf_file = os.path.basename(pdf_file_path) # e.g. "./Input/sample16.pdf" -> "sample16.pdf"
    pdf_directory_absolute = os.path.abspath(pdf_directory) # e.g. "./Input" -> "C:/Users/fabia/Desktop/MasterThesisRepo/TikzExtractor/Input"
    docker_image = "pdf2htmlex/pdf2htmlex:0.18.8.rc2-master-20200820-alpine-3.12.0-x86_64"
    zoom_level = "2"

    # Create the Docker command
    docker_command = [
        "docker", "run", "-ti", "--rm",
        "-v", f"{pdf_directory_absolute}:/pdf",
        "-w", "/pdf",
        docker_image,
        "--zoom", zoom_level,
        pdf_file,
        "--process-outline", "0",
        # "--font-format", "ttf",
        # "--embed-font", "0",
        # "--embed-image", "0", # This could be interesting...
        # "--embed-javascript", "0",
        # "--split-pages", "1",
    ]

    # Run the Docker command
    try:
        subprocess.run(docker_command, check=True)
        print("PDF conversion completed successfully.")
        html_file = os.path.splitext(pdf_file)[0] + ".html"
        html_file_path = os.path.join(pdf_directory_absolute, html_file)
        # process_html_file(html_file_path)
        return html_file_path
    except subprocess.CalledProcessError as e:
        print(f"An error occurred: {e}")
        return None