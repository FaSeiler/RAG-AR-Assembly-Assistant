using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace NASA_TLX
{
    public class Data_NASA_TLX : MonoBehaviour
    {
        [Header("Subject")]
        public int subjectID;

        [Header("Values")]
        public int value_MentalDemand;
        public int value_PhysicalDemand;
        public int value_TemporalDemand;
        public int value_Performance;
        public int value_Effort;
        public int value_Frustration;

        [Header("Value Pairs")]
        public int pair_Mental_Physical;          // 0: Mental, 1: Physical
        public int pair_Mental_Temporal;          // 0: Mental, 1: Temporal
        public int pair_Mental_Performance;       // 0: Mental, 1: Performance
        public int pair_Mental_Effort;            // 0: Mental, 1: Effort
        public int pair_Mental_Frustration;       // 0: Mental, 1: Frustration
        public int pair_Physical_Temporal;        // 0: Physical, 1: Temporal
        public int pair_Physical_Performance;     // 0: Physical, 1: Performance
        public int pair_Physical_Effort;          // 0: Physical, 1: Effort
        public int pair_Physical_Frustration;     // 0: Physical, 1: Frustration
        public int pair_Temporal_Performance;     // 0: Temporal, 1: Performance
        public int pair_Temporal_Frustration;     // 0: Temporal, 1: Frustration
        public int pair_Temporal_Effort;          // 0: Temporal, 1: Effort
        public int pair_Performance_Frustration;  // 0: Performance, 1: Frustration
        public int pair_Performance_Effort;       // 0: Performance, 1: Effort
        public int pair_Frustration_Effort;       // 0: Frustration, 1: Effort

        [Header("Value Sliders")]
        public Slider sliderMentalDemand;
        public Slider sliderPhysicalDemand;
        public Slider sliderTemporalDemand;
        public Slider sliderPerformance;
        public Slider sliderEffort;
        public Slider sliderFrustration;

        [Header("Weight Pairs")]
        public WeightPair_NASA_TLX weightPair_Mental_Physical;
        public WeightPair_NASA_TLX weightPair_Mental_Temporal;
        public WeightPair_NASA_TLX weightPair_Mental_Performance;
        public WeightPair_NASA_TLX weightPair_Mental_Effort;
        public WeightPair_NASA_TLX weightPair_Mental_Frustration;
        public WeightPair_NASA_TLX weightPair_Physical_Temporal;
        public WeightPair_NASA_TLX weightPair_Physical_Performance;
        public WeightPair_NASA_TLX weightPair_Physical_Effort;
        public WeightPair_NASA_TLX weightPair_Physical_Frustration;
        public WeightPair_NASA_TLX weightPair_Temporal_Performance;
        public WeightPair_NASA_TLX weightPair_Temporal_Frustration;
        public WeightPair_NASA_TLX weightPair_Temporal_Effort;
        public WeightPair_NASA_TLX weightPair_Performance_Frustration;
        public WeightPair_NASA_TLX weightPair_Performance_Effort;
        public WeightPair_NASA_TLX weightPair_Frustration_Effort;

        public void Start()
        {
            // Subject
            subjectID = -1;

            // Values
            value_MentalDemand = -1;
            value_PhysicalDemand = -1;
            value_TemporalDemand = -1;
            value_Performance = -1;
            value_Effort = -1;
            value_Frustration = -1;

            // Value Pairs
            pair_Mental_Physical = -1;
            pair_Mental_Temporal = -1;
            pair_Mental_Performance = -1;
            pair_Mental_Effort = -1;
            pair_Mental_Frustration = -1;
            pair_Physical_Temporal = -1;
            pair_Physical_Performance = -1;
            pair_Physical_Effort = -1;
            pair_Physical_Frustration = -1;
            pair_Temporal_Performance = -1;
            pair_Temporal_Frustration = -1;
            pair_Temporal_Effort = -1;
            pair_Performance_Frustration = -1;
            pair_Performance_Effort = -1;
            pair_Frustration_Effort = -1;

            sliderMentalDemand.onValueChanged.AddListener(delegate { value_MentalDemand = (int)sliderMentalDemand.value; });
            sliderPhysicalDemand.onValueChanged.AddListener(delegate { value_PhysicalDemand = (int)sliderPhysicalDemand.value; });
            sliderTemporalDemand.onValueChanged.AddListener(delegate { value_TemporalDemand = (int)sliderTemporalDemand.value; });
            sliderPerformance.onValueChanged.AddListener(delegate { value_Performance = (int)sliderPerformance.value; });
            sliderEffort.onValueChanged.AddListener(delegate { value_Effort = (int)sliderEffort.value; });
            sliderFrustration.onValueChanged.AddListener(delegate { value_Frustration = (int)sliderFrustration.value; });

            weightPair_Mental_Physical.OnSelectionChanged.AddListener((int pairValue) => { pair_Mental_Physical = pairValue; });
            weightPair_Mental_Temporal.OnSelectionChanged.AddListener((int pairValue) => { pair_Mental_Temporal = pairValue; });
            weightPair_Mental_Performance.OnSelectionChanged.AddListener((int pairValue) => { pair_Mental_Performance = pairValue; });
            weightPair_Mental_Effort.OnSelectionChanged.AddListener((int pairValue) => { pair_Mental_Effort = pairValue; });
            weightPair_Mental_Frustration.OnSelectionChanged.AddListener((int pairValue) => { pair_Mental_Frustration = pairValue; });
            weightPair_Physical_Temporal.OnSelectionChanged.AddListener((int pairValue) => { pair_Physical_Temporal = pairValue; });
            weightPair_Physical_Performance.OnSelectionChanged.AddListener((int pairValue) => { pair_Physical_Performance = pairValue; });
            weightPair_Physical_Effort.OnSelectionChanged.AddListener((int pairValue) => { pair_Physical_Effort = pairValue; });
            weightPair_Physical_Frustration.OnSelectionChanged.AddListener((int pairValue) => { pair_Physical_Frustration = pairValue; });
            weightPair_Temporal_Performance.OnSelectionChanged.AddListener((int pairValue) => { pair_Temporal_Performance = pairValue; });
            weightPair_Temporal_Frustration.OnSelectionChanged.AddListener((int pairValue) => { pair_Temporal_Frustration = pairValue; });
            weightPair_Temporal_Effort.OnSelectionChanged.AddListener((int pairValue) => { pair_Temporal_Effort = pairValue; });
            weightPair_Performance_Frustration.OnSelectionChanged.AddListener((int pairValue) => { pair_Performance_Frustration = pairValue; });
            weightPair_Performance_Effort.OnSelectionChanged.AddListener((int pairValue) => { pair_Performance_Effort = pairValue; });
            weightPair_Frustration_Effort.OnSelectionChanged.AddListener((int pairValue) => { pair_Frustration_Effort = pairValue; });
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                //ExportToJSON("NASA_TLX_Data");
                ExportData();
            }
        }

        public void ExportData()
        {
            Exporter_NASA_TLX exportData = new Exporter_NASA_TLX
            {
                subjectID = subjectID,

                value_MentalDemand = value_MentalDemand,
                value_PhysicalDemand = value_PhysicalDemand,
                value_TemporalDemand = value_TemporalDemand,
                value_Performance = value_Performance,
                value_Effort = value_Effort,
                value_Frustration = value_Frustration,

                pair_Mental_Physical = pair_Mental_Physical,
                pair_Mental_Temporal = pair_Mental_Temporal,
                pair_Mental_Performance = pair_Mental_Performance,
                pair_Mental_Effort = pair_Mental_Effort,
                pair_Mental_Frustration = pair_Mental_Frustration,
                pair_Physical_Temporal = pair_Physical_Temporal,
                pair_Physical_Performance = pair_Physical_Performance,
                pair_Physical_Effort = pair_Physical_Effort,
                pair_Physical_Frustration = pair_Physical_Frustration,
                pair_Temporal_Performance = pair_Temporal_Performance,
                pair_Temporal_Frustration = pair_Temporal_Frustration,
                pair_Temporal_Effort = pair_Temporal_Effort,
                pair_Performance_Frustration = pair_Performance_Frustration,
                pair_Performance_Effort = pair_Performance_Effort,
                pair_Frustration_Effort = pair_Frustration_Effort
            };

            // Serialize the data to JSON
            string jsonData = JsonUtility.ToJson(exportData, true);

            string timeStamp = System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");

            string fileName = subjectID + "_NASA-TLX_" + timeStamp;

            string path = Path.Combine(Application.persistentDataPath, fileName + ".json");

            File.WriteAllText(path, jsonData);
            Debug.Log("Data exported to: " + path);
        }
    }
}
