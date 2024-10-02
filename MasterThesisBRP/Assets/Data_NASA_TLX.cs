using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace NASA_TLX
{
    public class Data_NASA_TLX : MonoBehaviour
    {
        [Header("Scenario")]
        public Szenario_Evaluation scenario;

        [Header("Subject")]
        public string subjectID;

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
        public int pair_Effort_Frustration;       // 0: Effort, 1: Frustration

        [Header("Weighted Scores")]
        // Weighted scores for each dimension
        public float weighted_MentalDemand;
        public float weighted_PhysicalDemand;
        public float weighted_TemporalDemand;
        public float weighted_Performance;
        public float weighted_Effort;
        public float weighted_Frustration;

        [Header("Number of selected")]
        // Number of selected for each dimension
        int nr_Mental_Selected;
        int nr_Physical_Selected;
        int nr_Temporal_Selected;
        int nr_Performance_Selected;
        int nr_Effort_Selected;
        int nr_Frustration_Selected;

        [Header("Total Scores")]
        // Total scores
        float total_un_weighted_score;
        float sum_of_weighted_scores;
        float total_weighted_score;

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
        public WeightPair_NASA_TLX weightPair_Effort_Frustration;

        [Header("Finish Button")]
        public Button finishButton;

        public void Start()
        {
            // Subject
            subjectID = Settings_Evaluation.instance.subjectID;

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
            pair_Effort_Frustration = -1;

            sliderMentalDemand.onValueChanged.AddListener(delegate { value_MentalDemand = TransformFromUnityToNASAScale((int)sliderMentalDemand.value); CheckIfAllFilledOut(); });
            sliderPhysicalDemand.onValueChanged.AddListener(delegate { value_PhysicalDemand = TransformFromUnityToNASAScale((int)sliderPhysicalDemand.value); CheckIfAllFilledOut(); });
            sliderTemporalDemand.onValueChanged.AddListener(delegate { value_TemporalDemand = TransformFromUnityToNASAScale((int)sliderTemporalDemand.value); CheckIfAllFilledOut(); });
            sliderPerformance.onValueChanged.AddListener(delegate { value_Performance = TransformFromUnityToNASAScale((int)sliderPerformance.value); CheckIfAllFilledOut(); });
            sliderEffort.onValueChanged.AddListener(delegate { value_Effort = TransformFromUnityToNASAScale((int)sliderEffort.value); CheckIfAllFilledOut(); });
            sliderFrustration.onValueChanged.AddListener(delegate { value_Frustration = TransformFromUnityToNASAScale((int)sliderFrustration.value); CheckIfAllFilledOut(); });

            weightPair_Mental_Physical.OnSelectionChanged.AddListener((int pairValue) => { pair_Mental_Physical = pairValue; CheckIfAllFilledOut(); });
            weightPair_Mental_Temporal.OnSelectionChanged.AddListener((int pairValue) => { pair_Mental_Temporal = pairValue; CheckIfAllFilledOut(); });
            weightPair_Mental_Performance.OnSelectionChanged.AddListener((int pairValue) => { pair_Mental_Performance = pairValue; CheckIfAllFilledOut(); });
            weightPair_Mental_Effort.OnSelectionChanged.AddListener((int pairValue) => { pair_Mental_Effort = pairValue; CheckIfAllFilledOut(); });
            weightPair_Mental_Frustration.OnSelectionChanged.AddListener((int pairValue) => { pair_Mental_Frustration = pairValue; CheckIfAllFilledOut(); });
            weightPair_Physical_Temporal.OnSelectionChanged.AddListener((int pairValue) => { pair_Physical_Temporal = pairValue; CheckIfAllFilledOut(); });
            weightPair_Physical_Performance.OnSelectionChanged.AddListener((int pairValue) => { pair_Physical_Performance = pairValue; CheckIfAllFilledOut(); });
            weightPair_Physical_Effort.OnSelectionChanged.AddListener((int pairValue) => { pair_Physical_Effort = pairValue; CheckIfAllFilledOut(); });
            weightPair_Physical_Frustration.OnSelectionChanged.AddListener((int pairValue) => { pair_Physical_Frustration = pairValue; CheckIfAllFilledOut(); });
            weightPair_Temporal_Performance.OnSelectionChanged.AddListener((int pairValue) => { pair_Temporal_Performance = pairValue; CheckIfAllFilledOut(); });
            weightPair_Temporal_Frustration.OnSelectionChanged.AddListener((int pairValue) => { pair_Temporal_Frustration = pairValue; CheckIfAllFilledOut(); });
            weightPair_Temporal_Effort.OnSelectionChanged.AddListener((int pairValue) => { pair_Temporal_Effort = pairValue; CheckIfAllFilledOut(); });
            weightPair_Performance_Frustration.OnSelectionChanged.AddListener((int pairValue) => { pair_Performance_Frustration = pairValue; CheckIfAllFilledOut(); });
            weightPair_Performance_Effort.OnSelectionChanged.AddListener((int pairValue) => { pair_Performance_Effort = pairValue; CheckIfAllFilledOut(); });
            weightPair_Effort_Frustration.OnSelectionChanged.AddListener((int pairValue) => { pair_Effort_Frustration = pairValue; CheckIfAllFilledOut(); });

            CheckIfAllFilledOut();
        }

        private void CheckIfAllFilledOut()
        {
            if (subjectID != "-1" &&
                value_MentalDemand != -1 &&
                value_PhysicalDemand != -1 &&
                value_TemporalDemand != -1 &&
                value_Performance != -1 &&
                value_Effort != -1 &&
                value_Frustration != -1 &&
                pair_Mental_Physical != -1 &&
                pair_Mental_Temporal != -1 &&
                pair_Mental_Performance != -1 &&
                pair_Mental_Effort != -1 &&
                pair_Mental_Frustration != -1 &&
                pair_Physical_Temporal != -1 &&
                pair_Physical_Performance != -1 &&
                pair_Physical_Effort != -1 &&
                pair_Physical_Frustration != -1 &&
                pair_Temporal_Performance != -1 &&
                pair_Temporal_Frustration != -1 &&
                pair_Temporal_Effort != -1 &&
                pair_Performance_Frustration != -1 &&
                pair_Performance_Effort != -1 &&
                pair_Effort_Frustration != -1)
            {
                finishButton.interactable = true;
            }
            else
            {
                finishButton.interactable = false;
            }
        }

        public void ExportDataJSON()
        {
            CalculateTLXScores();

            Exporter_NASA_TLX exportData = new Exporter_NASA_TLX
            {
                subjectID = subjectID,
                szenario = scenario.szenario,

                total_un_weighted_score = total_un_weighted_score,
                sum_of_weighted_scores = sum_of_weighted_scores,
                total_weighted_score = total_weighted_score,

                value_MentalDemand = value_MentalDemand,
                value_PhysicalDemand = value_PhysicalDemand,
                value_TemporalDemand = value_TemporalDemand,
                value_Performance = value_Performance,
                value_Effort = value_Effort,
                value_Frustration = value_Frustration,

                weighted_MentalDemand = weighted_MentalDemand,
                weighted_PhysicalDemand = weighted_PhysicalDemand,
                weighted_TemporalDemand = weighted_TemporalDemand,
                weighted_Performance = weighted_Performance,
                weighted_Effort = weighted_Effort,
                weighted_Frustration = weighted_Frustration,

                nr_Mental_Selected = nr_Mental_Selected,
                nr_Physical_Selected = nr_Physical_Selected,
                nr_Temporal_Selected = nr_Temporal_Selected,
                nr_Performance_Selected = nr_Performance_Selected,
                nr_Effort_Selected = nr_Effort_Selected,
                nr_Frustration_Selected = nr_Frustration_Selected,

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
                pair_Effort_Frustration = pair_Effort_Frustration
            };

            // Serialize the data to JSON
            string jsonData = JsonUtility.ToJson(exportData, true);
            string timeStamp = System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");

            string directoryPath_Results = Path.Combine(Application.persistentDataPath, "Results");
            string directoryPath_SubjectID = Path.Combine(directoryPath_Results, subjectID.ToString());
            string directoryPath_Scenario = Path.Combine(directoryPath_SubjectID, scenario.szenario);

            // Check if the directory exists
            if (!Directory.Exists(directoryPath_Results))
            {
                // Create the directory if it doesn't exist
                Directory.CreateDirectory(directoryPath_Results);
            }
            if (!Directory.Exists(directoryPath_SubjectID))
            {
                // Create the directory if it doesn't exist
                Directory.CreateDirectory(directoryPath_SubjectID);
            }
            if (!Directory.Exists(directoryPath_Scenario))
            {
                // Create the directory if it doesn't exist
                Directory.CreateDirectory(directoryPath_Scenario);
            }

            string fileName = subjectID + "_" + scenario.szenario + "_NASA-TLX_" + timeStamp;

            string path = Path.Combine(directoryPath_Scenario, fileName + ".json");

            File.WriteAllText(path, jsonData);
            Debug.Log("Data exported to: " + path);
        }

        public void ExportDataCSV()
        {
            CalculateTLXScores();

            // Create a StringBuilder to construct the CSV data
            StringBuilder csvData = new StringBuilder();

            // Add the header line (column names)
            csvData.AppendLine("SubjectID,Scenario,Total_Un_Weighted_Score,Sum_Of_Weighted_Scores,Total_Weighted_Score," +
                                "Mental_Demand,Physical_Demand,Temporal_Demand,Performance,Effort,Frustration," +
                                "Weighted_Mental_Demand,Weighted_Physical_Demand,Weighted_Temporal_Demand," +
                                "Weighted_Performance,Weighted_Effort,Weighted_Frustration," +
                                "Nr_Mental_Selected,Nr_Physical_Selected,Nr_Temporal_Selected,Nr_Performance_Selected," +
                                "Nr_Effort_Selected,Nr_Frustration_Selected," +
                                "Pair_Mental_Physical,Pair_Mental_Temporal,Pair_Mental_Performance," +
                                "Pair_Mental_Effort,Pair_Mental_Frustration,Pair_Physical_Temporal," +
                                "Pair_Physical_Performance,Pair_Physical_Effort,Pair_Physical_Frustration," +
                                "Pair_Temporal_Performance,Pair_Temporal_Frustration,Pair_Temporal_Effort," +
                                "Pair_Performance_Frustration,Pair_Performance_Effort,Pair_Effort_Frustration");

            // Add the actual data
            csvData.AppendLine($"{subjectID},{scenario.szenario},{total_un_weighted_score},{sum_of_weighted_scores}," +
                               $"{total_weighted_score},{value_MentalDemand},{value_PhysicalDemand}," +
                               $"{value_TemporalDemand},{value_Performance},{value_Effort},{value_Frustration}," +
                               $"{weighted_MentalDemand},{weighted_PhysicalDemand},{weighted_TemporalDemand}," +
                               $"{weighted_Performance},{weighted_Effort},{weighted_Frustration}," +
                               $"{nr_Mental_Selected},{nr_Physical_Selected},{nr_Temporal_Selected}," +
                               $"{nr_Performance_Selected},{nr_Effort_Selected},{nr_Frustration_Selected}," +
                               $"{pair_Mental_Physical},{pair_Mental_Temporal},{pair_Mental_Performance}," +
                               $"{pair_Mental_Effort},{pair_Mental_Frustration},{pair_Physical_Temporal}," +
                               $"{pair_Physical_Performance},{pair_Physical_Effort},{pair_Physical_Frustration}," +
                               $"{pair_Temporal_Performance},{pair_Temporal_Frustration},{pair_Temporal_Effort}," +
                               $"{pair_Performance_Frustration},{pair_Performance_Effort},{pair_Effort_Frustration}");

            string timeStamp = System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");

            string directoryPath_Results = Path.Combine(Application.persistentDataPath, "Results");
            string directoryPath_SubjectID = Path.Combine(directoryPath_Results, subjectID.ToString());
            string directoryPath_Scenario = Path.Combine(directoryPath_SubjectID, scenario.szenario);

            // Check if the directory exists and create it if necessary
            if (!Directory.Exists(directoryPath_Results))
            {
                Directory.CreateDirectory(directoryPath_Results);
            }
            if (!Directory.Exists(directoryPath_SubjectID))
            {
                Directory.CreateDirectory(directoryPath_SubjectID);
            }
            if (!Directory.Exists(directoryPath_Scenario))
            {
                Directory.CreateDirectory(directoryPath_Scenario);
            }

            string fileName = $"{subjectID}_{scenario.szenario}_NASA-TLX_{timeStamp}";

            string path = Path.Combine(directoryPath_Scenario, fileName + ".csv");

            // Write the CSV data to the file
            File.WriteAllText(path, csvData.ToString());
            Debug.Log("Data exported to: " + path);
        }

        private int TransformFromUnityToNASAScale(int valueBefore)
        {
            int valueAfter = ((valueBefore - 1) * 100) / 20;
            return valueAfter;
        }

        public void CalculateTLXScores()
        {
            nr_Mental_Selected = 0;
            nr_Physical_Selected = 0;
            nr_Temporal_Selected = 0;
            nr_Performance_Selected = 0;
            nr_Effort_Selected = 0;
            nr_Frustration_Selected = 0;

            #region Calculate_Weighted_Scores
            if (pair_Mental_Physical == 0)
            {
                nr_Mental_Selected++;
            }
            else
            {
                nr_Physical_Selected++;
            }

            if (pair_Mental_Temporal == 0)
            {
                nr_Mental_Selected++;
            }
            else
            {
                nr_Temporal_Selected++;
            }

            if (pair_Mental_Performance == 0)
            {
                nr_Mental_Selected++;
            }
            else
            {
                nr_Performance_Selected++;
            }

            if (pair_Mental_Effort == 0)
            {
                nr_Mental_Selected++;
            }
            else
            {
                nr_Effort_Selected++;
            }

            if (pair_Mental_Frustration == 0)
            {
                nr_Mental_Selected++;
            }
            else
            {
                nr_Frustration_Selected++;
            }

            if (pair_Physical_Temporal == 0)
            {
                nr_Physical_Selected++;
            }
            else
            {
                nr_Temporal_Selected++;
            }

            if (pair_Physical_Performance == 0)
            {
                nr_Physical_Selected++;
            }
            else
            {
                nr_Performance_Selected++;
            }

            if (pair_Physical_Effort == 0)
            {
                nr_Physical_Selected++;
            }
            else
            {
                nr_Effort_Selected++;
            }

            if (pair_Physical_Frustration == 0)
            {
                nr_Physical_Selected++;
            }
            else
            {
                nr_Frustration_Selected++;
            }

            if (pair_Temporal_Performance == 0)
            {
                nr_Temporal_Selected++;
            }
            else
            {
                nr_Performance_Selected++;
            }

            if (pair_Temporal_Frustration == 0)
            {
                nr_Temporal_Selected++;
            }
            else
            {
                nr_Frustration_Selected++;
            }

            if (pair_Temporal_Effort == 0)
            {
                nr_Temporal_Selected++;
            }
            else
            {
                nr_Effort_Selected++;
            }

            if (pair_Performance_Frustration == 0)
            {
                nr_Performance_Selected++;
            }
            else
            {
                nr_Frustration_Selected++;
            }

            if (pair_Performance_Effort == 0)
            {
                nr_Performance_Selected++;
            }
            else
            {
                nr_Effort_Selected++;
            }

            if (pair_Effort_Frustration == 0)
            {
                nr_Effort_Selected++;
            }
            else
            {
                nr_Frustration_Selected++;
            }
            #endregion

            weighted_MentalDemand = value_MentalDemand * nr_Mental_Selected;
            weighted_PhysicalDemand = value_PhysicalDemand * nr_Physical_Selected;
            weighted_TemporalDemand = value_TemporalDemand * nr_Temporal_Selected;
            weighted_Performance = value_Performance * nr_Performance_Selected;
            weighted_Effort = value_Effort * nr_Effort_Selected;
            weighted_Frustration = value_Frustration * nr_Frustration_Selected;

            total_un_weighted_score = (value_MentalDemand + value_PhysicalDemand + value_TemporalDemand + value_Performance + value_Effort + value_Frustration) / 6;
            sum_of_weighted_scores = weighted_MentalDemand + weighted_PhysicalDemand + weighted_TemporalDemand + weighted_Performance + weighted_Effort + weighted_Frustration;
            total_weighted_score = sum_of_weighted_scores / 15;
        }
    }
}
