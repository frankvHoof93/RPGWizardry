using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class DataLoader : MonoBehaviour
{
    [SerializeField]
    private List<Image> images;

    private readonly float[,] data = new float[,]
    {
        { 0.41f, 0.375f, 0.35f, 0.475f, 0.46f, 0.5f, 0.38f, 0.41f, 0.51f, 0.395f, 0.415f },
        { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f },
        { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f },
        { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f },
        { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f },
        { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f },
        { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f },
        { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f },
        { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f },
        { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f },
        { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f },
        { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f },
        { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f },
        { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f },
        { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f }
    };

    private void Start()
    {
        if (data.GetLength(0) != images.Count)
        {
            Debug.LogError("Invalid Format");
            return;
        }

        // Loop across chromosomes
        for (int i = 0; i < data.GetLength(0); i++)
        {
            Image img = images[i];
            float[] imgData = data.GetRow(i);
            
            Texture2D imgTex = (Texture2D)img.mainTexture;
            ClearTexture(imgTex);

            // Loop across data for chromosome
            for (int j = 0; j < imgData.Length; j++)
            {
                // Add Pixel to Img
                imgTex.SetPixel(UnityEngine.Random.Range(0, imgTex.width), (int)(imgData[j] * imgTex.height), Color.black);
            }
            imgTex.Apply();
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
