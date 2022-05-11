using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DistantLands.Lumen.Data;

[CustomEditor(typeof(FlareData))]
[CanEditMultipleObjects]
public class E_LumenFlareData : Editor
{

    SerializedProperty flareMaterial;
    SerializedProperty globalScale;
    SerializedProperty globalBrightness;
    FlareData lumenFlare;
                                    


    void OnEnable()
    {

        lumenFlare = (FlareData)target;
        globalScale = serializedObject.FindProperty("globalScale");
        flareMaterial = serializedObject.FindProperty("flareMaterial");
        globalBrightness = serializedObject.FindProperty("globalBrightness");



    }

    public override void OnInspectorGUI()
    {

        GUIStyle foldoutStyle = new GUIStyle(GUI.skin.GetStyle("toolbarPopup"));
        foldoutStyle.fontStyle = FontStyle.Bold;
        foldoutStyle.margin = new RectOffset(30, 10, 5, 5);

        GUIStyle title = new GUIStyle(GUI.skin.GetStyle("boldLabel"));

        serializedObject.Update();
        Undo.RecordObject(lumenFlare, "Lumen Flare Changes");


        EditorPrefs.SetBool("lumen_ShowLayerSettings", EditorGUILayout.BeginFoldoutHeaderGroup(EditorPrefs.GetBool("lumen_ShowLayerSettings"), "   Flare Layers", foldoutStyle));
        EditorGUI.indentLevel++;

        if (EditorPrefs.GetBool("lumen_ShowLayerSettings"))
        {

            if (lumenFlare.flare.Count != 0)
                for (int i = 0; i < lumenFlare.flare.Count; i++)
                {
                    if (lumenFlare.flare[i] == null)
                        break;

                    FlareData.FlareLayer j = lumenFlare.flare[i];

                    SerializedProperty flareL = serializedObject.FindProperty("flare").GetArrayElementAtIndex(i);
                    SerializedObject so = new SerializedObject(target);

                    j.open = EditorGUILayout.Foldout(j.open, j.flareShape.ToString(), true);

                    if (j.open)
                    {
                        EditorGUI.indentLevel++;

                        EditorGUILayout.LabelField("Main Settings", title);

                        SerializedProperty flareShape = flareL.FindPropertyRelative("flareShape");
                        flareShape.enumValueIndex = (int)(FlareData.FlareLayer.FlareShapes)EditorGUILayout.EnumPopup("Flare Shape", j.flareShape);

                        EditorGUILayout.PropertyField(flareL.FindPropertyRelative("brightness"));
                        EditorGUILayout.PropertyField(flareL.FindPropertyRelative("scale"));
                        EditorGUILayout.PropertyField(flareL.FindPropertyRelative("colorMultiplier"));
                        EditorGUILayout.Space();



                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Button("Duplicate"))
                        {
                            FlareData.FlareLayer k = new FlareData.FlareLayer();
                            k.flareShape = j.flareShape;
                            k.brightness = j.brightness;
                            k.colorMultiplier = j.colorMultiplier;
                            k.scale = j.scale;

                            lumenFlare.flare.Insert(i + 1, k);
                            break;
                        }
                        if (GUILayout.Button("Remove"))
                        {
                            lumenFlare.flare.RemoveAt(i);
                            break;
                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.Space();
                        EditorGUI.indentLevel--;
                    }
                }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add New"))
            {
                FlareData.FlareLayer k = new FlareData.FlareLayer();
                k.flareShape = 0;
                k.brightness = 1;
                k.colorMultiplier = Color.white;
                k.scale = 1;


                lumenFlare.flare.Add(k);
            }
            if (GUILayout.Button("Clear All"))
            {
                if (EditorUtility.DisplayDialog("Clear All", "Are you sure you want to clear all layers from this flare?", "Yes", "Cancel"))
                    lumenFlare.flare.Clear();
            }
            EditorGUILayout.EndHorizontal();
        }     

        EditorGUI.indentLevel--;
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorPrefs.SetBool("lumen_ShowFadingSettings", EditorGUILayout.BeginFoldoutHeaderGroup(EditorPrefs.GetBool("lumen_ShowFadingSettings"), "   Fading Settings", foldoutStyle));
        EditorGUI.indentLevel++;

        if (EditorPrefs.GetBool("lumen_ShowFadingSettings"))
        {

            lumenFlare.needsToBeUpdated = true;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("useCameraDepthFade"));
            if (serializedObject.FindProperty("useCameraDepthFade").boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cameraDepthFadeStart"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cameraDepthFadeEnd"));
            }
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("useCameraDistanceFade"));
            if (serializedObject.FindProperty("useCameraDistanceFade").boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cameraDistanceFadeStart"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cameraDistanceFadeEnd"));
            }
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("useSceneDepthFade"));
            if (serializedObject.FindProperty("useSceneDepthFade").boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("sceneDepthFadeStart"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("sceneDepthFadeEnd"));
            }
            EditorGUILayout.Space();

        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorPrefs.SetBool("lumen_ShowVariationSettings", EditorGUILayout.BeginFoldoutHeaderGroup(EditorPrefs.GetBool("lumen_ShowVariationSettings"), "   Variation Settings", foldoutStyle));
        EditorGUI.indentLevel++;

        if (EditorPrefs.GetBool("lumen_ShowVariationSettings"))
        {

            lumenFlare.needsToBeUpdated = true;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("useVariation"));
            if (serializedObject.FindProperty("useVariation").boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("variationScale"));
                SerializedProperty speed = serializedObject.FindProperty("variationSpeed");
                speed.floatValue = EditorGUILayout.Slider("Variation Speed", lumenFlare.variationSpeed, 0, 10);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("variationColor"));
            }

        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorPrefs.SetBool("lumen_ShowGlobalSettings", EditorGUILayout.BeginFoldoutHeaderGroup(EditorPrefs.GetBool("lumen_ShowGlobalSettings"), "   Global Settings", foldoutStyle));
        EditorGUILayout.EndFoldoutHeaderGroup();
        if (EditorPrefs.GetBool("lumen_ShowGlobalSettings"))
        {
            lumenFlare.needsToBeUpdated = true;
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(globalScale);
            EditorGUILayout.PropertyField(globalBrightness);
            EditorGUILayout.PropertyField(flareMaterial);

            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();

#if UNITY_EDITOR

        EditorApplication.update += lumenFlare.ResetFlare;
#endif

    }
}

[CustomPropertyDrawer(typeof(FlareData.FlareLayer))]
public class E_LumenFlareLayer : PropertyDrawer
{



}
