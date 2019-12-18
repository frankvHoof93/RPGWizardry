using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.ResearchData
{
    public class DataBin
    {
        #region Methods
        #region Public
        /// <summary>
        /// List of all fragments in supplied Research Bin
        /// </summary>
        public List<Fragment> Bin { get; private set; }
        #endregion
        #region Private
        /// <summary>
        /// Datasets after splitting the bin into managable pieces
        /// </summary>
        private List<DataSet> DataSets;

        /// <summary>
        /// 
        /// </summary>
        private bool IsSolved;
        /// <summary>
        /// Definition of how bin will be divided into DataSets
        /// </summary>
        private int setSize;
        #endregion

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
        public DataSet UnsolvedDataSet()
        {
            for (int i = 0; i < this.setSize; i++)
            {
                if (!DataSets[i].IsSolved)
                    return DataSets[i];
            }
            return null;
        }
        #endregion
        #region Split
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
    #region Utility
    /// <summary>
    /// Helper methods for the lists.
    /// </summary>
    public static class ListExtensions
    {
        public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
    #endregion
}
