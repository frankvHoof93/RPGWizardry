using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace nl.SWEG.RPGWizardry.Sorcery
{
    public class SpellBarManager : MonoBehaviour
    {
        [SerializeField]
        public Vector4[] uvs;

        public float[] imgData;

        private float targetScale;

        [SerializeField]
        private Texture2D splatTex;

        private int lastLength = 0;

        private Image renderer;
        private Material m;
        private int uvID, uvLID;

        private readonly Vector4[] cache = new Vector4[32];

        // Start is called before the first frame update
        void Start()
        {

        }
        
        public void UpdateValues()
        {

        }
        // Update is called once per frame
        void Update()
        {
            // Array length can only be set once, thus changing length requires a new material or an indexer
            if (uvs.Length != lastLength)
                m.SetInt(uvLID, uvs.Length);
            if (uvs.Length != 0 && m != null)
            {
                // this should not be done every update, and is only here to update from editor for Demo
                Array.Copy(uvs, 0, cache, 0, Mathf.Clamp(uvs.Length, 0, 32));
                m.SetVectorArray(uvID, cache);
            }
        }
    }
}
