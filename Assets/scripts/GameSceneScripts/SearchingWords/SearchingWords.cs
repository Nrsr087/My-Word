using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchingWords : MonoBehaviour
{
    public Text displayText;
    public Image CrossLine;

    private string _word;


    void Start()
    {

    }

    private void OnEnable()
    {
        GameEvents.oncorrectWord += correctWord;

    }
    private void OnDisable()
    {
        GameEvents.oncorrectWord -= correctWord;

    }

    public void setWord(string word)
    {
        _word = word;
        displayText.text = word;
    }

    private void correctWord(string word, List<int> squareIndexes)
    {
        if (word == _word)
        {
            CrossLine.gameObject.SetActive(true);
        }
    }

}
