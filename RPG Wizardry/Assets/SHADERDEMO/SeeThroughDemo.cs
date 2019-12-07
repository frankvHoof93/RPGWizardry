using UnityEngine;

/// <summary>
/// This script should be re-written to work with a single renderer. This script can create at most 1 'circle' through an image
/// </summary>
public class SeeThroughDemo : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// Renderers this Script works on
    /// </summary>
    [SerializeField]
    private SpriteRenderer[] renderers;
    /// <summary>
    /// MPB is a class, but is used like a struct in Unity. Thus, we can use just a single instance, as long as we read every time.
    /// If this code is put on an instance (i.e. in a MonoBehaviour, you can just keep the current instance, as long as no code is trying to change properties via the renderer
    /// </summary>
    private MaterialPropertyBlock block;
    #endregion

    #region Methods
    /// <summary>
    /// Creates MPB, and sets UseSeeThrough to all Renderers (Random Value)
    /// </summary>
    private void Awake()
    {
        block = new MaterialPropertyBlock();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].GetPropertyBlock(block); // Read Properties
            block.SetFloat("_UseSeeThrough", UnityEngine.Random.Range((int)0, (int)2)); // Set Float (Bool)
            renderers[i].SetPropertyBlock(block); // Write Properties
        }
    }
    /// <summary>
    /// Sets ScreenSpace-Position for Transform to Renderer-Instance (INSTANCE, NOT MATERIAL)
    /// </summary>
    private void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position); // ScreenSpace-Position for Transform
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].GetPropertyBlock(block); // Read Properties
            block.SetVector("_SeeThroughCenter", new Vector4(screenPos.x, screenPos.y, 0, 0)); // Set Position
            renderers[i].SetPropertyBlock(block); // Write Properties
        }
    }
    #endregion
}