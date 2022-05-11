using DistantLands.Lumen.Data;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace DistantLands.Lumen
{
    public class LumenDynamicLightRay : LumenEffect
    {


        public DynamicRayData rayData;


        private void OnEnable()
        {
            DynamicRayData.setupRay += RedoEffectFromDelegate;
        }
        private void OnDisable()
        {
            DynamicRayData.setupRay -= RedoEffectFromDelegate;
        }

        public override void RedoEffect()
        {


            if (!rayData) { Debug.LogError("Make sure to set your ray data on " + name + "!"); return; }


            Transform[] children = GetComponentsInChildren<Transform>();

            for (int i = 1; i < children.Length; i++)
                DestroyImmediate(children[i].gameObject);

            int k = 0;

#if UNITY_EDITOR
            if (PrefabUtility.IsPartOfAnyPrefab(gameObject))
                if (gameObject.scene.name == null || gameObject.scene.name == gameObject.name)
                    return;
#endif

            foreach (DynamicRayData.RayLayer i in rayData.rayLayers)
            {
                k++;


                GameObject j = new GameObject("Lumen Layer")
                {
                    hideFlags = HideFlags.HideAndDontSave,

                };
                j.transform.parent = transform;
                j.transform.localEulerAngles = Vector3.zero + Vector3.right * 0.001f;
                j.transform.localPosition = i.position.x * rayData.globalScale * Vector3.right +
                    i.position.y * rayData.globalScale * Vector3.up +
                    i.position.z * rayData.globalScale * Vector3.forward;
                j.transform.localScale = new Vector3(i.scale.x * rayData.globalScale, i.scale.y * rayData.globalScale, i.scale.z * rayData.globalScale);

                MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();
                MeshFilter filter = j.AddComponent<MeshFilter>();
                filter.sharedMesh = GlobalLumenFunctions.GetDynamicRayShape(i.rayShape);
                filter.sharedMesh.bounds = new Bounds(Vector3.zero, new Vector3(i.raylength / j.transform.localScale.x, i.raylength / j.transform.localScale.y, i.raylength / j.transform.localScale.z));
                MeshRenderer meshRenderer = j.AddComponent<MeshRenderer>();
                meshRenderer.sharedMaterial = rayData.rayMaterial;
                meshRenderer.GetPropertyBlock(_propBlock);
                _propBlock.SetColor("_MainColor", i.colorMultiplier * localColor);
                _propBlock.SetFloat("_Intensity", i.brightness * rayData.globalBrightness);
                _propBlock.SetFloat("_RayLength", i.raylength * rayData.globalScale);
                _propBlock.SetFloat("_RenderOffset", k - 1);
                _propBlock.SetFloat("_UseCameraDepthFade", rayData.useCameraDepthFade ? 1 : 0);
                _propBlock.SetFloat("_Style", 1);
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

                _propBlock.SetFloat("_AutoAssignSun", rayData.autoAssignSun ? 1 : 0);
                _propBlock.SetFloat("_UseLumenSunScript", rayData.useLumenSun ? 1 : 0);
                _propBlock.SetVector("_SunDirection", rayData.sunDirection);
                _propBlock.SetFloat("_Bidirectional", rayData.bidirectional ? 1 : 0);
                _propBlock.SetFloat("_AngleOpacityEffect", rayData.angleOpacityEffect);
                _propBlock.SetFloat("_AngleRaylengthEffect", rayData.angleRaylengthEffect);

                _propBlock.SetFloat("_UseLightColor", rayData.useLightColor ? 1 : 0);
                meshRenderer.SetPropertyBlock(_propBlock);


            }

            rayData.needsToBeUpdated = false;

        }

    }
}