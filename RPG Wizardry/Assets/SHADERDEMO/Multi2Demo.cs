using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script should be re-written to work with a single renderer. This script can create at most 64 'circles' through an image, but breaks batching
/// </summary>
public class Multi2Demo : MonoBehaviour
{
    #region Variables
    #region Editor
    /// <summary>
    /// Renderer this Scripts works on
    /// </summary>
    [SerializeField]
    private Renderer render;
    /// <summary>
    /// Transform-Targets for Circle-Centers (MAX 64)
    /// </summary>
    [SerializeField]
    private Transform[] tfs;
    /// <summary>
    /// Radius for each Transform-Target (MAX 64)
    /// </summary>
    [SerializeField]
    private float[] radii;
    #endregion

    #region Private
    /// <summary>
    /// Material for Renderer. This Material is created on Start (from the Renderer)
    /// </summary>
    private Material mat;
    /// <summary>
    /// Cached Array for Positions, to prevent allocating one each frame
    /// </summary>
    private float[] cachedPos = new float[128];
    /// <summary>
    /// Cached Array for Radii, to prevent allocating one each frame
    /// </summary>
    private float[] cachedRad = new float[64];
    #endregion
    #endregion

    #region Methods
    /// <summary>
    /// Sets up Material & Radii
    /// </summary>
    private void Start()
    {
        mat = render.material; // Creates new Instance of Material (breaks batching)
        radii = new float[tfs.Length];
        for (int i = 0; i < radii.Length; i++)
            radii[i] = Random.Range(20f, 80f);
    }
    /// <summary>
    /// Sets Variables to Material (MATERIAL, NOT RENDERER-INSTANCE!)
    /// </summary>
    private void Update()
    {
        List<Vector2> positions = new List<Vector2>();
        List<float> radius = new List<float>();
        Camera cam = Camera.main;
        #region CalcVariables
        // Find Positions & Radii
        for (int i = 0; i < tfs.Length; i++)
        {
            if (tfs[i].gameObject.activeSelf)
            {
                positions.Add(cam.WorldToScreenPoint(tfs[i].position)); // MAX 64!
                radius.Add(radii[i]);
            }
        }
        // Set to Arrays
        for (int i = 0; i < positions.Count; i++)
        {
            cachedPos[i * 2] = positions[i].x;
            cachedPos[i * 2 + 1] = positions[i].y;
            cachedRad[i] = radius[i];
        }
        #endregion
        // Set Variables to Material
        mat.SetInt("_UseSeeThrough", 1);
        mat.SetInt("_SeeThroughLength", positions.Count);
        mat.SetFloatArray("centers", cachedPos);
        mat.SetFloatArray("radii", cachedRad);
    }
    #endregion
}
