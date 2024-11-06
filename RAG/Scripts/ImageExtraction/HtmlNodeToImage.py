import asyncio
from playwright.async_api import async_playwright
from PIL import Image
import os

data_dir = "../data/"

async def CaptureScreenshot(input_file, remove_original_after=True):
    async with async_playwright() as p:
        browser = await p.chromium.launch(headless=True)
        page = await browser.new_page()

        # Navigate to the local HTML file
        absolute_path = os.path.abspath(input_file)
        await page.goto(f"file:///{absolute_path}")
        # print(f"Opened file: {absolute_path}")

        # Wait for the page to fully load
        await page.wait_for_load_state("load")

        element_selector = "#capture"

        try:
            # Wait for the element to be visible with a specified timeout
            await page.wait_for_selector(element_selector, timeout=3000)  # Adjust timeout as needed
        # Proceed with capturing the screenshot if the element is visible
        # Your screenshot logic here
        except Exception as e:
            print(f"Error: {e}")
            
            await browser.close()
            
            if remove_original_after:
                os.remove(input_file)
            
            return "", "", ""

        # Get the bounding box of the element
        element = page.locator(element_selector)
        bounding_box = await element.bounding_box()

        # Zoom in on the page
        zoom_level = 4
        await page.evaluate(f"document.body.style.zoom = '{zoom_level}'")

        # Calculate the dimensions considering the zoom level
        scaled_width = bounding_box["width"] * zoom_level
        scaled_height = bounding_box["height"] * zoom_level

        # Set the viewport size to be large enough to capture the entire zoomed element
        viewport_width = int(scaled_width)
        viewport_height = int(scaled_height)
        await page.set_viewport_size(
            {"width": viewport_width, "height": viewport_height}
        )

        # Ensure the element is scrolled into view
        await element.scroll_into_view_if_needed()

        # Capture the screenshot of the entire page (to avoid cutting off the element)
        screenshot_path = "Screenshot.png"
        await page.screenshot(path=screenshot_path)

        # Open the screenshot using Pillow
        image = Image.open(screenshot_path)

        # Open the image
        # image.show()
        file_name = os.path.splitext(os.path.basename(input_file))[0]

        # File name e.g. "myPdf_page_8_3" -> <pdfName>_page_<pageNumber>_<imageIndex>
        # Split by underscores
        parts = file_name.split("_")

        # Extract the page_number and image_index from the end
        page_number = int(parts[-2])
        image_index = int(parts[-1])

        # Join the remaining parts to form the pdf_name
        pdf_name = "_".join(parts[:-3])

        # Save the image
        # Create a new directory if it does not yet exist
        thisImageDir = (
            f"{data_dir}/Images/{pdf_name}/"  # Example: "../data/Images/myPdf/"
        )
        if not os.path.exists(thisImageDir):
            os.makedirs(thisImageDir)

        image_path = (
            thisImageDir + file_name + ".png"
        )  # Example: "../data/Images/myPdf/myPdf_page_8_3.png"
        image.save(image_path)

        os.remove("Screenshot.png")
        if remove_original_after:
            os.remove(input_file)

        await browser.close()

        return pdf_name, page_number, image_path
