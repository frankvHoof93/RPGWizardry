using nl.SWEG.Willow.Utils.Functions;
using UnityEngine;

namespace nl.SWEG.Willow.UI.Popups
{
    /// <summary>
    /// Used to create Popups during gameplay
    /// </summary>
    public static class PopupFactory
    {
        #region Variables
        #region Constants
        /// <summary>
        /// Default TimeOut before Destroying a Popup
        /// </summary>
        private const float DefaultTimeOut = 1.5f;
        /// <summary>
        /// Default Height for DamageUI (in Pixels, based on 720p-resolution)
        /// </summary>
        private const uint DefaultDamageHeight = 25;
        #endregion

        #region Prefabs
        /// <summary>
        /// DamageUI-Prefab (Loaded from Resources)
        /// </summary>
        private static GameObject dmgUIPrefab;
        #endregion

        #region RuntimeVariables
        /// <summary>
        /// ID of top-most RenderLayer in Game. Filled when first required
        /// </summary>
        private static int? topMostRenderLayer;
        #endregion
        #endregion

        #region Methods
        #region Public
        #region DamageUI
        /// <summary>
        /// Creates a DamageUI. RENDERED ON TOP
        /// </summary>
        /// <param name="posWorld"></param>
        /// <param name="damage"></param>
        /// <param name="color">Color for Text. Defaults to Black</param>
        /// <returns>Created Popup</returns>
        public static PopupUI CreateDamageUI(Vector3 posWorld, ushort damage, Color? color, uint height = DefaultDamageHeight, float timeOut = DefaultTimeOut)
        {
            if (!topMostRenderLayer.HasValue)
                FindTopMostRenderLayer();
            return CreateDamageUI(posWorld, damage, topMostRenderLayer.Value + 1, short.MaxValue, color, height, timeOut);
        }

        /// <summary>
        /// Creates a DamageUI
        /// </summary>
        /// <param name="posWorld">WorldSpace-Position for UI</param>
        /// <param name="damage">Damage to Display</param>
        /// <param name="owner">Renderer for target that was hit (for RenderOrder)</param>
        /// <param name="color">Color for Text (Defaults to Black)</param>
        /// <param name="timeOut">Duration to display UI for (defaults to 1.5s)</param>
        /// <returns>Created Popup</returns>
        public static PopupUI CreateDamageUI(Vector3 posWorld, ushort damage, Renderer owner, Color? color, uint height = DefaultDamageHeight, float timeOut = DefaultTimeOut)
        {
            return CreateDamageUI(posWorld, damage, owner.sortingLayerID, owner.sortingOrder + 1, color, height, timeOut);
        }

        /// <summary>
        /// Creates a DamageUI
        /// </summary>
        /// <param name="posWorld">WorldSpace-Position for UI</param>
        /// <param name="damage">Damage to Display</param>
        /// <param name="renderLayer">RenderLayer for Text-Object</param>
        /// <param name="orderInRenderLayer">Order in RenderLayer</param>
        /// <param name="color">Color for Text (Defaults to Black)</param>
        /// <param name="timeOut">Duration to display UI for (defaults to 1.5s)</param>
        /// <returns>Created UI</returns>
        public static PopupUI CreateDamageUI(Vector3 posWorld, ushort damage, int renderLayer, int orderInRenderLayer, Color? color, uint height = DefaultDamageHeight, float timeOut = DefaultTimeOut)
        {
            if (dmgUIPrefab == null)
                dmgUIPrefab = Resources.Load<GameObject>("UI/DamageUI"); // Load Prefab from Resources
            PopupUI ui = Object.Instantiate(dmgUIPrefab).GetComponent<PopupUI>();
            ui.transform.position = posWorld;
            ui.SetText(damage.ToString(), color ?? Color.black); // Set Damage & Color
            ui.SetTextSize((uint)ResolutionMath.ConvertForHeight(height)); // Set text Size
            ui.SetRenderOrder(renderLayer, orderInRenderLayer); // Set RenderSettings
            ui.SetDestructionTimer(timeOut); // Set Timer
            return ui;
        }
        #endregion
        #endregion

        #region Private
        /// <summary>
        /// Finds and stores ID for Top-Most RenderLayer in the Game
        /// </summary>
        private static void FindTopMostRenderLayer()
        {
            int lowest = 0;
            SortingLayer[] layers = SortingLayer.layers;
            for (int i = 0; i < layers.Length; i++)
                if (layers[i].value < lowest)
                {
                    lowest = layers[i].value;
                    topMostRenderLayer = layers[i].id;
                }
        }
        #endregion
        #endregion
    }
}