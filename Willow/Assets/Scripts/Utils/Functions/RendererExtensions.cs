using UnityEngine;

namespace nl.SWEG.Willow.Utils.Functions
{
    /// <summary>
    /// Extension-Functions for Unity Renderer
    /// </summary>
    public static class RendererExtensions
    {
        /// <summary>
        /// MaterialPropertyBlock-Instance used to set variables to materials
        /// </summary>
        private static MaterialPropertyBlock block;
        /// <summary>
        /// Sets color to Renderer (_Color-Variable)
        /// </summary>
        /// <param name="r">Renderer to set to</param>
        /// <param name="c">Color to set</param>
        public static void SetSpriteColor(this Renderer r, Color c)
        {
            if (block == null)
                block = new MaterialPropertyBlock();
            r.GetPropertyBlock(block);
            block.SetColor("_Color", c);
            r.SetPropertyBlock(block);
        }
    }
}
