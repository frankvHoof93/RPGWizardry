using nl.SWEG.Willow.Utils.Functions;
using System.Collections.Generic;
using UnityEngine;

namespace nl.SWEG.Willow.GameWorld
{
    public class OpacityManagerLargeBatch : OpacityManager
    {
        #region Variables
        /// <summary>
        /// Cached Array for Positions
        /// </summary>
        private float[] cachedPos = new float[128];
        /// <summary>
        /// Cached Array for Radii
        /// </summary>
        private float[] cachedRad = new float[64];
        /// <summary>
        /// Materials to apply Opacity to
        /// </summary>
        private Material[] materials;
        #endregion

        #region Methods
        /// <summary>
        /// Sets up Materials
        /// </summary>
        private void Start()
        {
            materials = new Material[renderers.Length];
            for (int i = 0; i < renderers.Length; i++)
            {
                materials[i] = renderers[i].material;
                renderers[i].material = materials[i]; // Break Batching, Create Materials
            }
        }
        /// <summary>
        /// Sets Opacity to Materials
        /// </summary>
        /// <param name="objects">Objects to set Opacity for (max 64)</param>
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
        #endregion
    }
}