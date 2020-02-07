using nl.SWEG.Willow.Research.Data;
using nl.SWEG.Willow.Research.Data.DebugData;
using nl.SWEG.Willow.Research.IO;
using nl.SWEG.Willow.UI.Spells;
using nl.SWEG.Willow.Utils.Functions;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace nl.SWEG.Willow.Research
{
    /// <summary>
    /// Manages Research-Data, and renders Points on UI for Research-MiniGame
    /// </summary>
    public class ResearchManager : MonoBehaviour
    {
        #region Variables
        #region Editor
        #pragma warning disable 0649 // Hide Null-Warning for Editor-Variables
        /// <summary>
        /// Manager for SpellPage-UI
        /// </summary>
        [SerializeField]
        [Tooltip("Manager for SpellPage-UI")]
        private SpellPageManager spellManager;
        /// <summary>
        /// UI-Images to draw Points on
        /// </summary>
        [Header("UI")]
        [SerializeField]
        [Tooltip("UI-Images to draw Points on")]
        private List<Image> images = null;
        /// <summary>
        /// Message to display after Unlock
        /// </summary>
        [SerializeField]
        [Tooltip("Message to display after Unlock")]
        private TextMeshProUGUI unlockMsg;
        /// <summary>
        /// Texture used for a Point
        /// </summary>
        [SerializeField]
        [Tooltip("Texture used for a Point")]
        private Texture2D splatTex;
        /// <summary>
        /// Button to check Research for completion
        /// </summary>
        [SerializeField]
        [Tooltip("Button to check Research for completion")]
        private Button checkButton;
        #pragma warning restore 0649 // Restore Null-Warning after Editor-Variables
        #endregion

        #region Private
        /// <summary>
        /// Current DataBin to be used for mini-game/research
        /// </summary>
        private DataBin currentBin;
        /// <summary>
        /// Current DataSet to be used for mini-game/research
        /// </summary>
        private DataSet currentSet;
        /// <summary>
        /// ID for _DecalTex-Property
        /// </summary>
        private readonly int decalTexID = Shader.PropertyToID("_DecalTex");
        /// <summary>
        /// ID for splatCount-Property
        /// </summary>
        private readonly int splatCountID = Shader.PropertyToID("splatCount");
        /// <summary>
        /// ID for UVs-Property
        /// </summary>
        private readonly int uvID = Shader.PropertyToID("UVs");
        #endregion
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Checks if the Player has solved the current DataSet
        /// </summary>
        public void CheckIfSolved()
        {
            if (currentSet.CheckDataSolved())
            {
                spellManager.UnlockSpell();
                unlockMsg.enabled = true;
                checkButton.enabled = false;
                StartCoroutine(CoroutineMethods.RunDelayed(SwitchToSpellPage, 3f));
            }
            else
                unlockMsg.enabled = false;
        }
        #endregion

        #region Unity
        /// <summary>
        /// Applies UI when Manager is Enabled
        /// </summary>
        private void OnEnable()
        {
            UpdateResearch();
        }

        /// <summary>
        /// Re-Applies UI if Game regains Focus on Device
        /// </summary>
        /// <param name="focus">True for gain of Focus, False for loss of Focus</param>
        private void OnApplicationFocus(bool focus)
        {
            if (focus)
            {
                try
                {
                    PopulateUI();
                }
                catch (NullReferenceException)
                {
                    OnEnable();
                }
            }
        }
        #endregion

        #region Private
        /// <summary>
        /// Loads and Displays Research-Data
        /// </summary>
        private void UpdateResearch()
        {
            if (currentBin == null)
                currentBin = LoadDataBin();
            LoadNextDataSet();
            PopulateUI();
            checkButton.enabled = true;
            unlockMsg.enabled = false;
        }

        #region Loading
        /// <summary>
        /// Loads the next unsolved DataSet in the current DataBin
        /// </summary>
        private void LoadNextDataSet()
        {
            if (!currentBin.IsDataBinSolved())
                currentSet = currentBin.FirstUnsolvedDataSet();
        }

        /// <summary>
        /// Loads DataBin
        /// TODO: Remove Debug-DataBin and load actual Research
        /// </summary>
        /// <returns>Loaded Bin</returns>
        private DataBin LoadDataBin()
        {
            return DataStubBin.Bin;
        }
        #endregion

        #region UI
        /// <summary>
        /// Switches to SpellPage
        /// </summary>
        private void SwitchToSpellPage()
        {
            spellManager.gameObject.SetActive(true);
            transform.gameObject.SetActive(false);
        }

        /// <summary>
        /// Populates the Images on the Bars with Points for Research-Data
        /// </summary>
        private void PopulateUI()
        {
            for (int i = 0; i < 10; i++)
            {
                // Set UI-Image to Fragment
                Image imgRenderer = images[i];
                Fragment fragment = currentSet.Fragments[i];
                fragment.SetImage(imgRenderer);
                Texture2D imgTex = (Texture2D)imgRenderer.mainTexture;
                float targetScale = currentSet.Fragments[i].ImgData.Length;
                // Create Material for Splatting (break batching on purpose)
                Material m = new Material(Shader.Find("Custom/TextureDecal"));
                m.SetTexture(decalTexID, splatTex);
                imgRenderer.material = m;
                // Calculate tiling for Points in Fragment
                Vector4[] vectors = new Vector4[(int)targetScale];
                float xPos = (imgTex.width - 30f) / currentSet.Fragments[i].ImgData.Length;
                for (int j = 0; j < currentSet.Fragments[i].ImgData.Length; j++)
                {
                    Vector2 tiling = new Vector2(((float)imgTex.width / splatTex.width), ((float)imgTex.height / splatTex.height));
                    // base offset to corner:
                    Vector2 offset = new Vector2(.5f, .5f);
                    Vector2 posOnTarget = new Vector2(xPos * j, (int)(currentSet.Fragments[i].ImgData[j] * imgTex.height));
                    posOnTarget = new Vector2(posOnTarget.x / splatTex.width, posOnTarget.y / splatTex.height);
                    offset += posOnTarget;
                    offset *= -1f;
                    // Add Splat to Img
                    vectors[j] = new Vector4(tiling.x, tiling.y, offset.x, offset.y);
                }
                // Set Points to Material
                m.SetInt(splatCountID, vectors.Length);
                m.SetVectorArray(uvID, vectors);
            }
        }
        #endregion
        #endregion
        #endregion
    }
}