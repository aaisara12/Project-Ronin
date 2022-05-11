using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;



namespace DistantLands.Lumen.Data
{
    [CreateAssetMenu(menuName = "Distant Lands/Lumen/New Flare Data", order = 391)]
    public class FlareData : ScriptableObject
    {
        
        [System.Serializable]
        public class FlareLayer
        {

            public enum FlareShapes
            {
                HaloFlare1,
                LightBand1,
                LightBand2,
                LightBand3,
                LightBand4,
                LightHalo1,
                LightHalo2,
                LightHalo3,
                LightHalo4,
                LightHalo5,
                LightHalo6,
                LightHalo7,
                LightRing1,
                LightRing2,
                LightRing3,
                LightRing4,
                LightRing5,
                LightRing6,
                OblongFlare1,
                OblongFlare2,
                OblongFlare3,
                OblongFlare4,
                OblongFlare5,
                StylizedFlare1,
                StylizedFlare2,
                StylizedFlare3,
                StylizedFlare4,
                StylizedFlare5,
                StylizedFlare6,
                StylizedFlare7,
            }
            public FlareShapes flareShape;
            [HideInInspector]
            public Mesh flareMesh;
            public float scale = 1;
            [Range(0,1)]
            public float brightness = 1;
            [ColorUsage(false, true)]
            public Color colorMultiplier = Color.white;



            [HideInInspector]
            public bool open;


        }

        [SerializeField]
        public List<FlareLayer> flare = new List<FlareLayer>(1) { new FlareLayer() };
        public Material flareMaterial;
        public float globalScale = 1;
        [Range(0,1)]
        public float globalBrightness = 1;
        public float variationSpeed = 1;
        public float variationScale = 10;
        [ColorUsage(true, true)]
        public Color variationColor = Color.white;

        [HideInInspector]
        public bool showLayerSettings;
        [HideInInspector]
        public bool showMainSettings;
        [HideInInspector]
        public bool showVariationSettings;
        [HideInInspector]
        public bool showFadingSettings;



        public float cameraDepthFadeStart;
        public float cameraDepthFadeEnd;
        public float cameraDistanceFadeStart;
        public float cameraDistanceFadeEnd;
        public float sceneDepthFadeStart;
        public float sceneDepthFadeEnd;
        public bool useCameraDepthFade = true;
        public bool useCameraDistanceFade = true;
        public bool useSceneDepthFade = false;
        public bool useVariation;


        public delegate void SetupFlare();
        public static SetupFlare setupFlare;
        public bool needsToBeUpdated;

        public void OnValidate()
        {

#if UNITY_EDITOR
            EditorApplication.update += ResetFlare;
#endif

        }

        public void ResetFlare()
        {

            setupFlare?.Invoke();

#if UNITY_EDITOR
            EditorApplication.update -= ResetFlare;
#endif
        }

    }
}