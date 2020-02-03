using nl.SWEG.Willow.GameWorld;
using nl.SWEG.Willow.Utils.Functions;
using System;
using TMPro;
using UnityEngine;

namespace nl.SWEG.Willow.UI.Popups
{
    /// <summary>
    /// Displays Text, fading out & moving upwards
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class PopupUI : MonoBehaviour
    {
        #region Variables
        #region Editor
        #pragma warning disable 0649 // Hide Null-Warning for Editor-Variables
        /// <summary>
        /// TextField to Display Text
        /// </summary>
        [SerializeField]
        [Tooltip("TextField to Display Text")]
        private TextMeshProUGUI textField;
        /// <summary>
        /// Speed at which Canvas moves upward
        /// </summary>
        [SerializeField]
        [Tooltip("Speed at which Canvas moves upward")]
        private float movementSpeed = .5f;
        /// <summary>
        /// TweenType for Alpha-FadeOut
        /// </summary>
        [SerializeField]
        [Tooltip("TweenType for Alpha-FadeOut")]
        private LeanTweenType fadeOutType = LeanTweenType.easeInBack;
        #pragma warning restore 0649 // Restore Null-Warning after Editor-Variables
        #endregion

        #region Private
        /// <summary>
        /// Canvas for UI
        /// </summary>
        private Canvas canvas;
        #endregion
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Sets TextSize for text based on Camera-Size
        /// </summary>
        /// <param name="pixelsHeight">Height for text in Pixels (ScreenSpace)</param>
        public void SetTextSize(uint pixelsHeight)
        {
            if (CameraManager.Exists)
            {
                float realworldSize = CameraManager.Instance.Camera.orthographicSize * 2f; // Size of visible Height in WorldSpace
                float percentage = pixelsHeight / ResolutionMath.DefaultHeight; // Convert to percentage of full Height
                realworldSize = percentage * realworldSize;
                RectTransform textTf = (RectTransform)textField.transform;
                textTf.sizeDelta = new Vector2(realworldSize * 5f, realworldSize);
            }
            else
                throw new InvalidOperationException("No Camera Exists. TextSize cannot be determined");
        }

        /// <summary>
        /// Sets Text to Display
        /// </summary>
        /// <param name="text">String-Text to show</param>
        /// <param name="textColor">Color for text. Set null to keep current color</param>
        public void SetText(string text, Color? textColor = null)
        {
            textField.text = text;
            if (textColor.HasValue)
            {
                Color toSet = textColor.Value;
                toSet.a = 1f; // force alpha to 1
                textField.color = toSet;
            }
        }

        /// <summary>
        /// Sets Render-Order for Text
        /// </summary>
        /// <param name="layer">RenderLayer for Text</param>
        /// <param name="orderInLayer">Order within Layer</param>
        public void SetRenderOrder(int layer, int orderInLayer)
        {
            if (canvas == null)
                canvas = GetComponentInParent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingLayerID = layer;
            canvas.sortingOrder = orderInLayer;
        }

        /// <summary>
        /// Sets Alpha for Text
        /// </summary>
        /// <param name="alpha">Alpha to Set</param>
        public void SetAlpha(float alpha)
        {
            textField.alpha = alpha;
        }

        /// <summary>
        /// Sets timeout for PopupUI. Fades to Alpha-0 then destroys itself
        /// </summary>
        /// <param name="timer">Duration of Fade</param>
        public void SetDestructionTimer(float timer)
        {
            LeanTween.value(gameObject, SetAlpha, textField.alpha, 0f, timer) // Tween alpha to 0
                .setOnComplete(() => Destroy(gameObject)) // Destroy when done   
                .tweenType = fadeOutType; // Set TweenType from Editor
        }
        #endregion

        #region Unity
        /// <summary>
        /// Moves UI upwards (WorldSpace)
        /// </summary>
        private void Update()
        {
            transform.Translate(Vector3.up * movementSpeed * Time.deltaTime);
        }
        #endregion
        #endregion
    }
}