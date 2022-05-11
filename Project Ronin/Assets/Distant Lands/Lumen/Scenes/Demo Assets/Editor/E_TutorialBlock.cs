using DistantLands.Lumen.Demo;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TutorialBlock))]
public class E_TutorialBlock : Editor
{

    TutorialBlock obj;


    void OnEnable()
    {

        obj = (TutorialBlock)target;

    }


    public override void OnInspectorGUI()
    {


        SerializedProperty prop = serializedObject.FindProperty("helpNote");
        prop.stringValue = GUILayout.TextArea(obj.helpNote);

        serializedObject.ApplyModifiedProperties();


    }
}
