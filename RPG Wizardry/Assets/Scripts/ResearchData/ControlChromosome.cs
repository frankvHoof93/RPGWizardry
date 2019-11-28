using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace nl.SWEG.RPGWizardry.ResearchData
{

    public class ControlChromosome : Chromosome
    {

        public float CorrectPosition { get; set; }
        private float range;

        public ControlChromosome(Image image, Transform transform, float correctPosition, float range) : base(image, transform)
        {
            CorrectPosition = correctPosition;
            Solved = false;
            this.range = range;
        }

        public bool Solved { get; private set; }
        

        public override void CheckIfSolved()
        {
            if(transform.localPosition.y <= CorrectPosition + 10 && transform.localPosition.y >= CorrectPosition - 10)
            {
                Solved = true;
            }
        }   
    }
}
