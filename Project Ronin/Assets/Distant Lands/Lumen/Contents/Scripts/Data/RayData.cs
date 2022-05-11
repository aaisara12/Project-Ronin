using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


namespace DistantLands.Lumen.Data
{
    [CreateAssetMenu(menuName = "Distant Lands/Lumen/New Ray Data", order = 391)]
    public class RayData : ScriptableObject
    {
        
        [System.Serializable]
        public class RayLayer
        {

            public enum RayShapes
            {
                FlatLaserRay1,
                FlatLaserRay2,
                FlatLaserRay3,
                FlatLaserRay4,
                FlatRay1,
                FlatRay2,
                FlatRay3,
                FlatRay4,
                FlatRay5,
                FlatRay6,
                FlatSpotRay1,
                FlatSpotRay2,
                FlatSpotRay3,
                FlatSpotRay4,
                CylindricalScatter1,
                CylindricalScatter2,
                CylindricalScatter3,
                CylindricalScatter4,
                CylindricalScatter5,
                CylindricalScatter6,
                CylindricalScatter7,
                CylindricalScatter8,
                PrismScatter1,
                PrismScatter2,
                PrismScatter3,
                PrismScatter4,
                PrismScatter5,
                PrismScatter6,
                PrismScatter7,
                PrismScatter8,
                RayleighScatter1,
                RayleighScatter2,
                RayleighScatter3,
                RayleighScatter4,
                RayleighScatter5,
                RayleighScatter6,
                RayleighScatter7,
                SpotlightRay1,
                SpotlightRay2,
                SpotlightRay3,
                SpotlightRay4,
                SpotlightRay5,
                SpotlightScatter1,
                SpotlightScatter2,
                SpotlightScatter3,
                SpotlightScatter4,
                SpotlightScatter5,
                SpotlightScatter6,
                SpotlightScatter7,
                SpotlightScatter8,
                SolidScatter1,
            }
            public RayShapes rayShape;
            [HideInInspector]
            public Mesh rayMesh;
            [Range(0,1)]
            public float brightness = 1;
            [ColorUsage(false, true)]
            public Color colorMultiplier = Color.white;
            public Vector3 position;
            public Vector3 rotation;
            public Vector3 scale = Vector3.one;


            public delegate void SetupRay();
            public static SetupRay setupRay;


            [HideInInspector]
            public bool open;

        }

        public List<RayLayer> rayLayers;           
        public float globalScale = 1;
        public Material rayMaterial;
        public Vector3 globalRotation;
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
        public bool needsToBeUpdated;

        public bool useCameraDepthFade = true;
        public float cameraDepthFadeStart;
        public float cameraDepthFadeEnd;

        public bool useCameraDistanceFade = true;
        public float cameraDistanceFadeStart;
        public float cameraDistanceFadeEnd;

        public bool useSceneDepthFade = false;
        public float sceneDepthFadeStart;
        public float sceneDepthFadeEnd;

        public bool useAngleFade = false;
        public float angleFadeStart;
        public float angleFadeEnd;

        public bool useVariation;
        public bool useLightColor;


        public delegate void SetupRay();
        public static SetupRay setupRay;

        public void OnValidate()
        {

#if UNITY_EDITOR
            EditorApplication.update += ResetRay;
#endif

        }

        public void ResetRay()
        {

            setupRay?.Invoke();

#if UNITY_EDITOR
            EditorApplication.update -= ResetRay;
#endif
        } 
    }
}