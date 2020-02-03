namespace nl.SWEG.Willow.Research.Data
{
    /// <summary>
    /// Fragment used as control, so that a check can be performed as to whether a DataSet has been properly researched
    /// </summary>
    public class ControlFragment : Fragment
    {
        #region Variables
        /// <summary>
        /// The position that is the solution to this ControlFragment
        /// </summary>
        public readonly float CorrectPosition;
        /// <summary>
        /// Used to determine if this fragment is solved or not
        /// </summary>
        public bool Solved => ImageTransform != null && ImageTransform.localPosition.y <= CorrectPosition + range && ImageTransform.localPosition.y >= CorrectPosition - range;
        /// <summary>
        /// the offset from the correct position which will count as a valid solution
        /// </summary> 
        private readonly float range;
        #endregion

        #region Methods
        /// <summary>
        /// Constructor for a ControlFragment
        /// </summary>
        /// <param name="correctPosition">Correct Y-Position for Fragment</param>
        /// <param name="range">Range from <paramref name="correctPosition"/> that counts as Solved</param>
        public ControlFragment(float correctPosition, float range, float[] data) : base(data)
        {
            CorrectPosition = correctPosition;
            this.range = range;
        }
        #endregion
    }
}