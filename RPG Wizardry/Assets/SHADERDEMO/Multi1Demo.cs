using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This script should be re-written to work with a single renderer. This script can create at most 4 'circles' through an image, using GPU-Instancing
/// </summary>
public class Multi1Demo : MonoBehaviour
{
    #region Variables
    #region Editor
    /// <summary>
    /// Renderers this Script works on
    /// </summary>
    [SerializeField]
    private Renderer[] renderers;
    /// <summary>
    /// Transform-Targets for Circle-Centers
    /// </summary>
    [SerializeField]
    private Transform[] tfs;
    /// <summary>
    /// Radius for each Transform-Target
    /// </summary>
    [SerializeField]
    private float[] radii;
    #endregion

    #region Private
    /// <summary>
    /// Cached MaterialPropertyBlock
    /// </summary>
    private MaterialPropertyBlock block;
    #endregion
    #endregion

    #region Methods
    /// <summary>
    /// Creates MPB & sets up Radii
    /// </summary>
    private void Start()
    {
        block = new MaterialPropertyBlock();
        radii = new float[tfs.Length];
        for (int i = 0; i < radii.Length; i++)
            radii[i] = UnityEngine.Random.Range(25f, 80f);
    }
    /// <summary>
    /// Sets values for Renderer-Instances using MaterialPropertyBlock
    /// </summary>
    private void Update()
    {
        List<Transform> final = new List<Transform>(4);
        for (int i = 0; i < renderers.Length; i++)
        {
            final.Clear();
            Renderer r = renderers[i];
            // Read Current MPB-Values from Renderer (only necessary if renderer itself can/has changed any values)
            r.GetPropertyBlock(block);
            #region CalcVariables
            // Find 4 closest tfs
            List<Transform> closest = tfs.Where(tf => tf.gameObject.activeSelf).OrderBy(tf => Vector3.Distance(tf.position, r.transform.position)).ToList();
            int amount = Mathf.Min(closest.Count, 4);
            List<float> rad = new List<float>(4);
            for (int j = 0; j < amount; j++)
            {
                final.Add(closest[j]);
                if (j < 4)
                    rad.Add(radii[Array.IndexOf(tfs, closest[j])]);
            }
            // Set Radius-Values to a Vector4
            Vector4 radius = new Vector4();
            if (rad.Count > 0)
                radius.x = rad[0];
            if (rad.Count > 1)
                radius.y = rad[1];
            if (rad.Count > 2)
                radius.z = rad[2];
            if (rad.Count > 3)
                radius.w = rad[3];
            // Set Center-Values to Vector4-1
            Vector4 vec = new Vector4();
            if (final.Count > 0)
            {
                Vector3 screenSpace = Camera.main.WorldToScreenPoint(final[0].position);
                vec.x = screenSpace.x;
                vec.y = screenSpace.y;
            }
            if (final.Count > 1)
            {
                Vector3 screenSpace = Camera.main.WorldToScreenPoint(final[1].position);
                vec.z = screenSpace.x;
                vec.w = screenSpace.y;
            }
            // Set Center-Values to Vector4-4
            Vector4 vec2 = new Vector4();
            if (final.Count > 2)
            {
                Vector3 screenSpace = Camera.main.WorldToScreenPoint(final[2].position);
                vec2.x = screenSpace.x;
                vec2.y = screenSpace.y;
            }
            if (final.Count > 3)
            {
                Vector3 screenSpace = Camera.main.WorldToScreenPoint(final[3].position);
                vec2.z = screenSpace.x;
                vec2.w = screenSpace.y;
            }
            #endregion
            // Set Variables to MPB
            block.SetInt("_UseSeeThrough", 1);
            block.SetInt("_SeeThroughLength", amount);
            block.SetVector("_SeeThroughRadii", radius);
            block.SetVector("_SeeThroughCenter1", vec);
            block.SetVector("_SeeThroughCenter2", vec2);
            // Set MPB to renderer
            r.SetPropertyBlock(block);
        }
    }
    #endregion
}