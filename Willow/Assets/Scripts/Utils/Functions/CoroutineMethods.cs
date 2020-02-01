using System;
using System.Collections;
using UnityEngine;

namespace nl.SWEG.Willow.Utils.Functions
{
    /// <summary>
    /// Utility-Methods for Unity Coroutines
    /// </summary>
    public static class CoroutineMethods
    {
        /// <summary>
        /// Runs Action after Delay
        /// </summary>
        /// <param name="action">Action to perform</param>
        /// <param name="delay">Delay (in seconds) before Action. Set 0 to delay a single frame</param>
        public static IEnumerator RunDelayed(Action action, float delay)
        {
            if (delay == 0)
                yield return null;
            else
                yield return new WaitForSeconds(delay);
            action.Invoke();
        }
        /// <summary>
        /// Runs Action after Delay
        /// </summary>
        /// <param name="action">Action to perform</param>
        /// <param name="param">Parameter for Action</param>
        /// <param name="delay">Delay (in seconds) before Action. Set 0 to delay a single frame</param>
        public static IEnumerator RunDelayed<T>(Action<T> action, T param, float delay)
        {
            if (delay == 0)
                yield return null;
            else
                yield return new WaitForSeconds(delay);
            yield return new WaitForSeconds(delay);
            action.Invoke(param);
        }
    }
}
