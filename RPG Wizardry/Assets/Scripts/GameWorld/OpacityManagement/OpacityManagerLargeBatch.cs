using nl.SWEG.RPGWizardry.Utils.Functions;
using System.Collections.Generic;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.GameWorld
{
    public class OpacityManagerLargeBatch : OpacityManager
    {
        private float[] cachedPos = new float[128];
        private float[] cachedRad = new float[64];

        private Material[] materials;

        protected override void SetToShader(List<OpacityObject> objects)
        {
            if (!CameraManager.Exists)
                return;
            Camera cam = CameraManager.Instance.Camera;
            int amount = Mathf.Min(objects.Count, 64);
            for (int i = 0; i < amount; i++)
            {
                OpacityObject obj = objects[i];
                cachedRad[i] = ResolutionMath.ConvertForWidth(obj.opacity.OpacityRadius);
                // Move to ScreenSpace
                Vector3 pos = cam.WorldToScreenPoint(obj.transform.position + (Vector3)obj.opacity.OpacityOffset);
                cachedPos[i * 2] = pos.x;
                cachedPos[i * 2 + 1] = pos.y;
            }
            for (int i = 0; i < materials.Length; i++)
            {
                Material mat = materials[i];
                mat.SetInt("_UseSeeThrough", 1);
                mat.SetInt("_SeeThroughLength", amount);
                mat.SetFloatArray("centers", cachedPos);
                mat.SetFloatArray("radii", cachedRad);
            }
        }

        private void Start()
        {
            materials = new Material[renderers.Length];
            for (int i = 0; i < renderers.Length; i++)
            {
                materials[i] = renderers[i].material;
                renderers[i].material = materials[i]; // Break Batching, Create Materials
            }
        }
    }
}