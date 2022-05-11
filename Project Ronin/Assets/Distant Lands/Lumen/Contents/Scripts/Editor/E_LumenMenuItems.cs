using DistantLands.Lumen;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class E_LumenMenuItems : MonoBehaviour
{



    [MenuItem("Distant Lands/Lumen/Create Static Ray", false, 50)]
    static void CreateStaticRay()
    {

        Camera view = SceneView.lastActiveSceneView.camera;

        GameObject go = new GameObject();
        go.name = "Static Lumen Ray";
        go.transform.position = (view.transform.forward * 5) + view.transform.position;
        Selection.objects = new Object[1] { go };
        go.AddComponent<LumenLightRay>();


    }

    [MenuItem("Distant Lands/Lumen/Create Dynamic Ray", false, 50)]
    static void CreateDynamicRay()
    {

        Camera view = SceneView.lastActiveSceneView.camera;

        GameObject go = new GameObject();
        go.name = "Dynamic Lumen Ray";
        go.transform.position = (view.transform.forward * 5) + view.transform.position;
        Selection.objects = new Object[1] { go };
        go.AddComponent<LumenDynamicLightRay>();

    }

    [MenuItem("Distant Lands/Lumen/Clear Light FX Cache", false, 200)]
    static void Clear()
    {

        List<GameObject> gos = new List<GameObject>();

        if (!EditorUtility.DisplayDialog("Are you sure you want to clear the Lumen cache?",
            "This will delete any game object with hide flags enabled that appears to be missing a Lumen connection. " +
            "This may impact other objects in your scene with hide flags!", "Clear", "Cancel"))
            return;


        foreach(GameObject i in Resources.FindObjectsOfTypeAll<GameObject>())
        {

            if (i.hideFlags == HideFlags.HideAndDontSave && i.name == "Lumen Layer")
                gos.Add(i);

        }

        foreach (GameObject i in gos)
            DestroyImmediate(i);

        foreach (LumenEffect flare in FindObjectsOfType<LumenEffect>())
            flare.RedoEffect();

    }


    [MenuItem("Distant Lands/Lumen/Create Light Flare", false, 50)]
    static void CreateLightFlare()
    {

        Camera view = SceneView.lastActiveSceneView.camera;

        GameObject go = new GameObject();
        go.name = "Lumen Light Flare";
        go.transform.position = (view.transform.forward * 5) + view.transform.position;
        Selection.objects = new Object[1] { go };
        go.AddComponent<LumenFlare>();


    }
}
