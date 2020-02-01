using nl.SWEG.Willow.Player;
using nl.SWEG.Willow.Utils.Functions;
using UnityEngine;

namespace nl.SWEG.Willow.GameWorld
{
    [RequireComponent(typeof(Camera))]
    public class ScreenFade : MonoBehaviour
    {
        private Material mat;
        private Camera cam;

        // Start is called before the first frame update
        void Awake()
        {
            mat = new Material(Shader.Find("Hidden/CircleOverlay"));
            mat.SetColor("_OverlayColor", Color.black);
            if (PlayerManager.Exists)
                mat.SetVector("_PlayerPos", cam.WorldToScreenPoint(PlayerManager.Instance.transform.position));
            cam = GetComponent<Camera>();
            SetValue(1);
            enabled = true;

        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            mat.SetVector("_PlayerPos", cam.WorldToScreenPoint(PlayerManager.Instance.transform.position));
            Graphics.Blit(source, destination, mat);
        }

        public void SetValue(float value)
        {
            mat.SetFloat("_CircleRadius", 
                ResolutionMath.ConvertForWidth(value * 
                ResolutionMath.DefaultWidth));
        }
    }
}