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
        print(f"Opened file: {absolute_path}")

        # Wait for the page to fully load and stabilize
        await page.wait_for_load_state('networkidle')

        # Construct the attribute selector
        # int(page_div['data-page-no'], 16)
        attribute_selector = f'[capture_figure="{element_selector}"]'
        # print(f"Using attribute selector: {attribute_selector}")

        # Ensure the element is visible
        try:
            await page.wait_for_selector(attribute_selector, timeout=10000)  # 10 seconds timeout
            # print(f"Element with selector '{attribute_selector}' is visible.")
        except Exception as e:
            # print(f"Error: {e}")
            await browser.close()
            return

        # Get the bounding box of the element
        element = page.locator(attribute_selector)
        visible = await element.is_visible()
        bounding_box = await element.bounding_box()
        # print(f"Element visible: {visible}")
        # print(f"Bounding box: {bounding_box}")

        if bounding_box is None:
            print(f"Element with selector '{attribute_selector}' not found.")
            await browser.close()
            return

        # Zoom in on the page
        zoom_level = 2  # Adjust zoom level as necessary
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

        # Capture the screenshot of the entire page
        screenshot_path = input_file + '.png'
        await page.screenshot(path=screenshot_path)
        # print(f"Screenshot saved to: {screenshot_path}")

        # Open the screenshot using Pillow
        image = Image.open(screenshot_path)

        # Define the output image path
        file_name = os.path.splitext(os.path.basename(input_file))[0]
        image_path = f"C:/Users/fabia/Desktop/MasterThesisRepo/TikzExtractor/Output/{file_name}_{element_selector}.png"
        print(f"Image saved to: {image_path}")
        image.save(image_path)
        
        # os.remove(screenshot_path)


        await browser.close()

# Example usage
# asyncio.run(capture_screenshot("./Input/sample16.html", "capture_2_1", False))
