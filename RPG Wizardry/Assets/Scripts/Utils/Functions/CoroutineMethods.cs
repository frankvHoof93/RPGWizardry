using System;
using System.Collections;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Utils.Functions
{
    public static class CoroutineMethods
    {
        public static IEnumerator RunDelayed(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }

        public static IEnumerator RunDelayed<T>(Action<T> action, T param, float delay)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke(param);
        }
    }
}
