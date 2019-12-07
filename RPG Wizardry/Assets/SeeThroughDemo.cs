using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeThroughDemo : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer[] renderers;
    /// <summary>
    /// MPB is a class, but is used like a struct in Unity. Thus, we can use just a single instance, as long as we read every time. 
    /// TODO: Check timings vs memory on tis
    /// </summary>
    private MaterialPropertyBlock block;

    private void Awake()
    {
        block = new MaterialPropertyBlock();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].GetPropertyBlock(block);
            block.SetFloat("_UseSeeThrough", UnityEngine.Random.Range((int)0, (int)2));
            renderers[i].SetPropertyBlock(block);
        }
    }


    // Update is called once per frame
    private void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].GetPropertyBlock(block);
            block.SetVector("_SeeThroughCenter", new Vector4(screenPos.x, screenPos.y, 0, 0));
            renderers[i].SetPropertyBlock(block);
        }
    }
}
