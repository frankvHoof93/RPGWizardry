using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class multi1demo : MonoBehaviour
{
    [SerializeField]
    private Renderer[] renderers;

    [SerializeField]
    private Transform[] tfs;

    [SerializeField]
    private float[] radii;

    private MaterialPropertyBlock block;

    // Start is called before the first frame update
    void Start()
    {
        block = new MaterialPropertyBlock();
        radii = new float[tfs.Length];
        for (int i = 0; i < radii.Length; i++)
            radii[i] = UnityEngine.Random.Range(25f, 80f);
    }

    // Update is called once per frame
    void Update()
    {
        List<Transform> final = new List<Transform>(4);
        for (int i = 0; i < renderers.Length; i++)
        {
            final.Clear();
            Renderer r = renderers[i];
            r.GetPropertyBlock(block);
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
            block.SetInt("_UseSeeThrough", 1);
            Vector4 radius = new Vector4();
            if (rad.Count > 0)
                radius.x = rad[0];
            if (rad.Count > 1)
                radius.y = rad[1];
            if (rad.Count > 2)
                radius.z = rad[2];
            if (rad.Count > 3)
                radius.w = rad[3];
            block.SetVector("_SeeThroughRadii", radius);
            Vector4 vec = new Vector4();
            block.SetInt("_SeeThroughLength", amount);
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
            block.SetVector("_SeeThroughCenter1", vec);
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
            block.SetVector("_SeeThroughCenter2", vec2);
            r.SetPropertyBlock(block);
        }
    }
}
