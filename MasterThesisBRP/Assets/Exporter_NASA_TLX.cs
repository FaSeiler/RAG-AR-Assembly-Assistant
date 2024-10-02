using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NASA_TLX
{
    [System.Serializable]
    public class Exporter_NASA_TLX : MonoBehaviour
    {
        [HideInInspector] public string subjectID;
        [HideInInspector] public string szenario;

        [HideInInspector] public int value_MentalDemand;
        [HideInInspector] public int value_PhysicalDemand;
        [HideInInspector] public int value_TemporalDemand;
        [HideInInspector] public int value_Performance;
        [HideInInspector] public int value_Effort;
        [HideInInspector] public int value_Frustration;

        [HideInInspector] public int pair_Mental_Physical;
        [HideInInspector] public int pair_Mental_Temporal;
        [HideInInspector] public int pair_Mental_Performance;
        [HideInInspector] public int pair_Mental_Effort;
        [HideInInspector] public int pair_Mental_Frustration;
        [HideInInspector] public int pair_Physical_Temporal;
        [HideInInspector] public int pair_Physical_Performance;
        [HideInInspector] public int pair_Physical_Effort;
        [HideInInspector] public int pair_Physical_Frustration;
        [HideInInspector] public int pair_Temporal_Performance;
        [HideInInspector] public int pair_Temporal_Frustration;
        [HideInInspector] public int pair_Temporal_Effort;
        [HideInInspector] public int pair_Performance_Frustration;
        [HideInInspector] public int pair_Performance_Effort;
        [HideInInspector] public int pair_Frustration_Effort;
    }
}
