using nl.SWEG.RPGWizardry.Player.Inventory;
using nl.SWEG.RPGWizardry.Sorcery;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace nl.SWEG.RPGWizardry.ResearchData
{
    public class DataManager : MonoBehaviour
    {
        public PlayerInventory Inventory;
        public DataBin CurrentBin;
        private DataSet CurrentSet;
        private int setSize;
        [SerializeField]
        private List<Image> images;
        [SerializeField]
        private RenderTexture texture;
        // Start is called before the first frame update
        void Start()
        {
            CurrentBin = LoadDataBin();
            PopulateUI();
        }

        // Update is called once per frame
        void Update()
        {
            if(CurrentSet.CheckDataSolved())
            {
                Debug.Log("Finished");
            }
        }

        public void PopulateUI()
        {
            LoadDataSet();
            for (int i = 0; i < CurrentSet.Fragments.Count; i++)
            {
                CurrentSet.Fragments[i].FragmentImage = images[i];
                CurrentSet.Fragments[i].ImageTransform = images[i].transform;
                Texture2D imgTex = (Texture2D)CurrentSet.Fragments[i].FragmentImage.mainTexture;
                ClearTexture(imgTex);
                for (int j = 0; j < CurrentSet.Fragments[i].ImgData.Length - 1; j++)
                {
                    // Add Pixel to Img
                    
                    imgTex.SetPixel(UnityEngine.Random.Range(0, imgTex.width), (int)(CurrentSet.Fragments[i].ImgData[j] * imgTex.height), Color.black);
                }
                imgTex.Apply();


            }

        }

        private void ClearTexture(Texture2D tex)
        {
            for (int x = 0; x < tex.width; x++)
                for (int y = 0; y < tex.height; y++)
                    tex.SetPixel(x, y, Color.white);
            tex.Apply();
        }

        public void LoadDataSet()
        {
            if(!CurrentBin.IsDataBinSolved())
            {
                CurrentSet = CurrentBin.UnsolvedDataSet();
            }
        }

        private DataBin LoadDataBin()
        {
            DataStubBin Stub = new DataStubBin();
            return Stub.Bin;
            
        }

        private bool UnlockSpell(SpellPage spell)
        {
            return Inventory.UnlockSpell(spell);
        }

    }
}
