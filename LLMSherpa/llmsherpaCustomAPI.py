import os, sys
from llmsherpa.readers import LayoutPDFReader
from IPython.core.display import display, HTML


def GetDoc():
    directory_path = "/Users/fabia/Desktop/testapi"
    sys.path.insert(0, directory_path)
    llmsherpa_api_url = "http://localhost:5001/api/parseDocument?renderFormat=all&useNewIndentParser=true"
    pdf_url = "/Users/fabia/Desktop/testapi/test_manual.pdf"

    do_ocr = True
    if do_ocr:
        llmsherpa_api_url = llmsherpa_api_url + "&applyOcr=yes"
    pdf_reader = LayoutPDFReader(llmsherpa_api_url)
    doc = pdf_reader.read_pdf(pdf_url)
    return doc