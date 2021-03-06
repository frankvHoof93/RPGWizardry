﻿using UnityEngine;

namespace nl.SWEG.Willow.Utils.Behaviours
{
    /// <summary>
    /// Destroys Object (For usage with Unity Animator)
    /// </summary>
    public class DestroySelf : MonoBehaviour
    {
        /// <summary>
        /// Destroys this object. Called from Animation-Event
        /// </summary>
        public void OnAnimationEnd()
        {
            Destroy(gameObject);
        }
    }
}