# This finds all the tikz images and gives each one a unique index.

from bs4 import BeautifulSoup

def assign_ids_to_figures(html_file_path):
    # Read the HTML file
    with open(html_file_path, 'r', encoding='utf-8') as file:
        soup = BeautifulSoup(file, 'html.parser')

    # Initialize an index to keep track of figure divs per page
    page_figures = {}
    figure_ids = []

    # Iterate over all div elements
    for div in soup.find_all('div', class_='c'):
        # Find the parent page div
        page_div = div.find_parent('div', attrs={'data-page-no': True})

        if page_div:
            # Get the page number
            page_number = int(page_div['data-page-no'], 16)

            # Initialize index for the page if it doesn't exist
            if page_number not in page_figures:
                page_figures[page_number] = 0

            # Increment the index for this page
            page_figures[page_number] += 1
            figure_index = page_figures[page_number]

            # Create the ID string
            figure_id = f"capture_{page_number}_{figure_index}"

            # Assign the ID to the figure div
            div['capture_figure'] = figure_id
            figure_ids.append(figure_id)

    # Write the modified HTML back to a file
    # with open(html_file_path, 'w', encoding='utf-8') as file:
    # path = html_file_path.split(".")[0] + "_annotated.html"

    # with open(path, 'w', encoding='utf-8') as file:
    with open(html_file_path, 'w', encoding='utf-8') as file:
        file.write(str(soup))

    for figure_id in figure_ids:
        print(figure_id)
    return figure_ids

# # Call the function with your filename
# assign_ids_to_figures('./Input/sample16.html')
