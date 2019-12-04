using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace nl.SWEG.RPGWizardry.ResearchData
{

    public class Chromosome
    {

        public Image image;
        public bool HasMoved { get; private set; }  

        public float[] imgData { get; set; }

        public int OriginalRow { get; set; }
        public Transform transform { get; set; }

        private int OriginalPositionInDataSet;

        private float ImagePosition;
        public Chromosome(Image image, Transform transform)
        {
            this.image = image;
            this.transform = transform;
        }

        
        public virtual void CheckIfSolved()
        {
            
        }
    }
}
