using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class B_DialogueData
{
    public string speakerName;
    public Sprite speakerSprite;
    [TextArea(2, 4)]
    public string dialogueText;
    public bool isLeftSpeaker;
}
