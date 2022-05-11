using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DistantLands.Lumen;
using DistantLands.Lumen.Data;

[CustomEditor(typeof(LumenFlare))]
[CanEditMultipleObjects]
public class E_LumenFlare : Editor
{
    SerializedProperty flareData;
    SerializedProperty updateFrequency;
    SerializedProperty localColor;
    SerializedProperty localScale;
    LumenFlare lumenFlare;

    bool settings;


    void OnEnable()
    {

        lumenFlare = (LumenFlare)target;
        flareData = serializedObject.FindProperty("flareData");
        updateFrequency = serializedObject.FindProperty("updateFrequency");
        localColor = serializedObject.FindProperty("localColor");
        localScale = serializedObject.FindProperty("localScale");



    }

    public override void OnInspectorGUI()
    {


        serializedObject.Update();
        EditorGUILayout.PropertyField(flareData);

        EditorGUILayout.BeginHorizontal();
       
        if (GUILayout.Button("Create New Data"))
        {
            string path = EditorUtility.SaveFilePanel("Save Location", "Asset/", "New Flare", "asset");

            if (path.Length == 0)
                return;

            path = "Assets" + path.Substring(Application.dataPath.Length);

            FlareData i = CreateInstance(typeof(FlareData)) as FlareData;

            AssetDatabase.CreateAsset(i, path);
            Debug.Log("Saved asset to " + path + "!");

            flareData.objectReferenceValue = LumenProjectSetup.GetAssets<FlareData>(new string[1] { path.Substring(0, path.Length - (i.name.Length + 6)) }, i.name)[0];

        }


        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.Space();

        if (flareData.objectReferenceValue)
        {
            SerializedObject so = new SerializedObject(lumenFlare.flareData);
            CreateEditor(lumenFlare.flareData).OnInspectorGUI();

            GUIStyle foldoutStyle = new GUIStyle(GUI.skin.GetStyle("toolbarPopup"));
            foldoutStyle.fontStyle = FontStyle.Bold;
            foldoutStyle.margin = new RectOffset(30, 10, 5, 5);

            settings = EditorGUILayout.BeginFoldoutHeaderGroup(settings, "   Local Settings", foldoutStyle);
            EditorGUILayout.EndFoldoutHeaderGroup();

            if (settings)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(updateFrequency);
                EditorGUILayout.PropertyField(localColor);
                EditorGUILayout.PropertyField(localScale);
                EditorGUI.indentLevel--;
            }

            if (serializedObject.hasModifiedProperties || so.hasModifiedProperties || lumenFlare.flareData.needsToBeUpdated)
                lumenFlare.RedoEffect();

            serializedObject.ApplyModifiedProperties();
            so.ApplyModifiedProperties();
        }
        else
            EditorGUILayout.HelpBox("Set your Lumen Flare data here!", MessageType.Info, true);

    }
}
