import os
import fitz
import base64
import glob
from tqdm import tqdm  # pip install tqdm


def ExtractImagesFromPDF(pdf_url):
    # Create the full path to the file
    workdir = os.path.dirname(pdf_url) + "/Images/"  # -> ../data/Images/
    pdf_file_name = os.path.basename(pdf_url)
    pdf_name = os.path.splitext(pdf_file_name)[0]

    # Dictionary to hold all images
    image_dict = {}

    # Check if the file exists
    if os.path.isfile(pdf_url) and ".pdf" in pdf_file_name:
        doc = fitz.Document(pdf_url)

        # pdf_name = str(pdf_file_name[:-4])
        workdir += (
            pdf_name  # -> ../data/Images/et200sp_system_manual_en-US_en-US_stripped
        )

        for i in tqdm(range(len(doc)), desc="pages"):
            for img in tqdm(doc.get_page_images(i), desc="page_images"):
                xref = img[0]
                image = doc.extract_image(xref)
                pix = fitz.Pixmap(doc, xref)

                page_idx = str(i + 1)
                xref_idx = str(xref)

                # Create the directory if it doesn't exist
                os.makedirs(workdir, exist_ok=True)

                image_name = (
                    pdf_name + "_p" + str(page_idx) + "-" + str(xref_idx) + ".png"
                )
                image_file_path = workdir + "/" + image_name
                # print(image_file_path)
                pix.save(image_file_path)

                # Add the image to the dictionary
                image_key = pdf_name + "_" + page_idx
                if image_key in image_dict:
                    image_dict[image_key].append(image_file_path)
                else:
                    image_dict[image_key] = [image_file_path]

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
            parts = image_file_name.split("_p")
            page_idx = parts[1].split("-")[0]
            image_key = pdf_name + "_" + page_idx

            # Add the image to the dictionary
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
        print("-" * 80)


def GetBase64Image(image_path):
    """Encode an image to base64."""
    with open(image_path, "rb") as image_file:
        return base64.b64encode(image_file.read()).decode("utf-8")
