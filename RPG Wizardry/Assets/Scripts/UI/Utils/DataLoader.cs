using nl.SWEG.RPGWizardry.ResearchData;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace nl.SWEG.RPGWizardry.Utils
{
    /// <summary>
    /// Loads Data for SpellCrafting-Mechanic (DEBUG)
    /// </summary>
    public class DataLoader : MonoBehaviour
    {

        [SerializeField]
        private List<Image> images;

        private DataSet dataSet;

        private readonly float[,] data = new float[,]
        {
        { 0.410f, 0.375f, 0.350f, 0.475f, 0.460f, 0.500f, 0.380f, 0.410f, 0.510f, 0.395f, 0.415f },
        { 0.125f, 0.110f, 0.132f, 0.121f, 0.138f, 0.128f, 0.124f, 0.122f, 0.128f, 0.131f, 0.118f },
        { 0.495f, 0.448f, 0.524f, 0.430f, 0.580f, 0.690f, 0.468f, 0.438f, 0.500f, 0.465f, 0.512f },
        { 0.885f, 0.832f, 0.790f, 0.910f, 0.875f, 0.842f, 0.895f, 0.731f, 0.689f, 0.920f, 0.925f },
        { 0.635f, 0.645f, 0.660f, 0.612f, 0.700f, 0.580f, 0.665f, 0.600f, 0.689f, 0.800f, 0.635f },
        { 0.734f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f },
        { 0.409f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f },
        { 0.227f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f },
        { 0.220f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f },
        { 0.837f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f },
        { 0.678f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f },
        { 0.085f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f },
        { 0.129f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f },
        { 0.854f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f },
        { 0.795f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f }
        };

        private void Start()
        {
            dataSet = new DataSet();
            dataSet.Chromosomes = new List<Chromosome>();
            if (data.GetLength(0) != images.Count)
            {
                Debug.LogError("Invalid Format");
                return;
            }

            // Loop across chromosomes
            for (int i = 0; i < data.GetLength(0) -1; i++)
            {
                Chromosome chromosome = new Chromosome(images[i], images[i].transform);
                
                chromosome.imgData = data.GetRow(i);

                Texture2D imgTex = (Texture2D)chromosome.image.mainTexture;
                ClearTexture(imgTex);

                // Loop across data for chromosome
                for (int j = 0; j < chromosome.imgData.Length -1; j++)
                {
                    // Add Pixel to Img
                    imgTex.SetPixel(UnityEngine.Random.Range(0, imgTex.width), (int)(chromosome.imgData[j] * imgTex.height), Color.black);
                }
                imgTex.Apply();
                dataSet.Chromosomes.Add(chromosome);    
            }
            ControlChromosome control = new ControlChromosome(images[14], images[14].transform, -20, 10);
            dataSet.Chromosomes.Add(control);
        }

        private void Update()
        {
            if(dataSet.CheckDataSolved())
            {
                Debug.Log("Yo you won");
            }
        }


        private void OnApplicationQuit()
        {
            for (int i = 0; i < images.Count; i++)
                ClearTexture((Texture2D)images[i].mainTexture);
        }

        private void ClearTexture(Texture2D tex)
        {
            for (int x = 0; x < tex.width; x++)
                for (int y = 0; y < tex.height; y++)
                    tex.SetPixel(x, y, Color.white);
            tex.Apply();
        }
    }
};