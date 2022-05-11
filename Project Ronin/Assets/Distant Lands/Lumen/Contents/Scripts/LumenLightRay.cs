using DistantLands.Lumen.Data;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace DistantLands.Lumen
{
    public class LumenLightRay : LumenEffect
    {


        public RayData rayData;



        private void OnEnable()
        {
            RayData.setupRay += RedoEffectFromDelegate;
        }

        private void OnDisable()
        {
            RayData.setupRay -= RedoEffectFromDelegate;
        }

        public override void RedoEffect()
        {

            if (!rayData) { Debug.LogWarning("Make sure to set your ray data on " + name + "!"); return; }


            Transform[] children = GetComponentsInChildren<Transform>();

            for (int i = 1; i < children.Length; i++)
                DestroyImmediate(children[i].gameObject);

            int k = 0;

#if UNITY_EDITOR
            if (PrefabUtility.IsPartOfAnyPrefab(gameObject))
                if (gameObject.scene.name == null || gameObject.scene.name == gameObject.name)
                    return;
#endif

            foreach (RayData.RayLayer i in rayData.rayLayers)
            {
                k++;


                GameObject j = new GameObject("Lumen Layer")
                {
                    hideFlags = HideFlags.HideAndDontSave,

                };

                j.transform.parent = transform;
                j.transform.localEulerAngles = i.rotation + rayData.globalRotation;
                j.transform.localPosition = i.position.x * rayData.globalScale * Vector3.right +
                    i.position.y * rayData.globalScale * Vector3.up +
                    i.position.z * rayData.globalScale * Vector3.forward;
                j.transform.localScale = new Vector3(i.scale.x * rayData.globalScale, i.scale.y * rayData.globalScale, i.scale.z * rayData.globalScale);

                MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();
                j.AddComponent<MeshFilter>().mesh = GlobalLumenFunctions.GetStaticRayShape(i.rayShape);
                MeshRenderer meshRenderer = j.AddComponent<MeshRenderer>();
                meshRenderer.sharedMaterial = rayData.rayMaterial;
                meshRenderer.GetPropertyBlock(_propBlock);
                _propBlock.SetColor("_MainColor", i.colorMultiplier * localColor);
                _propBlock.SetFloat("_Intensity", i.brightness * rayData.globalBrightness);
                _propBlock.SetFloat("_RenderOffset", k - 1);
                _propBlock.SetFloat("_UseCameraDepthFade", rayData.useCameraDepthFade ? 1 : 0);
                _propBlock.SetFloat("_CameraDepthFadeStart", rayData.cameraDepthFadeStart);
                _propBlock.SetFloat("_CameraDepthFadeEnd", rayData.cameraDepthFadeEnd);

                _propBlock.SetFloat("_UseCameraDistanceFade", rayData.useCameraDistanceFade ? 1 : 0);
                _propBlock.SetFloat("_CameraDistanceFadeStart", rayData.cameraDistanceFadeStart);
                _propBlock.SetFloat("_CameraDistanceFadeEnd", rayData.cameraDistanceFadeEnd);

                _propBlock.SetFloat("_UseSceneDepthFade", rayData.useSceneDepthFade ? 1 : 0);
                _propBlock.SetFloat("_DepthFadeStartDistance", rayData.sceneDepthFadeStart);
                _propBlock.SetFloat("_DepthFadeEndDistance", rayData.sceneDepthFadeEnd);

                _propBlock.SetFloat("_UseAngleBasedFade", rayData.useAngleFade ? 1 : 0);
                _propBlock.SetFloat("_AngleFadeStart", rayData.angleFadeStart);
                _propBlock.SetFloat("_AngleFadeEnd", rayData.angleFadeEnd);

                _propBlock.SetFloat("_UseVariation", rayData.useVariation ? 1 : 0);
                _propBlock.SetColor("_VariationColor", rayData.variationColor);
                _propBlock.SetFloat("_VariationScale", rayData.variationScale);
                _propBlock.SetFloat("_VariationSpeed", rayData.variationSpeed);

                _propBlock.SetFloat("_UseLightColor", rayData.useLightColor ? 1 : 0);
                meshRenderer.SetPropertyBlock(_propBlock);


            }

            rayData.needsToBeUpdated = false;

        }



    }
}