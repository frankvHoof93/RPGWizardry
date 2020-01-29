using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.UI.Dialogue
{
    public struct Dialogue
    {
        public string name;

        public Sprite characterSprite;

        [TextArea(3, 10)]
        public string[] sentences;
    }

}