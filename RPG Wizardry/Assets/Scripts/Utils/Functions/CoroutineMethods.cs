using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Utils.Functions
{
    public static class CoroutineMethods
    {
        public static IEnumerator RunDelayed(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }
    }
}
