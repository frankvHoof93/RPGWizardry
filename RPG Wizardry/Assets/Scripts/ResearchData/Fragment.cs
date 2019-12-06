using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace nl.SWEG.RPGWizardry.ResearchData
{

    public class Fragment
    {
        #region Methods
        #region Public
        /// <summary>
        /// 
        /// </summary>
        public Image FragmentImage { get; set; }

        /// <summary>
        /// Texture data for the image
        /// </summary>
        public float[] ImgData { get; set; }

        /// <summary>
        /// Original position of the fragment in the fragment bin.
        /// </summary>
        public int OriginalRow { get; set; }
        /// <summary>
        /// UI Transform object needed to get the position.
        /// </summary>
        public Transform ImageTransform { get; set; }
        #endregion

        //public Fragment(Image image, Transform transform)
        //{
        //    this.FragmentImage = image;
        //    this.ImageTransform = transform;
        //}

        public Fragment()
        {
        }
        #endregion

        #region Check
        public virtual void CheckIfSolved()
        {
            
        }
        #endregion
    }
}
