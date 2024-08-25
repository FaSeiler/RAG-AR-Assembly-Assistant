import asyncio
import os
import glob
import base64
from PdfToHtml import convert_pdf_to_html
from TikzExtractor import extract_all_svg_filePath, extract_pages_filePaths
from HtmlNodeToImage import capture_screenshot


def extract_all_images_from_PDF(input_pdf):
    htmlFilePath = convert_pdf_to_html(input_pdf)

    html_page_filePaths = extract_pages_filePaths(htmlFilePath, False)

    # Print count of html_page_filePaths
    print(f"Found html pages count: {len(html_page_filePaths)}")

    all_extracted_svg_html_filePaths = []
    for html_page_filePath in html_page_filePaths:
        extracted_svg_html_filePaths = extract_all_svg_filePath(html_page_filePath, False)

        for extracted_svg_html_filePath in extracted_svg_html_filePaths:
            all_extracted_svg_html_filePaths.append(extracted_svg_html_filePath)

    image_dict = {}
    for svg_html_filePath in all_extracted_svg_html_filePaths:
        pdf_name, page_number, image_path = asyncio.run(capture_screenshot(svg_html_filePath, False))
        
        image_key = pdf_name + "_" + str(page_number)
        # print("Key: ", image_key, "\nValue:", image_path)

        if image_key in image_dict:
            image_dict[image_key].append(image_path)
        else:
            image_dict[image_key] = [image_path]

    print("Done extracting images from the PDF file!")

    return image_dict

def LoadImagesFromDirectory(pdf_url):
    # Create the full path to the directory
    workdir = os.path.dirname(pdf_url) + "/Images/"
    pdf_file_name = os.path.basename(pdf_url)
    pdf_name = os.path.splitext(pdf_file_name)[0]
    workdir += pdf_name

    # Dictionary to hold all images
    image_dict = {}

    # Check if the directory exists
    if os.path.isdir(workdir):
        # Iterate over all PNG files in the directory
        for image_file_path in glob.glob(workdir + "/*.png"):
            # Extract page index and xref index from the file name
            image_file_name = os.path.basename(image_file_path)


            parts = image_file_name.split('_')
            page_number = parts[2]

            print("Pdf name: ", pdf_name)
            print("Page number: ", page_number)
            print("Image file path: ", image_file_path)

            image_key = pdf_name + "_" + str(page_number)
            # print("Key: ", image_key, "\nValue:", image_path)

            if image_key in image_dict:
                image_dict[image_key].append(image_file_path)
            else:
                image_dict[image_key] = [image_file_path]

    return image_dict

def GetImageFilePaths(image_dict, pdf_name, page_idx):
    # Create the key from the pdf_name and page_idx
    key = pdf_name + "_" + str(page_idx)
    
    # Return the corresponding file_names from the dictionary
    return image_dict.get(key, [])

def PrintImageDict(image_dict):
    for key, value in image_dict.items():
        print(key, value)
        print("-"*80)

def GetBase64Image(image_path):
    """Encode an image to base64."""
    with open(image_path, "rb") as image_file:
        return base64.b64encode(image_file.read()).decode('utf-8')