using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class multi2demo : MonoBehaviour
{
    [SerializeField]
    private Renderer render;

    [SerializeField]
    private Transform[] tfs;

    [SerializeField]
    private float[] radii;

    private Material mat;

    private float[] cachedPos = new float[128];
    private float[] cachedRad = new float[64];
    // Start is called before the first frame update
    void Start()
    {
        mat = render.material; // Creates new Instance of Material (breaks batching)
        render.material = mat;
        radii = new float[tfs.Length];
        for (int i = 0; i < radii.Length; i++)
            radii[i] = Random.Range(20f, 80f);
    }

    // Update is called once per frame
    void Update()
    {
        List<Vector2> positions = new List<Vector2>();
        List<float> radius = new List<float>();
        Camera cam = Camera.main;
        for (int i = 0; i < tfs.Length; i++)
        {
            if (tfs[i].gameObject.activeSelf)
            {
                positions.Add(cam.WorldToScreenPoint(tfs[i].position)); // MAX 64!
                radius.Add(radii[i]);
            }
        }
        mat.SetInt("_UseSeeThrough", 1);
        mat.SetInt("_SeeThroughLength", positions.Count);
        for (int i = 0; i < positions.Count; i++)
        {
            cachedPos[i * 2] = positions[i].x;
            cachedPos[i * 2 + 1] = positions[i].y;
            cachedRad[i] = radius[i];
        }
        mat.SetFloatArray("centers", cachedPos);
        mat.SetFloatArray("radii", cachedRad);
    }
}
