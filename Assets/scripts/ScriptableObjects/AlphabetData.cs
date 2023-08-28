using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class AlphabetData : ScriptableObject
{

    [System.Serializable]
    public class LetterData
    {
        public string letter;
        public Sprite Image;
    }

    public List<LetterData> alphabetPlain = new List<LetterData>();
    public List<LetterData> alphabetNormale = new List<LetterData>();
    public List<LetterData> alphabetHighlited = new List<LetterData>();
    public List<LetterData> alphabetCorrect = new List<LetterData>();


}
