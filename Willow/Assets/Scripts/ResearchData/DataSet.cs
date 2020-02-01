using System.Collections.Generic;
using System.Linq;

namespace nl.SWEG.Willow.ResearchData
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
            ControlFragment start = new ControlFragment(40, 15);
            start.ImgData = new float[] { 0.150f, 0.175f, 0.150f, 0.175f, 0.160f, 0.120f, 0.180f, 0.125f, 0.130f, 0.195f, 0.145f };
            ControlFragment middle = new ControlFragment(20, 15);
            middle.ImgData = new float[] { 0.180f, 0.175f, 0.190f, 0.175f, 0.210f, 0.220f, 0.180f, 0.210f, 0.210f, 0.185f, 0.115f };
            ControlFragment end = new ControlFragment(-30, 15);
            end.ImgData = new float[] { 0.610f, 0.675f, 0.650f, 0.675f, 0.660f, 0.600f, 0.680f, 0.610f, 0.610f, 0.695f, 0.615f };
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
    