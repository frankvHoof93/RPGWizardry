using nl.SWEG.Willow.Utils.Functions;
using UnityEngine;
using UnityEngine.UI;

namespace nl.SWEG.Willow.Research.Data
{
    /// <summary>
    /// Research-Measurements for a single Bar in the MiniGame
    /// </summary>
    public class Fragment
    {
        #region Variables
        #region Data
        /// <summary>
        /// Texture data for the image
        /// </summary>
        public readonly float[] ImgData;
        /// <summary>
        /// Index of Data before Shuffling (Shuffling occurs in Constructor)
        /// </summary>
        public readonly int[] OriginalIndices;
        /// <summary>
        /// Original position of the fragment in the fragment bin.
        /// <para>
        /// By using this index, we can randomize the order in which Fragments are displayed to the user, but preserve the result for research
        /// </para>
        /// <para>
        /// This index is set to -1 for ControlFragments (or before it has been set)
        /// </para>
        /// </summary>
        public int OriginalRow { get; internal set; } = -1;
        #endregion

        #region UI
        /// <summary>
        /// UI-Image for Fragment
        /// </summary>
        public Image FragmentImage { get; private set; }
        /// <summary>
        /// UI Transform object needed to get the position.
        /// </summary>
        public Transform ImageTransform { get; private set; }
        #endregion
        #endregion

        #region Methods
        /// <summary>
        /// Constructor for Fragment
        /// </summary>
        /// <param name="data">Data for Fragment</param>
        public Fragment(float[] data)
        {
            OriginalIndices = data.Shuffle();
            ImgData = data;
        }
        /// <summary>
        /// Sets UI-Image for Fragment
        /// </summary>
        /// <param name="img">Image to Set</param>
        public void SetImage(Image img)
        {
            FragmentImage = img;
            ImageTransform = img.transform;
        }
        #endregion
    }
}