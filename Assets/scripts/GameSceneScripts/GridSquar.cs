using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSquar : MonoBehaviour
{
    public int squareIndex { get; set; }

    private AlphabetData.LetterData _normaleLetterData;
    private AlphabetData.LetterData _selectedLetterData;
    private AlphabetData.LetterData _correctLetterData;


    private SpriteRenderer _displayedImage;


    private bool _selected;
    private bool _clicked;
    private int _index = -1;
    private bool _correct;


    public void set_index(int index)
    {
        _index = index;
    }

    public int get_index()
    {
        return _index;
    }

    void Start()
    {
        _selected = false;
        _clicked = false;
        _correct = false;
        _displayedImage = GetComponent<SpriteRenderer>();

    }

    private void OnEnable()
    {
        GameEvents.OnEnableSquareSelection += OnEnableSquareSelection;
        GameEvents.OnDisableSquareSelection += OnDisableSquareSelection;
        GameEvents.OnSelectSquare += SelectSquare;
        GameEvents.oncorrectWord += correctWord;
    }

    private void OnDisable()
    {
        GameEvents.OnEnableSquareSelection -= OnEnableSquareSelection;
        GameEvents.OnDisableSquareSelection -= OnDisableSquareSelection;
        GameEvents.OnSelectSquare -= SelectSquare;
        GameEvents.oncorrectWord -= correctWord;

    }

    private void correctWord(string word, List<int> squareIndexes)
    {
        if (_selected && squareIndexes.Contains(_index))
        {
            _correct = true;
            _displayedImage.sprite = _correctLetterData.Image;
        }
        _selected = false;
        _clicked = false;
    }

    public void OnEnableSquareSelection()
    {
        _clicked = true;
        _selected = false;

    }
    public void OnDisableSquareSelection()
    {
        _clicked = false;
        _selected = false;

        if (_correct == true)
            _displayedImage.sprite = _correctLetterData.Image;
        else
            _displayedImage.sprite = _normaleLetterData.Image;

    }
    public void SelectSquare(Vector3 position)
    {
        if (this.gameObject.transform.position == position)
        {
            _displayedImage.sprite = _selectedLetterData.Image;
        }
    }




    public void setSprite(AlphabetData.LetterData normaleLetterData, AlphabetData.LetterData selectedLettterData,
    AlphabetData.LetterData correctLetterData)
    {
        _normaleLetterData = normaleLetterData;
        _selectedLetterData = selectedLettterData;
        _correctLetterData = correctLetterData;

        GetComponent<SpriteRenderer>().sprite = _normaleLetterData.Image;
    }

    private void OnMouseDown()
    {
        OnEnableSquareSelection();
        GameEvents.EnableSquareSelectionMethod();

        checkSquare();
        _displayedImage.sprite = _selectedLetterData.Image;

    }

    private void OnMouseEnter()
    {
        checkSquare();

    }
    private void OnMouseUp()
    {
        GameEvents.ClearSelectionMethod();
        GameEvents.DisableSquareSelectionMethod();
    }

    public void checkSquare()
    {
        if (_selected == false && _clicked == true)
        {
            _selected = true;
            GameEvents.CheckSquareMethod(_normaleLetterData.letter, gameObject.transform.position, _index);
        }
    }

}
