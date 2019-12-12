using nl.SWEG.RPGWizardry.Utils.Functions;
using System.Collections.Generic;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.GameWorld
{
    /// <summary>
    /// To be used with the OpacitySmallShader
    /// </summary>
    public class OpacityManagerSmallBatch : OpacityManager
    {
        MaterialPropertyBlock[] propertyBlocks;

        private void Start()
        {
            propertyBlocks = new MaterialPropertyBlock[renderers.Length];
            for (int i = 0; i < renderers.Length; i++)
            {
                MaterialPropertyBlock block = new MaterialPropertyBlock();
                renderers[i].GetPropertyBlock(block);
                propertyBlocks[i] = block;
            }
        }

        protected override void SetToShader(List<OpacityObject> objects)
        {
            if (!CameraManager.Exists)
                return; // No active Camera

            Camera cam = CameraManager.Instance.Camera;
            int amount = objects.Count > 4 ? 4 : objects.Count;

            Vector4 vec = new Vector4(), vec2 = new Vector4(), radius = new Vector4();
            for (int i = 0; i < amount; i++)
            {
                OpacityObject obj = objects[i];
                Vector3 screenPos = cam.WorldToScreenPoint(obj.transform.position + (Vector3)obj.opacity.OpacityOffset);
                Vector4 v = i < 2 ? vec : vec2;
                if (i % 2 == 0) // Even indices
                {
                    v.x = screenPos.x;
                    v.y = screenPos.y;
                }
                else // i % 2 == 1 Odd indices
                {
                    v.z = screenPos.x;
                    v.w = screenPos.y;
                }
                if (i < 2) // Set to vector (reference)
                    vec = v;
                else
                    vec2 = v;
                radius[i] = ResolutionMath.ConvertForWidth(obj.opacity.OpacityRadius);
            }
            for (int i = 0; i < renderers.Length; i++)
            {
                MaterialPropertyBlock mpb = propertyBlocks[i];
                renderers[i].GetPropertyBlock(mpb);
                mpb.SetInt("_UseSeeThrough", 1);
                mpb.SetInt("_SeeThroughLength", amount);
                mpb.SetVector("_SeeThroughRadii", radius);
                mpb.SetVector("_SeeThroughCenter1", vec);
                mpb.SetVector("_SeeThroughCenter2", vec2);
                renderers[i].SetPropertyBlock(mpb);
            }
        }
    }
}