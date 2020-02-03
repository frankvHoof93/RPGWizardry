using nl.SWEG.Willow.Player;
using nl.SWEG.Willow.Utils.Functions;
using UnityEngine;

namespace nl.SWEG.Willow.UI.CameraEffects
{
    /// <summary>
    /// Places an overlay over all of the screen. A circle inside of the overlay can be made transparent
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class ScreenFade : MonoBehaviour
    {
        #region Variables
        /// <summary>
        /// Material used for Fading
        /// </summary>
        private Material mat;
        /// <summary>
        /// Camera used to calculate between world- and Screen-Space
        /// </summary>
        private Camera cam;
        /// <summary>
        /// ID for _PlayerPos-Property
        /// </summary>
        private readonly int playerPosID = Shader.PropertyToID("_PlayerPos");
        /// <summary>
        /// ID for _CircleRadius-Property
        /// </summary>
        private readonly int circleRadiusID = Shader.PropertyToID("_CircleRadius");
        #endregion

        #region Methods
        /// <summary>
        /// Sets value for Circle-Radius
        /// </summary>
        /// <param name="value">Value (in pixels, based on 720p-resolution) to set</param>
        public void SetValue(float value)
        {
            mat.SetFloat(circleRadiusID,
                ResolutionMath.ConvertForWidth(value *
                ResolutionMath.DefaultWidth));
        }

        /// <summary>
        /// Creates Material and sets initial value
        /// </summary>
        private void Awake()
        {
            mat = new Material(Shader.Find("Hidden/CircleOverlay"));
            mat.SetColor("_OverlayColor", Color.black);
            if (PlayerManager.Exists)
                mat.SetVector(playerPosID, cam.WorldToScreenPoint(PlayerManager.Instance.transform.position));
            cam = GetComponent<Camera>();
            SetValue(1);
        }
        /// <summary>
        /// Renders Overlay
        /// </summary>
        /// <param name="source">Source-Texture from Camera</param>
        /// <param name="destination">Destination-Texture for Render-Pipeline</param>
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            mat.SetVector(playerPosID, cam.WorldToScreenPoint(PlayerManager.Instance.transform.position));
            Graphics.Blit(source, destination, mat);
        }
        #endregion
    }
}