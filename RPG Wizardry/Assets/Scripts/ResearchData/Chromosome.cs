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

        private float oldposition;

        public float[] imgData { get; set; }

        public Transform transform { get; set; }

        public Chromosome(Image image, Transform transform)
        {
            this.image = image;
            this.transform = transform;
        }



        public bool CheckPosition()
        {
            if (transform.localPosition.y != oldposition)
            {
                return true;
            }
            else
                return false;
        }
        
        public virtual void CheckIfSolved()
        {
            
        }
    }
}
