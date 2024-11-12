from bs4 import BeautifulSoup
import datetime
import os


def ExtractPagesFilePaths(html_file_path, counter, remove_original_after=True):
    # Read the HTML file
    with open(html_file_path, 'r', encoding='utf-8') as file:
        content = file.read()

    # Get the file name from the path without the extension
    file_name = os.path.splitext(os.path.basename(html_file_path))[0].split("_part_")[0]

    # Parse the HTML
    soup = BeautifulSoup(content, 'lxml')
    
    # Find all SVG elements
    svgs = soup.find_all('svg')
    
    html_page_filePaths = []
    # Iterate over all SVG elements and create a new BeautifulSoup object for each
    for index, svg in enumerate(svgs):
        # Create a new BeautifulSoup object with only the current SVG element
        new_soup = BeautifulSoup('<!DOCTYPE html><html><head><meta charset="utf-8"><title>SVG Page</title></head><body style="margin:0', 'lxml')
        new_soup.body.append(svg)

        # Append the new soup to the list
        
        # Create a file path for each new soup object
        # print("Path: ", html_file_path, "\nCounter: ", counter, "\nIndex: ", index, "\n====================")
        output_file_path = os.path.join(f'{file_name}_page_{(counter * 10) + index + 1}.html')
        
        # Write the new BeautifulSoup object to a file
        with open(output_file_path, 'w', encoding='utf-8') as file:
            file.write(str(new_soup))

        html_page_filePaths.append(output_file_path)

    print(f'Successfully saved {len(svgs)} SVG pages to {output_file_path}')
    
    if remove_original_after:
        os.remove(html_file_path)

    return html_page_filePaths

def ExtractAllSvgFilePath(file_path, remove_original_after=True):
    # Get the file name string without extension
    html_file_name = file_path.split("/")[-1].split(".")[0]

    # Read the HTML file
    with open(file_path, 'r', encoding='utf-8') as file:
        soup = BeautifulSoup(file, 'lxml')
    
    if remove_original_after:
        os.remove(file_path)

    # Find the <svg> tag
    svg = soup.find('svg')
    
    if not svg:
        print("No <svg> tag found in the file.")
        return
    
    # Find the first level <g>
    first_level_g = svg.find('g')
    
    if not first_level_g:
        print("No first level <g> tag found in the <svg>.")
        return
    
    # Find the second level <g>
    second_level_g = first_level_g.find('g')
    
    if not second_level_g:
        print("No second level <g> tag found inside the first level <g>.")
        return
    
    # Remove all attributes from the first <g> (including any transform)
    first_level_g.attrs = {}

    # Clear the first level <g> content (remove everything inside it)
    first_level_g.clear()

    # Add the id attribute to the first <g>
    first_level_g['id'] = 'capture'
    
    # Initialize variables
    current_inner_gs = []
    file_index = 1
    created_files = []  # List to store paths of created files

    # Function to write out the current content to a file
    def WriteToFile():
        nonlocal file_index, current_inner_gs
        if current_inner_gs:
            # Append the current inner_gs to first_level_g
            first_level_g.extend(current_inner_gs)
            # Output file name for the modified HTML
            output_file_path = f"./{html_file_name}_{file_index}.html"
            # Write the modified HTML to a new file
            with open(output_file_path, 'w', encoding='utf-8') as file:
                file.write(soup.prettify())
            print(f"HTML part {file_index} saved to {output_file_path}")
            # Increment the file index for the next output file
            file_index += 1
            # Clear current inner_gs
            current_inner_gs = []
            # Add the path of the created file to the list
            created_files.append(output_file_path)

    # Now, move the contents of the second <g> to the first <g> position
    for tag in second_level_g.find_all(recursive=False):
        if tag.name == 'g':
            if tag.find(True):  # Check if the tag has at least one child
                # Check if the tag does not only have children tags of type <text>
                if not all(child.name == 'text' for child in tag.find_all(recursive=False)):
                    current_inner_gs.append(tag)
        else:
            # If an interrupting tag is found, save the current content and reset
            WriteToFile()
            first_level_g.clear()  # Clear the first_level_g for the next set
            current_inner_gs = []

    # After the loop, make sure to write any remaining content
    WriteToFile()

    print("HTML processing complete.")

    return created_files