﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace nl.SWEG.RPGWizardry.ResearchData
{

    public class ControlFragment : Fragment
    {
        #region Methods
        #region Public
        /// <summary>
        /// The position which is the solution to this control fragment.
        /// </summary>
        public float CorrectPosition { get; set; }
        /// <summary>
        /// Used to determine if this fragment is solved or not.
        /// </summary>
        public bool Solved { get; private set; }
        /// <summary>
        /// the offset from the correct position which will count as a valid solution
        /// </summary>
        #endregion
        #region Private
        private float range;
        #endregion
        public ControlFragment(float correctPosition, float range) : base()
        {
            CorrectPosition = correctPosition;
            Solved = false;
            this.range = range;
        }
        #endregion
        #region Check
        /// <summary>
        /// Checks the current position of the control fragment versus the correct position + offsets to see if this fragment has been solved.
        /// </summary>
        public override void CheckIfSolved()
        {
            if(ImageTransform.localPosition.y <= CorrectPosition + 10 && ImageTransform.localPosition.y >= CorrectPosition - 10)
            {
                Solved = true;
            }
        }
        #endregion
    }
}
