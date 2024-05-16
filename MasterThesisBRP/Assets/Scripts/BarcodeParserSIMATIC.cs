using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarcodeSIMATIC : MonoBehaviour
{
    public string GetArticleNumberFromBarcode(string barcode)
    {
        // Sample barcode 1: 1P6AG1212-1AE31-2XB0+ -> Article Number 1: 6AG1212-1AE31-2XB0
        // Sample barcode 2: 1P6ES7155-6AR00-0AN0+23S286336D72F96+SC-J8MK1502 -> Article Number 2: 6ES7155-6AR00-0AN0
        // Infer article number from barcode
        string articleNumber = barcode.Substring(2, barcode.IndexOf('+') - 2);
        return articleNumber;
    }
}
