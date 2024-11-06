import asyncio
import os
import glob
import base64
from ImageExtraction.PdfToHtml import ConvertPdfToHTML
from ImageExtraction.TikzExtractor import ExtractAllSvgFilePath, ExtractPagesFilePaths
from ImageExtraction.HtmlNodeToImage import CaptureScreenshot
from ImageExtraction.PdfSplitter import SplitPdf


def ExtractAllImagesFromPDF(input_pdf):
    split_pdfs = SplitPdf(input_pdf)

    html_page_filePaths_all = []
    for counter, pdf in enumerate(split_pdfs):
        htmlFilePath = ConvertPdfToHTML(pdf, True) # Convertes the pdf to html
        print(counter, " / HTML file Path", htmlFilePath)
        html_page_filePaths = ExtractPagesFilePaths(htmlFilePath, counter, True) # Extracts the individual pages from the 10 page long html file
        html_page_filePaths_all.extend(html_page_filePaths)

    # Print count of html_page_filePaths
    print(f"Found html pages count: {len(html_page_filePaths_all)}")

    all_extracted_svg_html_filePaths = []
    for html_page_filePath in html_page_filePaths_all:
        extracted_svg_html_filePaths = ExtractAllSvgFilePath(
            html_page_filePath, True
        )
        for extracted_svg_html_filePath in extracted_svg_html_filePaths:
            all_extracted_svg_html_filePaths.append(extracted_svg_html_filePath)

    print("Found SVG pages count: ", len(all_extracted_svg_html_filePaths))
    for counter, i in enumerate(all_extracted_svg_html_filePaths):
        print(counter, " / ", i)


    image_dict = {}
    for svg_html_filePath in all_extracted_svg_html_filePaths:
        pdf_name, page_number, image_path = asyncio.run(
            CaptureScreenshot(svg_html_filePath, True)
        )

        if not pdf_name or not page_number or not image_path:
            continue

        image_key = pdf_name + "_" + str(page_number)
        print("Saving image: ", image_path)

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

            # Extract the page number from the filename
            parts = image_file_name.split("_")
            if len(parts) >= 4:
                # Page number should be the second to last number
                page_number = parts[-2]
            else:
                # If filename format is unexpected, skip this file
                print(f"Unexpected filename format: {image_file_name}")
                continue

            image_key = pdf_name + "_" + str(page_number)

            if image_key in image_dict:
                image_dict[image_key].append(image_file_path)
            else:
                image_dict[image_key] = [image_file_path]

    return image_dict


def GetImageFilePaths(image_dict, pdf_name, page_idx):

    # print("ImageExtractor.py: image_dict: ", image_dict)
    # print("ImageExtractor.py: pdf_name: ", pdf_name)
    # print("ImageExtractor.py: page_idx: ", page_idx)

    key = pdf_name + "_" + str(page_idx)

    file_names = image_dict.get(key, [])
    return file_names


def PrintImageDict(image_dict):
    for key, value in image_dict.items():
        print(key, value)
        print("-" * 80)


def GetBase64Image(image_path):
    """Encode an image to base64."""
    with open(image_path, "rb") as image_file:
        return base64.b64encode(image_file.read()).decode("utf-8")
    

# if __name__ == "__main__":
#     image_dict = ExtractAllImagesFromPDF("../data/PowerSupplyManual.pdf")
#     # image_dict = ExtractAllImagesFromPDF("../data/test.pdf")
#     print("Image dict:")
#     PrintImageDict(image_dict)
#     # image_dict = LoadImagesFromDirectory("../data/test.pdf")
#     # for key, value in image_dict.items():
#     #     print(key, value)
#     #     print("-" * 80)
