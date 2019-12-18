using nl.SWEG.RPGWizardry.Utils;
using System.Collections.Generic;

namespace nl.SWEG.RPGWizardry.ResearchData
{
    public class DataBin
    {
        #region Variables
        /// <summary>
        /// List of all fragments in supplied Research Bin
        /// </summary>
        public List<Fragment> Bin { get; private set; }
        /// <summary>
        /// Datasets after splitting the bin into managable pieces
        /// </summary>
        private List<DataSet> DataSets;
        /// <summary>
        /// Flags the databin is its solved or not.
        /// </summary>
        private bool IsSolved;
        /// <summary>
        /// Definition of how bin will be divided into DataSets
        /// </summary>
        private int setSize;
        #endregion
        #region Methods
        public DataBin(List<Fragment> bin, int setSize)
        {
            Bin = bin;
            this.setSize = setSize;
            DataSets = new List<DataSet>();
            for (int i = 0; i < this.setSize; i++)
            {
                DataSets.Add(new DataSet());
            }

            SplitBin();
            for (int i = 0; i < this.setSize; i++)
            {
                DataSets[i].GenerateControlFragments();
            }
        }

        /// <summary>
        /// Checks if the sets in this databin are solved
        /// </summary>
        /// <returns>A true or false based on the outcome</returns>
        public bool IsDataBinSolved()
        {
            int solvedCount = 0;
            for (int i = 0; i < this.setSize; i++)
            {
                if (DataSets[i].IsSolved)
                    solvedCount++;
            }
            return solvedCount == setSize;
        }
        /// <summary>
        /// Returns the first unsolved Dataset
        /// </summary>
        /// <returns>A Dataset</returns>
        public DataSet FirstUnsolvedDataSet()
        {
            for (int i = 0; i < this.setSize; i++)
            {
                if (!DataSets[i].IsSolved)
                    return DataSets[i];
            }
            return null;
        }
        /// <summary>
        /// Converts the bin into the Datasets
        /// </summary>
        private void SplitBin()
        {
            int batchsize = Bin.Count / setSize;
            List<List<Fragment>> FragmentSets = Bin.ChunkBy<Fragment>(batchsize);
            for (int i = 0; i < DataSets.Count; i++)
            {
                DataSets[i].Fragments = FragmentSets[i];
            }

        }
        #endregion
    }
}
