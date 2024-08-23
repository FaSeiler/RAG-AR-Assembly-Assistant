const puppeteer = require('puppeteer');
const path = require('path');
const fs = require('fs');

(async () => {
    // Launch a headless browser
    const browser = await puppeteer.launch();
    const page = await browser.newPage();
    
    // Define the path to your HTML file
    const htmlFilePath = path.join(__dirname, 'ExtractedTikzSample5.html');

    // Load the HTML file
    await page.goto(`file://${htmlFilePath}`, { waitUntil: 'networkidle0' });

    // Capture the screenshot
    const element = await page.$('#mySvg'); // Adjust the selector if needed
    if (element) {
        await element.screenshot({ path: 'screenshot.png' });
        console.log('Screenshot saved as screenshot.png');
    } else {
        console.error('Element not found');
    }

    // Close the browser
    await browser.close();
})();
