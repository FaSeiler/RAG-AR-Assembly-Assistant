import os, sys
from llmsherpa.readers import LayoutPDFReader
from IPython.core.display import display, HTML

llmsherpa_api_url = (
    "http://localhost:5010/api/parseDocument?renderFormat=all&useNewIndentParser=true"
)


def ParsePDF(pdf_url):
    """
    Parses a PDF document and returns the parsed document.

    Returns:
        doc: The parsed document. https://llmsherpa.readthedocs.io/en/latest/llmsherpa.readers.html#llmsherpa.readers.layout_reader.Document
    """
    global llmsherpa_api_url
    do_ocr = True
    if do_ocr:
        llmsherpa_api_url = llmsherpa_api_url #+ "&applyOcr=yes"

    pdf_reader = LayoutPDFReader(llmsherpa_api_url)
    doc = pdf_reader.read_pdf(pdf_url)

    return doc


def WriteHTMLDocToFile(doc, filename="output.html"):
    """
    Writes an HTML document to a file.

    Parameters:
    - doc: The HTML document to be written.
    - filename: The name of the output file (default is "output.html").

    Returns:
    None
    """
    html = doc.to_html()
    with open(filename, "w", encoding="utf-8") as file:
        file.write(html)
    print(f"HTML document saved as {filename}")


def PrintChunks(doc):
    for chunk in doc.chunks():
        print(chunk.to_context_text())
        print("-" * 80)


def PrintSection(doc, section):
    for chunk in doc.chunks():
        if chunk.parent.title == section:
            print(chunk.to_context_text())
            print()
            print("-" * 80)


def PrintTables(doc):
    for table in doc.tables():
        print(table.to_text())
        print("-" * 80)


def PrintSections(doc):
    for section in doc.sections():
        print(section.to_text())
        print("-" * 80)
