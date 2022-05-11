using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DistantLands.Lumen;
using DistantLands.Lumen.Data;

[CustomEditor(typeof(LumenLightRay))]
[CanEditMultipleObjects]
public class E_LumenRay : Editor
{
    SerializedProperty flareData;
    SerializedProperty updateFrequency;
    SerializedProperty localColor;
    LumenLightRay lumenRay;
    bool settings;


    void OnEnable()
    {

        lumenRay = (LumenLightRay)target;
        flareData = serializedObject.FindProperty("rayData");
        updateFrequency = serializedObject.FindProperty("updateFrequency");
        localColor = serializedObject.FindProperty("localColor");



    }

    public override void OnInspectorGUI()
    {

        serializedObject.Update();
        EditorGUILayout.PropertyField(flareData);
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Create New Data"))
        {
            string path = EditorUtility.SaveFilePanel("Save Location", "Asset/", "New Ray", "asset");

            if (path.Length == 0)
                return;

            path = "Assets" + path.Substring(Application.dataPath.Length);

            RayData i = CreateInstance(typeof(RayData)) as RayData;

            AssetDatabase.CreateAsset(i, path);
            Debug.Log("Saved asset to " + path + "!");

            flareData.objectReferenceValue = LumenProjectSetup.GetAssets<RayData>(new string[1] { path.Substring(0, path.Length - (i.name.Length + 6)) }, i.name)[0];

        }

        EditorGUILayout.EndHorizontal();
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.Space();

        if (flareData.objectReferenceValue)
        {
            SerializedObject so = new SerializedObject(lumenRay.rayData);
            CreateEditor(lumenRay.rayData).OnInspectorGUI();

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
                EditorGUI.indentLevel--;
            }

            if (serializedObject.hasModifiedProperties || so.hasModifiedProperties || lumenRay.rayData.needsToBeUpdated)
                lumenRay.RedoEffect();

                                                                                                        
            serializedObject.ApplyModifiedProperties();
            so.ApplyModifiedProperties();
        }
        else
            EditorGUILayout.HelpBox("Set your Lumen Ray data here!", MessageType.Info, true);
    }
}