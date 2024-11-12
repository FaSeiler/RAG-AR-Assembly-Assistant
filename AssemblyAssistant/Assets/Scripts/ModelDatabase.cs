using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// This database returns the prefab of a model based on the article number.
/// In the future there should be a Siemens database where the models are stored.
/// </summary>
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
        //AddArticleNumbersFromComponentDatabase();
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

    // This is just for testing purposes
    private void AddArticleNumbersFromComponentDatabase()
    {
        foreach (var tmp in ComponentDatabase.instance.articleNumbersToLoad)
        {
            models.Add(new Model { articleNumber = tmp, prefab = null });
        }
    }
}
