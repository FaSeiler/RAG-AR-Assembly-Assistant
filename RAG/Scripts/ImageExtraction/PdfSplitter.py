import PyPDF2
import os

def SplitPdf(filepath, remove_original_after=False):
    # Create a PdfReader object
    pdf_reader = PyPDF2.PdfReader(filepath)
    total_pages = len(pdf_reader.pages)
    
    # Define output directory and ensure it exists
    output_dir = "split_pdfs"
    os.makedirs(output_dir, exist_ok=True)
    
    # List to store the paths of the split PDFs
    split_files = []
    
    # Iterate over the pages in chunks of 10
    for start_page in range(0, total_pages, 10):
        pdf_writer = PyPDF2.PdfWriter()
        end_page = min(start_page + 10, total_pages)
        
        for page_num in range(start_page, end_page):
            pdf_writer.add_page(pdf_reader.pages[page_num])
        
        # Define output filepath
        split_filename = f"{os.path.splitext(os.path.basename(filepath))[0]}_part_{start_page // 10 + 1}.pdf"
        split_filepath = os.path.join(output_dir, split_filename)
        
        # Write the split PDF to the output directory
        with open(split_filepath, "wb") as output_pdf:
            pdf_writer.write(output_pdf)
        
        # Add the filepath to the list
        split_files.append(split_filepath)
    
    # Remove the original PDF file if specified
    if remove_original_after:
        os.remove(filepath)

    return split_files