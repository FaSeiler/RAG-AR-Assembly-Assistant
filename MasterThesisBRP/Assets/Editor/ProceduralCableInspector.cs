using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ProceduralCable))]
public class ProceduralCableInspector : Editor {

    ProceduralCable proceduralCable;

    public void OnEnable()
    {
        proceduralCable = (ProceduralCable)target;
        Undo.undoRedoPerformed += () => { proceduralCable.UpdateObject(); };
    }

    public override void OnInspectorGUI()
    {
        proceduralCable.drawEditorLines = EditorGUILayout.Toggle("Draw lines ", proceduralCable.drawEditorLines);

        EditorGUI.BeginChangeCheck();
        float newCurvature = EditorGUILayout.FloatField("Curvature", proceduralCable.curvature);

        // a field for the proceduralCable.upVector
        proceduralCable.upVector = EditorGUILayout.Vector3Field("Up Vector", proceduralCable.upVector);

        // a field for the proceduralCable.connectorA and B GameObject
        proceduralCable.connectorA = (GameObject)EditorGUILayout.ObjectField("Connector A", proceduralCable.connectorA, typeof(GameObject), true);
        proceduralCable.connectorB = (GameObject)EditorGUILayout.ObjectField("Connector B", proceduralCable.connectorB, typeof(GameObject), true);

        proceduralCable.aTransform = (Transform)EditorGUILayout.ObjectField("A Transform", proceduralCable.aTransform, typeof(Transform), true);
        if (proceduralCable.aTransform != null)
        {
            proceduralCable.a = proceduralCable.aTransform.position;
        }

        // a field for the b position transform
        proceduralCable.bTransform = (Transform)EditorGUILayout.ObjectField("B Transform", proceduralCable.bTransform, typeof(Transform), true);
        if (proceduralCable.bTransform != null)
        {
            proceduralCable.b = proceduralCable.bTransform.position;
        }

        int newStep = EditorGUILayout.IntField("Step",proceduralCable.step);
        int newRadiusStep = EditorGUILayout.IntField("Radius step", proceduralCable.radiusStep);
        float newRadius = EditorGUILayout.FloatField("Radius", proceduralCable.radius);
        Vector2 newUvMultiply = EditorGUILayout.Vector2Field("UV Multiply", proceduralCable.uvMultiply);

        // a button to update transform positions
        if (GUILayout.Button("Update Transform Positions"))
        {
            if (proceduralCable.aTransform != null)
            {
                proceduralCable.a = proceduralCable.aTransform.position;
            }
            if (proceduralCable.bTransform != null)
            {
                proceduralCable.b = proceduralCable.bTransform.position;
            }
        }

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(proceduralCable, "Change parameter");

            proceduralCable.curvature = newCurvature;

            newStep = newStep < 1 ? 1 : newStep;
            proceduralCable.step = newStep;

            newRadiusStep = newRadiusStep < 3 ? 3 : newRadiusStep;
            proceduralCable.radiusStep = newRadiusStep;

            newRadius = newRadius < 0 ? 0 : newRadius;
            proceduralCable.radius = newRadius;

            proceduralCable.uvMultiply = newUvMultiply;

            proceduralCable.UpdateObject();

            EditorUtility.SetDirty(proceduralCable);
        }
    }

    public void UpdateCablePositions(Vector3 positionA, Vector3 positionB)
    {
        proceduralCable.a = positionA;
        proceduralCable.b = positionB;
        proceduralCable.UpdateObject();
    }

    private void OnSceneGUI()
    {
        int step = proceduralCable.step;

        EditorGUI.BeginChangeCheck();

        Vector3 newAposition = Handles.DoPositionHandle(proceduralCable.a, Quaternion.identity);
        Vector3 newBposition = Handles.DoPositionHandle(proceduralCable.b, Quaternion.identity);

        if (proceduralCable.aTransform != null)
        {
            proceduralCable.aTransform.position = newAposition;
        }
        if (proceduralCable.bTransform != null)
        {
            proceduralCable.bTransform.position = newBposition;
        }


        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(proceduralCable, "Change position of extremity point");
            proceduralCable.a = newAposition;
            proceduralCable.b = newBposition;
            proceduralCable.UpdateObject();
            EditorUtility.SetDirty(proceduralCable);
        }

        if (proceduralCable.drawEditorLines)
        {
            for (int i = 0; i < step; i++)
            {
                Handles.DrawLine(proceduralCable.PointPosition(i), proceduralCable.PointPosition(i + 1));
                DrawVerticesForPoint(i);
            }
            Handles.DrawPolyLine(proceduralCable.VerticesForPoint(step));
            DrawVerticesForPoint(step);
        }



    }

    private void DrawVerticesForPoint(int i)
    {
        Vector3[] verticesForPoint = proceduralCable.VerticesForPoint(i);

        for (int h = 0; h < verticesForPoint.Length-1; h++)
            Handles.DrawLine(verticesForPoint[h], verticesForPoint[h + 1]);

        Handles.DrawLine(verticesForPoint[proceduralCable.radiusStep-1], verticesForPoint[0]);
    }

}
