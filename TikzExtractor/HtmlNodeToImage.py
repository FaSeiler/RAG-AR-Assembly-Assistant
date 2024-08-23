import asyncio
from playwright.async_api import async_playwright
from PIL import Image
import os

async def capture_screenshot(input_file, element_selector):
    async with async_playwright() as p:
        browser = await p.chromium.launch(headless=True)
        page = await browser.new_page()

        # Navigate to the local HTML file
        absolute_path = os.path.abspath(input_file)
        await page.goto(f"file:///{absolute_path}")

        # Wait for the page to fully load
        await page.wait_for_load_state("load")

        # Ensure the element is visible
        await page.wait_for_selector(element_selector)

        # Get the bounding box of the element
        element = page.locator(element_selector)
        bounding_box = await element.bounding_box()

        # Zoom in on the page
        zoom_level = 1.3
        await page.evaluate(f"document.body.style.zoom = '{zoom_level}'")

        # Calculate the dimensions considering the zoom level
        scaled_width = bounding_box['width'] * zoom_level
        scaled_height = bounding_box['height'] * zoom_level

        # Set the viewport size to be large enough to capture the entire zoomed element
        viewport_width = int(scaled_width)
        viewport_height = int(scaled_height)
        await page.set_viewport_size({"width": viewport_width, "height": viewport_height})

        # Ensure the element is scrolled into view
        await element.scroll_into_view_if_needed()

        # Capture the screenshot of the entire page (to avoid cutting off the element)
        screenshot_path = 'Screenshot.png'
        await page.screenshot(path=screenshot_path)

        # Open the screenshot using Pillow
        image = Image.open(screenshot_path)

        # Crop the image to the bounding box of the element
        left = bounding_box['x'] * zoom_level
        top = bounding_box['y'] * zoom_level
        right = left + scaled_width
        bottom = top + scaled_height

        # Crop the image to match the zoomed element's dimensions
        # cropped_image = image.crop((left, top, right, bottom))

        # # Save the cropped image
        # cropped_image.save('sample5_html_tikz_extracted_PNG.png')
        image.show()
        await browser.close()

# Run the function
# asyncio.run(capture_screenshot(r"C:\Users\fabia\Desktop\TikzExtractor\Output\sample5_html_tikz_extracted.html", "#capture"))
