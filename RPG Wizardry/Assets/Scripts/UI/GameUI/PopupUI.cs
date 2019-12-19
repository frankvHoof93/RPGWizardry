using TMPro;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.UI.GameUI
{
    [RequireComponent(typeof(Canvas))]
    public class PopupUI : MonoBehaviour
    {
        #region Variables
        #region Editor
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
        private float movementSpeed = .5f;
        /// <summary>
        /// TweenType for Alpha-FadeOut
        /// </summary>
        [SerializeField]
        [Tooltip("TweenType for Alpha-FadeOut")]
        private LeanTweenType fadeOutType = LeanTweenType.easeInBack;
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
        /// Grabs reference to Canvas
        /// </summary>
        private void Awake()
        {
            canvas = GetComponentInParent<Canvas>();
        }
        /// <summary>
        /// Moves UI upwards
        /// </summary>
        private void Update()
        {
            transform.Translate(Vector3.up * movementSpeed * Time.deltaTime);
        }
        #endregion
        #endregion
    }
}