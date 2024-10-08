from spire.pdf.common import *
from spire.pdf import *


def convert_pdf_to_html(pdf_file_path):
    # Get the file name string without extension
    pdf_file_name = pdf_file_path.split("/")[-1].split(".")[0]

    # Create a Document object
    doc = PdfDocument()

    # Load the PDF document
    doc.LoadFromFile(pdf_file_path)

    # Set the conversion options
    convertOptions = doc.ConvertOptions
    convertOptions.SetPdfToHtmlOptions(True, True, 1, True)

    # Specify the output HTML file path
    output_file_path = pdf_file_name + ".html"

    # Save the PDF document to HTML format
    doc.SaveToFile(output_file_path, FileFormat.HTML)

    # Dispose resources
    doc.Dispose()

    # Return the output file path
    return output_file_path
