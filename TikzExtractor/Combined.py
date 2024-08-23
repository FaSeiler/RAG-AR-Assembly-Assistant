import asyncio
import os
from PdfToHtml import convert_pdf_to_html
from TikzExtractor import extract_all_svg
from HtmlNodeToImage import capture_screenshot


def main():
    input_pdf = "./Input/sample8.pdf"

    htmlFilePath = convert_pdf_to_html(input_pdf)
    extractedSvgHtmlFilePaths = extract_all_svg(htmlFilePath)

    for htmlTikzFilePath in extractedSvgHtmlFilePaths:
        # Get the absolute file path of htmlTikzFilePath
        absoluteHtmlTikzFilePath = os.path.abspath(htmlTikzFilePath)
        asyncio.run(capture_screenshot(absoluteHtmlTikzFilePath, "#capture"))

if __name__ == "__main__":
    main()
    pass