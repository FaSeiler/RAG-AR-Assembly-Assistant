import asyncio
import os
from PdfSplitter import split_pdf
from PdfToHtml import convert_pdf_to_html
from TikzExtractor import extract_all_svg_filePath, extract_pages_filePaths
from HtmlNodeToImage import capture_screenshot


def extract_all_images_from_PDF(input_pdf):
    split_pdfs = split_pdf(input_pdf, False)

    for counter, pdf in enumerate(split_pdfs):
        htmlFilePath = convert_pdf_to_html(pdf, counter)
        html_page_filePaths = extract_pages_filePaths(htmlFilePath, counter, False)

        # Print count of html_page_filePaths
        print(f"Found html pages count: {len(html_page_filePaths)}")

        all_extracted_svg_html_filePaths = []
        for html_page_filePath in html_page_filePaths:
            extracted_svg_html_filePaths = extract_all_svg_filePath(html_page_filePath, False)

            for extracted_svg_html_filePath in extracted_svg_html_filePaths:
                all_extracted_svg_html_filePaths.append(extracted_svg_html_filePath)


        for svg_html_filePath in all_extracted_svg_html_filePaths:
            asyncio.run(capture_screenshot(svg_html_filePath, False))

def main():
    input_pdf = "./Input/et200sp_system_manual_en-US_en-US_stripped.pdf"
    extract_all_images_from_PDF(input_pdf)



if __name__ == "__main__":
    main()
    pass