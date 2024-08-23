from bs4 import BeautifulSoup

def extract_all_svg(file_path):
    # Get the file name string without extension
    html_file_name = file_path.split("/")[-1].split(".")[0]

    # Read the HTML file
    with open(file_path, 'r', encoding='utf-8') as file:
        soup = BeautifulSoup(file, 'lxml')
    
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
    def write_to_file():
        nonlocal file_index, current_inner_gs
        if current_inner_gs:
            # Append the current inner_gs to first_level_g
            first_level_g.extend(current_inner_gs)
            # Output file name for the modified HTML
            output_file_path = f"./Output/{html_file_name}_tikz_extracted_{file_index}.html"
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
            if tag.find(True): # Check if the tag has at least one child
                current_inner_gs.append(tag)
        else:
            # If an interrupting tag is found, save the current content and reset
            write_to_file()
            first_level_g.clear()  # Clear the first_level_g for the next set
            current_inner_gs = []

    # After the loop, make sure to write any remaining content
    write_to_file()

    print("HTML processing complete.")

    return created_files

# # Path to the input HTML file
# input_html_file = "./Output/sample5_html.html"

# # Process the SVG in the HTML file
# extract_svg(input_html_file)
