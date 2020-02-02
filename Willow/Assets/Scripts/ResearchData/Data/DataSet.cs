using nl.SWEG.Willow.Utils;
using System.Collections.Generic;
using System.Linq;

namespace nl.SWEG.Willow.Research.Data
{
    /// <summary>
    /// A set of Data as used in the MiniGame
    /// <para>
    /// This is a subset of a DataBin, with just enough data for a single attempt at the MiniGame
    /// </para>
    /// </summary>
    public class DataSet
    {
        #region Variables
        /// <summary>
        /// ReadOnly-List of Fragments in this DataSet
        /// </summary>
        public IReadOnlyList<Fragment> Fragments => fragments.AsReadOnly();
        /// <summary>
        /// Index for Set in DataBin
        /// <para>
        /// By using this index, we can randomize the order in which DataSets are displayed to the user, but preserve the result for research
        /// </para>
        /// </summary>
        public readonly int OriginalIndex;
        /// <summary>
        /// List of fragments for this dataset
        /// </summary>
        private readonly List<Fragment> fragments;
        #endregion

        #region Methods
        /// <summary>
        /// Constructor for a DataSet
        /// </summary>
        /// <param name="fragments">Fragments in Set</param>
        /// <param name="origIndex">Index of Set in DataBin</param>
        public DataSet(List<Fragment> fragments, int origIndex)
        {
            this.fragments = fragments;
            OriginalIndex = origIndex;
            List<ControlFragment> controls = GenerateControlFragments();
            ShuffleFragments(controls);
        }

        /// <summary>
        /// Whether all the ControlFragments in the set have been solved
        /// </summary>
        /// <returns>True if all ControlFragments have been solved</returns>
        public bool CheckDataSolved()
        {
            IEnumerable<ControlFragment> controlFragments = fragments.OfType<ControlFragment>();
            foreach (ControlFragment control in controlFragments)
                if (!control.Solved)
                    return false;
            return true;
        }

        /// <summary>
        /// Generates ControlFragments for this DataSet
        /// TODO: Add more randomized generation of Control Fragments
        /// </summary>
        private List<ControlFragment> GenerateControlFragments()
        {
            return new List<ControlFragment> {
            new ControlFragment(40, 15,
                new float[] { 0.150f, 0.175f, 0.150f, 0.175f, 0.160f, 0.120f, 0.180f, 0.125f, 0.130f, 0.195f, 0.145f }
            ),
            new ControlFragment(20, 15,
                new float[] { 0.180f, 0.175f, 0.190f, 0.175f, 0.210f, 0.220f, 0.180f, 0.210f, 0.210f, 0.185f, 0.115f }
            ),
            new ControlFragment(-30, 15,
                new float[] { 0.610f, 0.675f, 0.650f, 0.675f, 0.660f, 0.600f, 0.680f, 0.610f, 0.610f, 0.695f, 0.615f }
            ) };
        }

        /// <summary>
        /// Shuffles Fragments in Set
        /// </summary>
        /// <param name="controls">ControlFragments to Add</param>
        private void ShuffleFragments(List<ControlFragment> controls)
        {
            List<Fragment> original = new List<Fragment>(fragments);
            fragments.AddRange(controls);
            fragments.Shuffle();
            for (int i = 0; i < fragments.Count; i++)
                fragments[i].OriginalRow = original.IndexOf(fragments[i]);
        }
        #endregion
    }
}