using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "ScriptableObjects/Dialogue", order = 1)]
public class DialogueData : ScriptableObject
{
    /// <summary>
    /// Name of the character speaking the dialogue
    /// </summary>
    public string Name => characterName;
    [SerializeField]
    [Tooltip("Name of the character speaking the dialogue")]
    private string characterName;

    /// <summary>
    /// Sprite of the character speaking the dialogue
    /// Leave empty to show text without portrait
    /// </summary>
    public Sprite Sprite => characterSprite;
    [SerializeField]
    [Tooltip("Portrait of the character. Leave empty to just show text")]
    private Sprite characterSprite;

    /// <summary>
    /// List of dialogue string
    /// </summary>
    public string[] Sentences => sentenceStrings;
    [SerializeField]
    [Tooltip("Array of dialogue strings, in order displayed")]
    [TextArea(3, 10)]
    private string[] sentenceStrings;
}