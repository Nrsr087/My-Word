using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchingWordsList : MonoBehaviour
{
    public GameData currentGameData;
    public GameObject searhcingWordPrefab;
    public float Offset = 0.0f;
    public int maxColums = 5;
    public int maxRows = 4;
    private int _colums = 2;
    private int _rows;
    private int _wordsNumber;
    private List<GameObject> _words = new List<GameObject>();

    private void Start()
    {
        _wordsNumber = WordsGrid.instance.currentBoardData.searchwords.Count;
        if (_wordsNumber < _colums)
            _rows = 1;

        else
            calculatoColumsAndRowsNumber();

        creatWordObjects();
        setWordsPosition();
    }

    private void calculatoColumsAndRowsNumber()
    {
        do
        {
            _colums++;
            _rows = _wordsNumber / _colums;

        } while (_rows >= maxRows);

        if (_colums > maxColums)
        {
            _colums = maxColums;
            _rows = _wordsNumber / _colums;
        }
    }


    private bool tryIncreaseColumNumber()
    {
        _colums++;
        _rows = _wordsNumber / _colums;
        if (_colums > maxRows)
        {
            _colums = maxColums;
            _rows = _wordsNumber / _colums;
            return false;
        }

        if (_wordsNumber % _colums > 0)
            _rows++;

        return true;
    }

    private void creatWordObjects()
    {
        var squareScale = getSquareScale(new Vector3(1f, 1f, 0.1f));
        for (var index = 0; index < _wordsNumber; index++)
        {
            _words.Add(Instantiate(searhcingWordPrefab) as GameObject);
            _words[index].transform.SetParent(this.transform);
            _words[index].GetComponent<RectTransform>().localScale = squareScale;
            _words[index].GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
            _words[index].GetComponent<SearchingWords>().setWord(WordsGrid.instance.currentBoardData.searchwords[index].word);


        }
    }

    private Vector3 getSquareScale(Vector3 defaultScale)
    {
        var finalscale = defaultScale;
        var adjustemnt = 0.01f;

        while (shouldScaleDown(finalscale))
        {
            finalscale.x -= adjustemnt;
            finalscale.y -= adjustemnt;

            if (finalscale.x <= 0 || finalscale.y <= 0)
            {
                finalscale.x = adjustemnt;
                finalscale.y = adjustemnt;


                return finalscale;
            }
        }
        return finalscale;
    }

    private bool shouldScaleDown(Vector3 targetScale)
    {
        var squareRect = searhcingWordPrefab.GetComponent<RectTransform>();
        var parrentRect = this.GetComponent<RectTransform>();
        var squareSize = new Vector3(0f, 0f);
        squareSize.x = squareRect.rect.width * targetScale.x + Offset;
        squareSize.y = squareRect.rect.height * targetScale.y + Offset;

        var totalSquareheight = squareSize.y * _rows;

        if (totalSquareheight > parrentRect.rect.height)
        {

            while (totalSquareheight > parrentRect.rect.height)
            {
                if (tryIncreaseColumNumber())
                    totalSquareheight = squareSize.y * _rows;

                else
                    return true;
            }
        }
        var totalsquareWidth = squareSize.x * _colums;
        if (totalsquareWidth > parrentRect.rect.width)
            return true;

        return false;
    }

    private void setWordsPosition()
    {
        var squareRect = _words[0].GetComponent<RectTransform>();
        var wordOffset = new Vector2
        {
            x = squareRect.rect.width * squareRect.transform.localScale.x + Offset,
            y = squareRect.rect.height * squareRect.transform.localScale.y + Offset
        };

        int columNumber = 0;
        int rowsNumber = 0;
        var startPosition = getfirstSquarePosition();

        foreach (var word in _words)
        {

            if (columNumber + 1 > _colums)
            {
                columNumber = 0;
                rowsNumber++;
            }

            var positionX = startPosition.x + wordOffset.x * columNumber;
            var positionY = startPosition.y - wordOffset.y * rowsNumber;

            word.GetComponent<RectTransform>().localPosition = new Vector2(positionX, positionY);
            columNumber++;
        }

    }
    private Vector2 getfirstSquarePosition()
    {
        var startPosition = new Vector2(0f, transform.position.y);
        var squareRect = _words[0].GetComponent<RectTransform>();
        var parrentRect = this.GetComponent<RectTransform>();
        var squareSize = new Vector2(0f, 0f);

        squareSize.x = squareRect.rect.width * squareRect.transform.localScale.x + Offset;
        squareSize.y = squareRect.rect.height * squareRect.transform.localScale.y + Offset;


        var shiftby = (parrentRect.rect.width - (squareSize.x * _colums)) / 2;

        startPosition.x = ((parrentRect.rect.width - squareSize.x) / 2) * (-1);
        startPosition.x += shiftby;

        startPosition.y = (parrentRect.rect.height - squareSize.y) / 2;

        return startPosition;
    }
}
