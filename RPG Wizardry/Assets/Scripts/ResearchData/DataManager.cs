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
            for (int i = 0; i < CurrentSet.Fragments.Count; i++)
            {
                CurrentSet.Fragments[i].FragmentImage = images[i];
                CurrentSet.Fragments[i].ImageTransform = images[i].transform.parent;
                Texture2D imgTex = (Texture2D)CurrentSet.Fragments[i].FragmentImage.mainTexture;
                ClearTextures(imgTex);
                for (int j = 0; j < CurrentSet.Fragments[i].ImgData.Length - 1; j++)
                {
                    // Add Pixel to Img
                    
                    imgTex.SetPixel(UnityEngine.Random.Range(0, imgTex.width), (int)(CurrentSet.Fragments[i].ImgData[j] * imgTex.height), Color.magenta);
                }
                imgTex.Apply();


            }

        }


        /// <summary>
        /// Cleares the images and reset it to their base state.
        /// </summary>
        /// <param name="tex">Target texture</param>
        private void ClearTextures(Texture2D tex)
        {
            for (int x = 0; x < tex.width; x++)
                for (int y = 0; y < tex.height; y++)
                    tex.SetPixel(x, y, new Color(0,0,0,0));
            tex.Apply();
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
