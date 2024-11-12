
# from HtmlNodeToImage import capture_screenshot
import asyncio
import os
from Pdf2HtmlEx import convert_pdf_to_html
from FigureAnnotator import assign_ids_to_figures
from PageSplitter import split_html_by_page_with_capture_figure
from CaptureHtmlNodeImage import capture_screenshot

def main():
    pdf_file_path = "./Input/sample.pdf"
    html_file_path = convert_pdf_to_html(pdf_file_path)
    assign_ids_to_figures(html_file_path)

    filepaths_and_capture_figures = split_html_by_page_with_capture_figure(html_file_path)
    for html_file_path, capture_figure_values in filepaths_and_capture_figures.items():
        for element_selector in capture_figure_values:
            # Run the capture_screenshot function for each figure
            print(f"Capturing screenshot for {element_selector} in {html_file_path}")
            asyncio.run(capture_screenshot(html_file_path, element_selector))
        # Remove the html_file after all capture_figure_values for that file are processed
        # os.remove(html_file_path)

        
if __name__ == "__main__":
    main()
    pass