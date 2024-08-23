import asyncio
from playwright.async_api import async_playwright
from PIL import Image
import os

async def capture_screenshot(input_file, remove_original_after=True):
    async with async_playwright() as p:
        browser = await p.chromium.launch(headless=True)
        page = await browser.new_page()

        # Navigate to the local HTML file
        absolute_path = os.path.abspath(input_file)
        await page.goto(f"file:///{absolute_path}")
        print(f"Opened file: {absolute_path}")

        # Wait for the page to fully load
        await page.wait_for_load_state("load")

        element_selector = "#capture"

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

        # Open the image
        # image.show()
        file_name = os.path.splitext(os.path.basename(input_file))[0]
        image_path = f"C:/Users/fabia/Desktop/MasterThesisRepo/TikzExtractor/Output/{file_name}.png"
        image.save(image_path)
        
        os.remove("Screenshot.png")
        if remove_original_after:
            os.remove(input_file)

        await browser.close()
