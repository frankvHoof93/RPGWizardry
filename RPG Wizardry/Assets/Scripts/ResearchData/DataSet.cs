﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace nl.SWEG.RPGWizardry.ResearchData
{
    public class DataSet
    {
        #region Variables
        /// <summary>
        /// List of fragments for this dataset
        /// </summary>
        public List<Fragment> Fragments;
 
        public bool IsSolved { get;  private set; }
        #endregion
        //TODO: Add automatic generation of Control Fragments
        #region Methods
        #region Control
        public void GenerateControlFragments()
        {
            ControlFragment start = new ControlFragment(20, 10);
            start.ImgData = new float[] { 1.410f, 1.375f, 1.350f, 1.475f, 0.460f, 0.500f, 0.380f, 0.410f, 0.510f, 0.395f, 0.415f };
            ControlFragment middle = new ControlFragment(-20, 10);
            middle.ImgData = new float[] { 0.410f, 0.375f, 0.350f, 0.475f, 0.460f, 0.500f, 0.380f, 0.410f, 0.510f, 0.395f, 0.415f };
            ControlFragment end = new ControlFragment(-10, 10);
            end.ImgData = new float[] { 0.410f, 0.375f, 0.350f, 0.475f, 0.460f, 0.500f, 0.380f, 0.410f, 0.510f, 0.395f, 0.415f };
            int mid = (Fragments.Count + 3) / 2;
            Fragments.Insert(0, start);
            Fragments.Insert(mid, middle);
            Fragments.Add(end);

        }
        #endregion
        #region Check
        /// <summary>
        /// Checks if all the control fragments in the fragment list are solved or not.
        /// </summary>
        /// <returns>True or False based on solution.</returns>
        public bool CheckDataSolved()
        {
            List<ControlFragment> controlFragments = Fragments.OfType<ControlFragment>().ToList();
            int SolvedCount = 0;
            foreach (ControlFragment control in controlFragments)
            {
                control.CheckIfSolved();
                if (control.Solved)
                {
                    SolvedCount++;
                }
                    
            }
            if(controlFragments.Count() == SolvedCount)
            {
                return true;
            }
            SolvedCount = 0;
            return false;
        }
        #endregion
        #endregion
    }
}
    