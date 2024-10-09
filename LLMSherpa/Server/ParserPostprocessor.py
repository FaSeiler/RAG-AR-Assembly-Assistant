def GetChunkParentSectionTitles(chunk):
    # Build a string that consists of recursively getting the chunk parent titles
    parent_titles = ""
    parent = chunk.parent
    while parent:
        try:
            parent_titles = parent.title + " > " + parent_titles
            parent = parent.parent
        except Exception as e:
            break
    # Remove the trailing " >"
    parent_titles = parent_titles.rstrip(" >")
    return parent_titles


# We want the full context_text without the section title. The section title is included in the metadata
def RemoveFirstLine(text):
    lines = text.splitlines(True)[1:]  # splitlines(True) keeps the line breaks
    return "".join(lines)


def MergeChunks(doc):
    # Dictionary to store merged content
    merged_chunks = {}

    # Iterate through each chunk and merge content
    for chunk in doc.chunks():
        parent_section_hierarchy = GetChunkParentSectionTitles(chunk)
        parent_section = chunk.parent.title
        text = RemoveFirstLine(chunk.to_context_text())
        page_idx = chunk.page_idx + 1
        level = chunk.level

        if parent_section_hierarchy not in merged_chunks:
            merged_chunks[parent_section_hierarchy] = {
                "text": text,
                "start_page": page_idx,
                "end_page": page_idx,
                "parent_section": parent_section,
                "level": level,
            }
        else:
            merged_chunks[parent_section_hierarchy]["text"] += "\n" + text
            merged_chunks[parent_section_hierarchy]["end_page"] = max(
                merged_chunks[parent_section_hierarchy]["end_page"], page_idx
            )

    # Convert the merged dictionary back to list of chunks with updated page numbers
    final_chunks = []
    for parent_section_hierarchy, data in merged_chunks.items():
        start_page = data["start_page"]
        end_page = data["end_page"]
        page_nr = (
            str(start_page) if start_page == end_page else f"{start_page}-{end_page}"
        )

        final_chunks.append(
            {
                "parent_section_hierarchy": parent_section_hierarchy,
                "text": data["text"],
                "page_nr": page_nr,
                "parent_section": data["parent_section"],
                "level": data["level"],
            }
        )

    return final_chunks


def GetCustomChunkString(chunk):
    output = []
    
    output.append(f"Page Nr: {chunk['page_nr']}")
    output.append(f"Parent Section: {chunk['parent_section']}")
    output.append(f"Parent Section Hierarchy: {chunk['parent_section_hierarchy']}")
    output.append(f"Level in the hierarchy: {chunk['level']}")
    output.append("")
    output.append(f"Text: {chunk['text']}")
    output.append("")
    output.append("-" * 100)
    
    # Join the list with newline characters
    return "\n".join(output)

def SaveCustomChunksToFile(custom_chunks, file_path):
    all_chunks_string = []  # List to collect all customChunkStrings
    
    # Loop through each chunk, generate its string, and append to the list
    for chunk in custom_chunks:
        customChunkString = GetCustomChunkString(chunk)
        print(customChunkString)  # Optionally print each customChunkString
        all_chunks_string.append(customChunkString)  # Append to list
    
    # Join all strings with newlines separating them
    full_output = "\n".join(all_chunks_string)
    
    # Write the full output to a text file
    with open(file_path, 'w', encoding='utf-8') as file:
        file.write(full_output)

# chunk = final_chunks[10]
# print_custom_chunk(chunk)
