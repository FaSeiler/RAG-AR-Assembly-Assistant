import json
import os
from RAG import SendQueryForPDF, Init


def main():
    load_index=True
    pdf_data = Init(load_index)

    query = "How do I install a baseunit?"
    pdf_file_name = "et200sp_system_manual_en-US_en-US_stripped.pdf"
    # pdf_file_name = "Lebenslauf_Fabian_Seiler.pdf"
    SendQueryForPDF(query=query, pdf_file_name=pdf_file_name, pdf_data=pdf_data)

if __name__ == "__main__":
    main()

    