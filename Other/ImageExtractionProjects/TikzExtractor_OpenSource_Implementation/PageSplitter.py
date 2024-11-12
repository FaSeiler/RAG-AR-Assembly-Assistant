from bs4 import BeautifulSoup
import os

def split_html_by_page_with_capture_figure(filepath):
    # Read the input HTML file
    with open(filepath, 'r', encoding='utf-8') as file:
        html_content = file.read()
    
    # Parse the HTML content
    soup = BeautifulSoup(html_content, 'html.parser')

    # Initialize dictionary to keep track of output file paths and capture_figure values
    result = {}

    # Find the body and page container
    page_container = soup.find('div', id='page-container')

    # If the page container does not exist, raise an error
    if not page_container:
        raise ValueError("No page-container div found in the HTML file.")

    # Iterate over each div with a data-page-no attribute
    page_divs = page_container.find_all('div', attrs={'data-page-no': True})
    for page_div in page_divs:
        page_no = int(page_div['data-page-no'], 16)
        
        # Check if there's any <div> with the attribute capture_figure under the current page div
        capture_figure_divs = page_div.find_all('div', attrs={'capture_figure': True})
        
        if capture_figure_divs:
            # Extract capture_figure attribute values
            capture_figure_values = [div['capture_figure'] for div in capture_figure_divs]
            
            # Create a new BeautifulSoup object for the new page
            new_soup = BeautifulSoup(str(soup), 'html.parser')
            
            # Clear existing page content
            new_page_container = new_soup.find('div', id='page-container')
            new_page_container.clear()
            
            # Copy the specific page div into the new page container
            new_page_container.append(page_div)
            
            # Build the HTML structure for the new page
            new_html_content = str(new_soup)
            
            # Define the output file path
            output_filepath = f"./Output/page_{page_no}.html"
            
            # Ensure output directory exists
            os.makedirs(os.path.dirname(output_filepath), exist_ok=True)
            
            # Write the new HTML content to the file
            with open(output_filepath, 'w', encoding='utf-8') as output_file:
                output_file.write(new_html_content)
            
            # Add the file path and capture_figure values to the result dictionary
            result[output_filepath] = capture_figure_values
    
    return result

# Example usage
# filepaths_and_capture_figures = split_html_by_page_with_capture_figure('./Input/sample16.html')
# print("Created files and capture_figure values:", filepaths_and_capture_figures)
