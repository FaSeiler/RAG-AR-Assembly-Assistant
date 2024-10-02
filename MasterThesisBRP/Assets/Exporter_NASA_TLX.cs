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

        // Total scores
        [HideInInspector] public float total_un_weighted_score;
        [HideInInspector] public float sum_of_weighted_scores;
        [HideInInspector] public float total_weighted_score;

        // Raw scores for each dimension
        [HideInInspector] public int value_MentalDemand;
        [HideInInspector] public int value_PhysicalDemand;
        [HideInInspector] public int value_TemporalDemand;
        [HideInInspector] public int value_Performance;
        [HideInInspector] public int value_Effort;
        [HideInInspector] public int value_Frustration;

        // Weighted scores for each dimension
        [HideInInspector] public float weighted_MentalDemand;
        [HideInInspector] public float weighted_PhysicalDemand;
        [HideInInspector] public float weighted_TemporalDemand;
        [HideInInspector] public float weighted_Performance;
        [HideInInspector] public float weighted_Effort;
        [HideInInspector] public float weighted_Frustration;

        // Number of selected per dimension
        [HideInInspector] public int nr_Mental_Selected;
        [HideInInspector] public int nr_Physical_Selected;
        [HideInInspector] public int nr_Temporal_Selected;
        [HideInInspector] public int nr_Performance_Selected;
        [HideInInspector] public int nr_Effort_Selected;
        [HideInInspector] public int nr_Frustration_Selected;

        // Pairwise comparisons
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
        [HideInInspector] public int pair_Effort_Frustration;
    }
}
