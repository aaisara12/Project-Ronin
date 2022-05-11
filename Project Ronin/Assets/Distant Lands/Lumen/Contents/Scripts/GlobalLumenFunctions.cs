using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DistantLands.Lumen.Data;

public class GlobalLumenFunctions
{

    public static List<Mesh> flareShapes = new List<Mesh>();
    public static List<Mesh> staticRayShapes = new List<Mesh>();
    public static List<Mesh> dynamicRayShapes = new List<Mesh>();



    public static void DoubleCheckShapeLists()
    {


        if (flareShapes.Count == 0 || staticRayShapes.Count == 0 || dynamicRayShapes.Count == 0)
            LumenProjectSetup();

    }


    public static Mesh GetFlareShape(FlareData.FlareLayer.FlareShapes flareShape)
    {
        Mesh i = null;


        if (flareShapes.Count == 0)
        {
            GetFlareShapeMeshes();
            Debug.Log("Flare Shapes Count: " + flareShapes.Count);
        }

        i = flareShapes[(int)flareShape];

        return i;
    }

    public static Mesh GetStaticRayShape(RayData.RayLayer.RayShapes rayShape)
    {
        Mesh i = null;

        if (staticRayShapes.Count == 0)
        {
            GetStaticRayShapeMeshes();
            Debug.Log("Ray Shapes Count: " + staticRayShapes.Count);
        }

        i = staticRayShapes[(int)rayShape];

        return i;
    }

    public static Mesh GetDynamicRayShape(DynamicRayData.RayLayer.RayShapes rayShape)
    {
        Mesh i = null;

        if (dynamicRayShapes == null)
            Debug.LogAssertion("UHHHHHHHHH");

        if (dynamicRayShapes.Count == 0)
        {
            GetDynamicRayShapeMeshes();
            Debug.Log("Ray Shapes Count: " + dynamicRayShapes.Count);
        }

        i = dynamicRayShapes[(int)rayShape];

        return i;
    }

    public static void GetFlareShapeMeshes()
    {

        Mesh[] meshes = Resources.LoadAll<Mesh>("");

        List<Mesh> flareMeshes = new List<Mesh>();

        for (int i = 0; i < meshes.Length; i++)
        {
            if (meshes[i].name.Contains("_LUMENFLARE"))
            {
                flareMeshes.Add(meshes[i]);

            }
        }

        flareShapes = flareMeshes;


    }

    public static void GetStaticRayShapeMeshes()
    {

        Mesh[] meshes = Resources.LoadAll<Mesh>("");

        List<Mesh> rayMeshes = new List<Mesh>();

        for (int i = 0; i < meshes.Length; i++)
        {
            if (meshes[i].name.Contains("_LUMENRAY"))
            {
                rayMeshes.Add(meshes[i]);

            }
        }

        staticRayShapes = rayMeshes;
    }
    public static void GetDynamicRayShapeMeshes()
    {

        Mesh[] meshes = Resources.LoadAll<Mesh>("");

        List<Mesh> rayMeshes = new List<Mesh>();             

        for (int i = 0; i < meshes.Length; i++)
        {
            if (meshes[i].name.Contains("_LUMENDYNRAY"))
            {
                rayMeshes.Add(meshes[i]);

            }
        }

        dynamicRayShapes = rayMeshes;
    }

    [RuntimeInitializeOnLoadMethod]
    static void LumenProjectSetup()
    {

        GetFlareShapeMeshes();
        GetStaticRayShapeMeshes();
        GetDynamicRayShapeMeshes();


    }

}