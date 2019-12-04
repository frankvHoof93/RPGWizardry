namespace nl.SWEG.RPGWizardry.Utils.Functions
{
    public static class MathFunctions
    {
        /// <summary>
        /// Wraps value around bounds
        /// </summary>
        /// <param name="input">Input to Wrap</param>
        /// <param name="minimum">INCLUSIVE minimum for Wrapping</param>
        /// <param name="maximum">EXCLUSIVE maximum for Wrapping</param>
        /// <returns>Wrapped Value</returns>
        public static int Wrap(int input, int minimum, int maximum)
        {
            return (((input - minimum) % (maximum - minimum)) + 
                (maximum - minimum)) % (maximum - minimum) + minimum;
        }

        /// <summary>
        /// Wraps value around bounds
        /// </summary>
        /// <param name="input">Input to Wrap</param>
        /// <param name="minimum">INCLUSIVE minimum for Wrapping</param>
        /// <param name="maximum">EXCLUSIVE maximum for Wrapping</param>
        /// <returns>Wrapped Value</returns>
        public static decimal Wrap(decimal input, decimal minimum, decimal maximum)
        {
            return (((input - minimum) % (maximum - minimum)) +
                (maximum - minimum)) % (maximum - minimum) + minimum;
        }
    }
}
