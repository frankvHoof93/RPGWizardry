using nl.SWEG.Willow.Research.Data;
using nl.SWEG.Willow.Utils.Functions;
using System.Collections.Generic;

namespace nl.SWEG.Willow.Research.IO
{
    /// <summary>
    /// A set of ResearchData, as received from Researchers
    /// </summary>
    public class DataBin
    {
        #region Variables
        #region Public
        /// <summary>
        /// List of all fragments in supplied Research Bin
        /// </summary>
        public IReadOnlyList<Fragment> Bin => bin.AsReadOnly();
        #endregion

        #region Private
        /// <summary>
        /// List of all fragments in supplied Research Bin
        /// </summary>
        private readonly List<Fragment> bin;
        /// <summary>
        /// DataSets after splitting the bin into manageable pieces
        /// </summary>
        private readonly List<DataSet> dataSets;
        /// <summary>
        /// Number of DataSets in Bin
        /// </summary>
        private readonly int numberOfSets;
        #endregion
        #endregion

        #region Methods
        /// <summary>
        /// Constructor for DataBin
        /// </summary>
        /// <param name="bin">Fragments in Bin</param>
        /// <param name="numberOfSets">Number of DataSets in Bin</param>
        public DataBin(List<Fragment> bin, int numberOfSets)
        {
            this.bin = bin;
            this.numberOfSets = numberOfSets;
            dataSets = new List<DataSet>();
            List<List<Fragment>> sets = SplitBin();
            List<List<Fragment>> shuffledSets = new List<List<Fragment>>(sets); // Copy List
            shuffledSets.Shuffle(); // Shuffle List
            for (int i = 0; i < this.numberOfSets; i++)
                dataSets.Add(new DataSet(shuffledSets[i], sets.IndexOf(shuffledSets[i]))); // Create DataSets
        }

        /// <summary>
        /// Whether the Sets in this DataBin are solved
        /// </summary>
        /// <returns>True if all Sets have been solved</returns>
        public bool IsDataBinSolved()
        {
            for (int i = 0; i < this.numberOfSets; i++)
                if (!dataSets[i].CheckDataSolved())
                    return false;
            return true;
        }

        /// <summary>
        /// Returns the first unsolved DataSet
        /// </summary>
        /// <returns>First unsolved DataSet, or null if none can be found</returns>
        public DataSet FirstUnsolvedDataSet()
        {
            for (int i = 0; i < this.numberOfSets; i++)
                if (!dataSets[i].CheckDataSolved())
                    return dataSets[i];
            return null;
        }

        /// <summary>
        /// Converts the bin into the Datasets
        /// </summary>
        /// <returns>List of Sets</returns>
        private List<List<Fragment>> SplitBin()
        {
            int batchSize = bin.Count / numberOfSets;
            return bin.ChunkBy(batchSize);
        }
        #endregion
    }
}
