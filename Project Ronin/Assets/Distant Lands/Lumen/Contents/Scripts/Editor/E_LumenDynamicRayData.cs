
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using DistantLands.Lumen.Data;

[CustomEditor(typeof(DynamicRayData))]
[CanEditMultipleObjects]
public class E_LumenDynamicRayData : Editor
{

    SerializedProperty rayMaterial;
    SerializedProperty globalScale;
    SerializedProperty globalBrightness;
    SerializedProperty globalRotation;
    DynamicRayData lumenRay;



    void OnEnable()
    {

        lumenRay = (DynamicRayData)target;
        globalScale = serializedObject.FindProperty("globalScale");
        rayMaterial = serializedObject.FindProperty("rayMaterial");
        globalBrightness = serializedObject.FindProperty("globalBrightness");
        globalRotation = serializedObject.FindProperty("globalRotation");



    }

    public override void OnInspectorGUI()
    {

        GUIStyle foldoutStyle = new GUIStyle(GUI.skin.GetStyle("toolbarPopup"));
        foldoutStyle.fontStyle = FontStyle.Bold;
        foldoutStyle.margin = new RectOffset(30, 10, 5, 5);

        GUIStyle title = new GUIStyle(GUI.skin.GetStyle("boldLabel"));

        serializedObject.Update();
        Undo.RecordObject(lumenRay, "Lumen Dynamic Ray Changes");


        EditorPrefs.SetBool("lumen_ShowLayerSettings", EditorGUILayout.BeginFoldoutHeaderGroup(EditorPrefs.GetBool("lumen_ShowLayerSettings"), "   Flare Layers", foldoutStyle));
        EditorGUI.indentLevel++;

        if (EditorPrefs.GetBool("lumen_ShowLayerSettings"))
        {

            if (lumenRay.rayLayers.Count != 0)
                for (int i = 0; i < lumenRay.rayLayers.Count; i++)
                {
                    if (lumenRay.rayLayers[i] == null)
                        break;

                    DynamicRayData.RayLayer j = lumenRay.rayLayers[i];
                    SerializedObject so = new SerializedObject(target);
                    SerializedProperty flareL = serializedObject.FindProperty("rayLayers").GetArrayElementAtIndex(i);

                    j.open = EditorGUILayout.Foldout(j.open, j.rayShape.ToString(), true);

                    if (j.open)
                    {
                        EditorGUI.indentLevel++;

                        EditorGUILayout.LabelField("Main Settings", title);


                        SerializedProperty flareShape = flareL.FindPropertyRelative("rayShape");
                        flareShape.enumValueIndex = (int)(DynamicRayData.RayLayer.RayShapes)EditorGUILayout.EnumPopup("Ray Shape", j.rayShape);



                        EditorGUILayout.PropertyField(flareL.FindPropertyRelative("brightness"));
                        EditorGUILayout.PropertyField(flareL.FindPropertyRelative("raylength"));
                        EditorGUILayout.PropertyField(flareL.FindPropertyRelative("position"));
                        EditorGUILayout.PropertyField(flareL.FindPropertyRelative("scale"));

                        if (!lumenRay.useLightColor)
                            EditorGUILayout.PropertyField(flareL.FindPropertyRelative("colorMultiplier"));

                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Button("Duplicate"))
                        {
                            DynamicRayData.RayLayer k = new DynamicRayData.RayLayer();
                            k.position = j.position;
                            k.brightness = j.brightness;
                            k.raylength = j.raylength;
                            k.rayMesh = j.rayMesh;
                            k.rayShape = j.rayShape;
                            k.scale = j.scale;

                            lumenRay.rayLayers.Insert(i + 1, k);
                        }
                        if (GUILayout.Button("Remove"))
                        {
                            lumenRay.rayLayers.RemoveAt(i);
                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.Space();
                        EditorGUI.indentLevel--;
                    }

                    lumenRay.needsToBeUpdated = true;

                }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add New"))
            {
                DynamicRayData.RayLayer k = new DynamicRayData.RayLayer();
                k.rayShape = 0;
                k.brightness = 1;
                k.colorMultiplier = Color.white;
                k.scale = Vector3.one;
                k.raylength = 10;

                lumenRay.rayLayers.Add(k);
            }
            if (GUILayout.Button("Clear All"))
            {
                if (EditorUtility.DisplayDialog("Clear All", "Are you sure you want to clear all layers from this ray?", "Yes", "Cancel"))
                    lumenRay.rayLayers.Clear();
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUI.indentLevel--;

        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorPrefs.SetBool("lumen_ShowFadingSettings", EditorGUILayout.BeginFoldoutHeaderGroup(EditorPrefs.GetBool("lumen_ShowFadingSettings"), "   Fading Settings", foldoutStyle));
        EditorGUI.indentLevel++;

        if (EditorPrefs.GetBool("lumen_ShowFadingSettings"))
        {

            lumenRay.needsToBeUpdated = true;
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

            EditorGUILayout.PropertyField(serializedObject.FindProperty("useAngleFade"));
            if (serializedObject.FindProperty("useAngleFade").boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("angleFadeStart"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("angleFadeEnd"));
            }
            EditorGUILayout.Space();

        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorPrefs.SetBool("lumen_ShowVariationSettings", EditorGUILayout.BeginFoldoutHeaderGroup(EditorPrefs.GetBool("lumen_ShowVariationSettings"), "   Variation Settings", foldoutStyle));
        EditorGUI.indentLevel++;

        if (EditorPrefs.GetBool("lumen_ShowVariationSettings"))
        {

            lumenRay.needsToBeUpdated = true;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("useVariation"));
            if (serializedObject.FindProperty("useVariation").boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("variationScale"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("variationSpeed"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("variationColor"));
            }
            EditorGUILayout.Space();
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorPrefs.SetBool("lumen_ShowDynamicSettings", EditorGUILayout.BeginFoldoutHeaderGroup(EditorPrefs.GetBool("lumen_ShowDynamicSettings"), "   Dynamic Settings", foldoutStyle));
        EditorGUILayout.EndFoldoutHeaderGroup();
        if (EditorPrefs.GetBool("lumen_ShowDynamicSettings"))
        {

            lumenRay.needsToBeUpdated = true;


            EditorGUILayout.PropertyField(serializedObject.FindProperty("bidirectional"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("angleOpacityEffect"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("angleRaylengthEffect"));

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("autoAssignSun"));
            if (!serializedObject.FindProperty("autoAssignSun").boolValue)
                EditorGUILayout.PropertyField(serializedObject.FindProperty("sunDirection"));
            else
                EditorGUILayout.PropertyField(serializedObject.FindProperty("useLumenSun"));

            EditorGUILayout.Space();

        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorPrefs.SetBool("lumen_ShowGlobalSettings", EditorGUILayout.BeginFoldoutHeaderGroup(EditorPrefs.GetBool("lumen_ShowGlobalSettings"), "   Global Settings", foldoutStyle));
        EditorGUILayout.EndFoldoutHeaderGroup();
        if (EditorPrefs.GetBool("lumen_ShowGlobalSettings"))
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("useLightColor"));
            EditorGUILayout.PropertyField(globalScale);
            EditorGUILayout.PropertyField(globalBrightness);
            EditorGUILayout.PropertyField(rayMaterial);
            EditorGUI.indentLevel--;
        }


        serializedObject.ApplyModifiedProperties();

    }
}
