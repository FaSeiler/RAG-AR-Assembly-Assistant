using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ModelDatabase : Singleton<ModelDatabase>
{
    public List<Model> models = new List<Model>();

    [System.Serializable]
    public struct Model
    {
        public string articleNumber;
        public GameObject prefab;
    }

    private void Start()
    {
        StringBuilder stringBuilder = new StringBuilder();

        foreach (Model model in models)
        {
            stringBuilder.Append(model.articleNumber + "\n");
        }

        Debug.Log(stringBuilder.ToString());
    }

    public GameObject GetModel(string articleNumber)
    {
        foreach (Model model in models)
        {
            if (model.articleNumber == articleNumber)
            {
                return model.prefab;
            }
        }

        return null;
    }
}
