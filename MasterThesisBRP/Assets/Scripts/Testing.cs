using System.Collections;
using System.Net.Http;
using UnityEngine;
using HtmlAgilityPack;

public class Testing : MonoBehaviour
{
    private string url = "https://mall.industry.siemens.com/mall/en/ww/Catalog/Product/6ES7155-6AR00-0AN0";

    void Start()
    {
        // Start the scraping process
        StartCoroutine(ScrapeData());
    }

    private IEnumerator ScrapeData()
    {
        using (HttpClient client = new HttpClient())
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
            }
        }
    }

    private void ParseHtml(string html)
    {
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(html);

        // Example of parsing specific data, adjust the XPath queries as needed
        var productNameNode = doc.DocumentNode.SelectSingleNode("//h1[@class='product-name']");
        var productDescriptionNode = doc.DocumentNode.SelectSingleNode("//div[@class='product-description']");
        var productDetailsNode = doc.DocumentNode.SelectSingleNode("//div[@class='product-details']");

        if (productNameNode != null)
        {
            string productName = productNameNode.InnerText.Trim();
            Debug.Log("Product Name: " + productName);
        }

        if (productDescriptionNode != null)
        {
            string productDescription = productDescriptionNode.InnerText.Trim();
            Debug.Log("Product Description: " + productDescription);
        }

        if (productDetailsNode != null)
        {
            string productDetails = productDetailsNode.InnerHtml.Trim();
            Debug.Log("Product Details: " + productDetails);
        }
    }
}
