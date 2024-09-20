using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Text;
using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine.EventSystems;
using System.IO;

public class Testing : MonoBehaviour
{
    // Reference to the "reference" object
    public GameObject reference;

    // Reference to the "place" object
    public GameObject place;

    // Offset relative to the "reference" object's local space
    public Vector3 offset;

    //void Update()
    //{
    //    // Invert the X component of the offset to account for the axis flip issue
    //    Vector3 adjustedOffset = new Vector3(-offset.x, offset.y, offset.z);

    //    // Apply the adjusted offset relative to the reference object's rotation and position
    //    Vector3 worldOffset = reference.transform.position + reference.transform.rotation * adjustedOffset;

    //    // Set the position of the "place" object
    //    place.transform.position = worldOffset;

    //    // Set the rotation of the "place" object to match the "reference" object's rotation
    //    place.transform.rotation = reference.transform.rotation;
    //}

    //private void Start()
    //{
    //    //place.transform.position = TrackingManager.instance.referenceTransform.position;
    //    //place.transform.rotation = TrackingManager.instance.referenceTransform.rotation;
    //}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            TrackingManager.instance.UpdateReferenceTransform(reference.transform);
        }
    }
}
