using DistantLands.Lumen.Data;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;



namespace DistantLands.Lumen
{
    public class LumenFlare : LumenEffect
    {


        Camera m_MainCamera;
        public FlareData flareData;

        public float localScale = 1;


        // Start is called before the first frame update
        void Awake()
        {

            GlobalLumenFunctions.DoubleCheckShapeLists();
            ResetMainCamera();
            RedoEffect();


        }

        private void OnEnable()
        {
            FlareData.setupFlare += RedoEffectFromDelegate;
        }
        private void OnDisable()
        {
            FlareData.setupFlare -= RedoEffectFromDelegate;
        }

        public void ResetMainCamera()
        {

            
            m_MainCamera = Camera.main;

        } 

        // Update is called once per frame
        void LateUpdate()
        {

            if (!m_MainCamera)
                ResetMainCamera();
            else if (!m_MainCamera.isActiveAndEnabled)
                ResetMainCamera();


            if (!m_MainCamera)
            {
                Debug.LogWarning("Make sure that you have a main camera in your scene for Lumen Flares to work properly");
                return;
            }

            transform.forward = m_MainCamera.transform.forward;

        }

        public override void RedoEffect()
        {



            Transform[] children = GetComponentsInChildren<Transform>();

            for (int i = 1; i < children.Length; i++)
                DestroyImmediate(children[i].gameObject);

            int k = 0;

            if (flareData == null) { Debug.LogWarning("Make sure to set your flare data on " + name + "!"); return; }
            if (flareData.flare.Count == 0) { return; }

#if UNITY_EDITOR
            if (PrefabUtility.IsPartOfAnyPrefab(gameObject))
                if (gameObject.scene.name == null || gameObject.scene.name == gameObject.name)
                    return;
#endif

            foreach (FlareData.FlareLayer i in flareData.flare)
            {
                k++;


                GameObject j = new GameObject("Lumen Layer")
                {
                    hideFlags = HideFlags.HideAndDontSave,

                };

                j.transform.parent = transform;
                j.transform.localPosition = Vector3.zero;
                j.transform.localEulerAngles = Vector3.zero;
                j.transform.localScale = new Vector3(i.scale * flareData.globalScale * localScale, i.scale * flareData.globalScale * localScale, 1);

                MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();
                j.AddComponent<MeshFilter>().mesh = GlobalLumenFunctions.GetFlareShape(i.flareShape);
                MeshRenderer meshRenderer = j.AddComponent<MeshRenderer>();
                meshRenderer.sharedMaterial = flareData.flareMaterial;
                meshRenderer.GetPropertyBlock(_propBlock);
                _propBlock.SetColor("_MainColor", i.colorMultiplier * localColor);
                _propBlock.SetFloat("_Intensity", i.brightness * flareData.globalBrightness);
                _propBlock.SetFloat("_RenderOffset", k - 1);
                _propBlock.SetFloat("_UseCameraDepthFade", flareData.useCameraDepthFade ? 1 : 0);
                _propBlock.SetFloat("_CameraDepthFadeStart", flareData.cameraDepthFadeStart);
                _propBlock.SetFloat("_CameraDepthFadeEnd", flareData.cameraDepthFadeEnd);

                _propBlock.SetFloat("_UseCameraDistanceFade", flareData.useCameraDistanceFade ? 1 : 0);
                _propBlock.SetFloat("_CameraDistanceFadeStart", flareData.cameraDistanceFadeStart);
                _propBlock.SetFloat("_CameraDistanceFadeEnd", flareData.cameraDistanceFadeEnd);

                _propBlock.SetFloat("_UseSceneDepthFade", flareData.useSceneDepthFade ? 1 : 0);
                _propBlock.SetFloat("_DepthFadeStartDistance", flareData.sceneDepthFadeStart);
                _propBlock.SetFloat("_DepthFadeEndDistance", flareData.sceneDepthFadeEnd);


                _propBlock.SetFloat("_UseVariation", flareData.useVariation ? 1 : 0);
                _propBlock.SetColor("_VariationColor", flareData.variationColor);
                _propBlock.SetFloat("_VariationScale", flareData.variationScale);     
                _propBlock.SetFloat("_VariationSpeed", flareData.variationSpeed);
                meshRenderer.SetPropertyBlock(_propBlock);


            }

            flareData.needsToBeUpdated = false;


        }


    }
}