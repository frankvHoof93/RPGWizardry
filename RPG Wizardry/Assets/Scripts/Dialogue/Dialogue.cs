using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Dialogue
{
    public string name;

    public Sprite characterSprite;

    [TextArea(3, 10)]
    public string[] sentences;
}
