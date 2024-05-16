using System.Collections;
using System.Net.Http;
using UnityEngine;
using HtmlAgilityPack;
using System.Text;

public class WebScraperSIMATIC : MonoBehaviour
{
    private static readonly HttpClient client = new HttpClient();

    //void Start()
    //{
    //    // You can start the scraping process with a default URL or set it using the method
    //    //string url = "https://mall.industry.siemens.com/mall/en/ww/Catalog/Product/6ES7155-6AR00-0AN0";
    //    string url = "https://mall.industry.siemens.com/mall/en/ww/Catalog/Product/6AG1212-1AE31-2XB0";
    //    StartCoroutine(ScrapeData(url));
    //}

    // Method to start the scraping process with a given article number
    public void StartScraping(string articleNumber)
    {
        string url = $"https://mall.industry.siemens.com/mall/en/ww/Catalog/Product/{articleNumber}";

        StartCoroutine(ScrapeData(url));
    }


    private IEnumerator ScrapeData(string url)
    {
        // Asynchronously fetch the web page content
        var response = client.GetAsync(url);
        while (!response.IsCompleted) yield return null;

        if (response.Result.IsSuccessStatusCode)
        {
            string pageContent = response.Result.Content.ReadAsStringAsync().Result;
            ParseHtml(pageContent);
        }
        else
        {
            Debug.LogError("Failed to retrieve the webpage.");
            DebugUI.AddLog("Failed to retrieve the webpage.");
        }
    }

    private void ParseHtml(string html)
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        // Select the start and end nodes for the "Product" section
        var startNode = htmlDoc.DocumentNode.SelectSingleNode("//tr[td[text()='Product']]");
        var endNode = htmlDoc.DocumentNode.SelectSingleNode("//tr[td[text()='Price data']]");

        if (startNode != null && endNode != null)
        {
            StringBuilder productData = new StringBuilder();

            for (var node = startNode.NextSibling; node != null && node != endNode; node = node.NextSibling)
            {
                if (node.NodeType == HtmlNodeType.Element)
                {
                    // Extract text content from each cell in the row
                    var cells = node.SelectNodes("td");
                    if (cells != null && cells.Count == 2)
                    {
                        string key = cells[0].InnerText.Trim();
                        string value = cells[1].InnerText.Trim();
                        productData.AppendLine($"{key}: {value}");
                    }
                }
            }

            Debug.Log(productData.ToString());
            DebugUI.AddLog(productData.ToString());
        }
        else
        {
            Debug.LogError("Failed to find the Product or Price data section.");
            DebugUI.AddLog("Failed to find the Product or Price data section.");
        }
    }
}
