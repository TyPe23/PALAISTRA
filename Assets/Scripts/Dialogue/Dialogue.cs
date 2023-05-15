using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string dialogueName;

    [TextArea(3,10)]
    public string[] sentences;
}
