using nl.SWEG.Willow.GameWorld;
using nl.SWEG.Willow.Utils.Functions;
using System.Collections.Generic;
using UnityEngine;

namespace nl.SWEG.Willow.UI.CameraEffects.Opacity
{
    /// <summary>
    /// Manages Opacity for Small Objects. This can handle few Opacity-Objects (Max 4), but works with GPU-Instancing
    /// <para>
    /// To be used with the OpacitySmallShader
    /// </para>
    /// </summary>
    public class OpacityManagerSmallBatch : OpacityManager
    {
        #region Variables
        /// <summary>
        /// PropertyBlocks for Materials being managed
        /// </summary>
        private MaterialPropertyBlock[] propertyBlocks;
        /// <summary>
        /// ID for _UseSeeThrough-Property
        /// </summary>
        private int useSeeThroughID = Shader.PropertyToID("_UseSeeThrough");
        /// <summary>
        /// ID for _SeeThroughLength-Property
        /// </summary>
        private int seeThroughLengthID = Shader.PropertyToID("_SeeThroughLength");
        /// <summary>
        /// ID for _SeeThroughRadii-Property
        /// </summary>
        private int seeThroughRadiiID = Shader.PropertyToID("_SeeThroughRadii");
        /// <summary>
        /// ID for _SeeThroughCenter1-Property
        /// </summary>
        private int seeThroughCenter1ID = Shader.PropertyToID("_SeeThroughCenter1");
        /// <summary>
        /// ID for _SeeThroughCenter2-Property
        /// </summary>
        private int seeThroughCenter2ID = Shader.PropertyToID("_SeeThroughCenter2");
        #endregion

        #region Methods
        /// <summary>
        /// Sets up MaterialPropertyBlocks
        /// </summary>
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
        /// <summary>
        /// Sets Opacity-Values to Shader
        /// </summary>
        /// <param name="objects">List of Objects to handle</param>
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
                mpb.SetInt(useSeeThroughID, 1);
                mpb.SetInt(seeThroughLengthID, amount);
                mpb.SetVector(seeThroughRadiiID, radius);
                mpb.SetVector(seeThroughCenter1ID, vec);
                mpb.SetVector(seeThroughCenter2ID, vec2);
                renderers[i].SetPropertyBlock(mpb);
            }
        }
        #endregion
    }
}