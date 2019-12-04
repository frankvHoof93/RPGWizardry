using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace  nl.SWEG.RPGWizardry.ResearchData
{
    public class DataBin
    {
        private List<Chromosome> Bin;
        private List<DataSet> DataSets;
        private int SetSize;

        public DataBin()
        {
            DataSets = new List<DataSet>();
            for (int i= 0; i < SetSize; i++)
            {
                DataSets.Add(new DataSet());
            }
        }

        private void SplitBin()
        {
            int batchsize = Bin.Count / SetSize;
            List<List<Chromosome>> ChromosomeSets = Bin.ChunkBy<Chromosome>(batchsize);
            for (int i = 0; i < DataSets.Count; i++)
            {
                DataSets[i].Chromosomes = ChromosomeSets[i];
            }

        }

    }
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
}
