using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using DistantLands.Lumen.Data;

[InitializeOnLoad]
public class LumenProjectSetup
{


    static LumenProjectSetup()
    {


        GlobalLumenFunctions.GetFlareShapeMeshes();
        GlobalLumenFunctions.GetStaticRayShapeMeshes();
        GlobalLumenFunctions.GetDynamicRayShapeMeshes();


    }


    public static List<T> GetAssets<T>(string[] _foldersToSearch, string _filter) where T : UnityEngine.Object
    {
        string[] guids = AssetDatabase.FindAssets(_filter, _foldersToSearch);
        List<T> a = new List<T>();
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            a.Add(AssetDatabase.LoadAssetAtPath<T>(path));
        }
        return a;
    }

}