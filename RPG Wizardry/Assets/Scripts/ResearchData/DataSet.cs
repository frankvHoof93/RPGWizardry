using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace nl.SWEG.RPGWizardry.ResearchData
{
    public class DataSet
    {
        #region Methods
        #region Public
        /// <summary>
        /// List of fragments for this dataset
        /// </summary>
        public List<Fragment> Fragments { get; set; }

        public bool IsSolved { get; set; }
        #endregion
        //TODO: Add automatic generation of Control Fragments
        #endregion
        #region Check
        /// <summary>
        /// Checks if all the control fragments in the fragment list are solved or not.
        /// </summary>
        /// <returns>True or False based on solution.</returns>
        public bool CheckDataSolved()
        {
            int SolvedCount = 0;
            foreach (ControlFragment control in Fragments.OfType<ControlFragment>())
            {
                control.CheckIfSolved();
                if (control.Solved)
                {
                    SolvedCount++;
                }
                    
            }
            if(Fragments.OfType<ControlFragment>().Count() == SolvedCount)
            {
                return true;
            }
            SolvedCount = 0;
            return false;
        }
        #endregion
    }
}
