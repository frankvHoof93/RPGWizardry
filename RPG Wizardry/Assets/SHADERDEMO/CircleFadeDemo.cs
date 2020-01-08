using nl.SWEG.RPGWizardry.Utils.DataTypes;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Camera))] // Script must be on Camera to run OnRenderImage
public class CircleFadeDemo : MonoBehaviour
{
    #region Variables
    #region Editor
    /// <summary>
    /// Resets Demo via Inspector
    /// </summary>
    [SerializeField]
    private bool Reset;
    /// <summary>
    /// TimeOut before running Demo
    /// </summary>
    [SerializeField]
    private float TimeOut = 4f;
    /// <summary>
    /// Duration of CircleFade
    /// </summary>
    [SerializeField]
    private float Duration = 1.5f;
    /// <summary>
    /// Range for Fading (Min- & Max-Size)
    /// </summary>
    [SerializeField]
    private FloatRange Range;
    /// <summary>
    /// Color for Fade-Overlay
    /// </summary>
    [SerializeField]
    private Color overlayColor = Color.green;
    /// <summary>
    /// Target to center fade on
    /// </summary>
    [SerializeField]
    private Transform target;
    #endregion

    #region Private
    /// <summary>
    /// Material to use for Blitting (Material with Shader & Settings)
    /// </summary>
    private Material mat;
    /// <summary>
    /// Current Radius for CircleFade
    /// </summary>
    private float currRadius;
    /// <summary>
    /// Camera used for Blitting (Provides Input-Image)
    /// </summary>
    private Camera cam;
    #endregion
    #endregion

    #region Methods
    #region Unity
    /// <summary>
    /// Creates Material for Effect
    /// Grabs Reference to Camera
    /// </summary>
    private void Awake()
    {
        mat = new Material(Shader.Find("Hidden/CircleOverlay"));
        cam = GetComponent<Camera>();
    }
    /// <summary>
    /// Runs for Reset
    /// </summary>
    private void Start()
    {
        StopAllCoroutines();
        StartCoroutine(Fade(TimeOut, Duration));
    }
    /// <summary>
    /// Checks Inspector-Value for Reset and runs Reset
    /// </summary>
    private void Update()
    {
        if (Reset)
        {
            Start();
            Reset = false;
        }
    }
    /// <summary>
    /// Runs Effect. Takes input-image from camera, adds shader-effect, then outputs to destination
    /// </summary>
    /// <param name="source">Input-Texture from RenderPipeline</param>
    /// <param name="destination">Output-Texture to RenderPipeline</param>
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, mat);
    }
    #endregion

    #region Coroutine
    /// <summary>
    /// Coroutine for Setting CircleRadius
    /// </summary>
    /// <param name="timeout">TimeOut before setting first Radius</param>
    /// <param name="duration">Duration for shrinking</param>
    private IEnumerator Fade(float timeout, float duration)
    {
        mat.SetFloat("_CircleRadius", 100000); // Don't do this in production.. Only enable the script when needed
        mat.SetColor("_OverlayColor", overlayColor);
        currRadius = Range.Max;
        yield return new WaitForSeconds(timeout);
        float slope = (Range.Max - Range.Min) / duration;
        float currTime = 0;

        while (currTime < duration)
        {
            currTime = Mathf.Clamp(currTime + Time.deltaTime, 0, duration);
            currRadius = Mathf.Clamp((duration - currTime) * slope + Range.Min, Range.Min, Range.Max);
            Vector3 screenPos = cam.WorldToScreenPoint(target.position);
            mat.SetVector("_PlayerPos", new Vector4(screenPos.x, Screen.height - screenPos.y, 0, 0)); // Inverted Y
            mat.SetFloat("_CircleRadius", currRadius);
            yield return null;
        }
    }
    #endregion
    #endregion
}