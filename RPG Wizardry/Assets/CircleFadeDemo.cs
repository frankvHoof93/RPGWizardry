using nl.SWEG.RPGWizardry.Utils.DataTypes;
using System.Collections;
using UnityEngine;

public class CircleFadeDemo : MonoBehaviour
{
    [SerializeField]
    private bool Reset;
    [SerializeField]
    private float TimeOut = 4f;
    [SerializeField]
    private float Duration = 1.5f;
    [SerializeField]
    private FloatRange Range;
    [SerializeField]
    private Color overlayColor = Color.green;

    private Material mat;

    [SerializeField]
    private Transform target;

    private float currRadius;

    private Camera cam;

    private void Awake()
    {
        mat = new Material(Shader.Find("Hidden/CircleOverlay"));
    }

    // Start is called before the first frame update
    void Start()
    {
        StopAllCoroutines();
        StartCoroutine(Fade(TimeOut, Duration));
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Reset)
        {
            Start();
            Reset = false;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, mat);
    }

    private IEnumerator Fade(float timeout, float duration)
    {
        mat.SetFloat("_CircleRadius", 100000); // Don't do this.. Only enable the script when needed
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
}
