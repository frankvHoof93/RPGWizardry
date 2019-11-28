using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace nl.SWEG.RPGWizardry.ResearchData
{
    public class DataSet
    {
        
        public List<Chromosome> Chromosomes { get; set; }
        private int SolvedCount;


        public bool CheckDataSolved()
        {
            foreach (ControlChromosome control in Chromosomes.OfType<ControlChromosome>())
            {
                control.CheckIfSolved();
                if (control.Solved)
                {
                    SolvedCount++;
                }
                    
            }
            if(ControlChromosomeCount() == SolvedCount)
            {
                return true;
            }
            return false;
        }

        private int ControlChromosomeCount()
        {
           return Chromosomes.OfType<ControlChromosome>().Count();
        }
    }
}
