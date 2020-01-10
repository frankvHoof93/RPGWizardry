using nl.SWEG.RPGWizardry.Sorcery;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace nl.SWEG.RPGWizardry.ResearchData
{
    public class DataManager : MonoBehaviour
    {
        #region Variables
        /// <summary>
        /// Current bin to be used for minigames/research
        /// </summary>
        private DataBin CurrentBin;

        /// <summary>
        /// Current selection of fragments used for the minigame
        /// </summary>
        private DataSet CurrentSet;

        /// <summary>
        /// The bar images that need to be drawn on.
        /// </summary>
        [SerializeField]
        private List<Image> images;

        /// <summary>
        /// Spell manager used to switch to based on research result
        /// </summary>
        [SerializeField]
        private SpellPageManager spellManager;

        /// <summary>
        /// Message to show if you unlocked the spell or not.
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI message;

        [SerializeField]
        private Texture2D splatTex;

        /// <summary>
        /// Button the player can use to check if he did the research correctly.
        /// </summary>
        [SerializeField]
        private Button checkButton;
        #endregion
        #region Methods
        // Start is called before the first frame update
        void Start()
        {
            //TODO: Currently the null check is mainly used to circumvent the constant reloading of the datastub
            if(CurrentBin == null)
            {
                CurrentBin = LoadDataBin();
            }
            PopulateUI();
            checkButton.enabled = true;
            message.enabled = false;

        }

        private void Awake()
        {

        }

        /// <summary>
        /// Perform start when page is enabled again.
        /// </summary>
        private void OnEnable()
        {
            Start();
        }

        // Update is called once per frame
        void Update()
        {
           
        }

        /// <summary>
        /// Used to check if the player has solved the research Set
        /// </summary>
        public void CheckIfSolved()
        {
            if (CurrentSet.CheckDataSolved())
            {
                spellManager.UnlockSpell();
                message.enabled = true;
                checkButton.enabled = false;
            }
            else
            {
                message.enabled = false;
            }
        }

        //TODO: Convert to RenderTexture, can't currently work out how to do it.
        /// <summary>
        /// Used to populate the images on the slider bars for the spellcrafting minigame.
        /// </summary>
        public void PopulateUI()
        {
            LoadDataSet();
            Debug.Log("Fragments: " + CurrentSet.Fragments.Count);
            for (int i = 0; i < 10; i++)
            {
                //old shit
                CurrentSet.Fragments[i].FragmentImage = images[i];
                CurrentSet.Fragments[i].ImageTransform = images[i].transform.parent;
                Texture2D imgTex = (Texture2D)CurrentSet.Fragments[i].FragmentImage.mainTexture;
                //ClearTextures(imgTex);

                float targetScale = CurrentSet.Fragments[i].ImgData.Length;


                Image renderer = CurrentSet.Fragments[i].FragmentImage;
                Material m = new Material(Shader.Find("Custom/TextureDecal"));
                m.SetTexture("_DecalTex", splatTex);
                renderer.material = m;


                Vector4[] vectors = new Vector4[(int)targetScale];

                for (int j = 0; j < CurrentSet.Fragments[i].ImgData.Length - 1; j++)
                {
                    Vector2 tiling = new Vector2(((float)imgTex.width / (float)splatTex.width), ((float)imgTex.height / (float)splatTex.height));
                    // base offset to corner:
                    Vector2 offset = new Vector2(.5f, .5f); // TODO: Check this

                    Vector2 posOnTarget = new Vector2(UnityEngine.Random.Range(0, imgTex.width-20), (int)(CurrentSet.Fragments[i].ImgData[j] * imgTex.height));
                //    Debug.Log("PosOnTarget: " + posOnTarget);
                    posOnTarget = new Vector2(posOnTarget.x / splatTex.width, posOnTarget.y / splatTex.height);
                    offset += posOnTarget;
                    offset *= -1f;
                //    Debug.Log($"Tiling: {tiling.ToString("N2")} Offset: {offset.ToString("N2")}");
                    // Add Pixel to Img
                    vectors[j] = new Vector4(tiling.x, tiling.y, offset.x, offset.y);
                    m.SetInt("splatCount", vectors.Length);
                    m.SetVectorArray("UVs", vectors);

                    //imgTex.SetPixel(UnityEngine.Random.Range(0, imgTex.width), (int)(CurrentSet.Fragments[i].ImgData[j] * imgTex.height), Color.magenta);
                }
                //imgTex.Apply();
                

            }

        }


        /// <summary>
        /// Cleares the images and reset it to their base state.
        /// </summary>
        /// <param name="tex">Target texture</param>
        private void ClearTextures(Texture2D tex)
        {
          //  for (int x = 0; x < tex.width; x++)
          //      for (int y = 0; y < tex.height; y++)
           //         tex.SetPixel(x, y, new Color(0,0,0,0));
           // tex.Apply();
        }

        private void OnApplicationQuit()
        {
            for (int i = 0; i < images.Count; i++)
                ClearTextures((Texture2D)images[i].mainTexture);
        }

        /// <summary>
        /// loads the next dataset when the previous ones are solved.
        /// </summary>
        public void LoadDataSet()
        {
            if(!CurrentBin.IsDataBinSolved())
            {
                CurrentSet = CurrentBin.FirstUnsolvedDataSet();
            }
        }

        //TODO: Remove datastub 
        private DataBin LoadDataBin()
        {
            DataStubBin Stub = new DataStubBin();
            return Stub.Bin;
            
        }
        #endregion

    }
}
