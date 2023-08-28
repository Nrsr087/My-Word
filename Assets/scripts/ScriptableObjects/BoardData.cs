using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class BoardData : ScriptableObject
{
    [System.Serializable]
    public class SearchingWord
    {
        public string word;
    }


    [System.Serializable]
    public class BoradRow
    {
        public int Size;
        public string[] Row;

        public BoradRow(int size)
        {
            creatRow(size);
        }

        public void creatRow(int size)
        {
            Size = size;
            Row = new string[size];
            clearRow();
        }

        public void clearRow()
        {
            for (int i = 0; i < Size; i++)
            {
                Row[i] = " ";
            }
        }
    }

    public float TimeInSecconds;
    public int colums = 0;
    public int Rows = 0;

    public BoradRow[] Board;
    public List<SearchingWord> searchwords = new List<SearchingWord>();

    public void ClearWithEmptyStrings()
    {
        for (int i = 0; i < colums; i++)
        {
            Board[i].clearRow();
        }
    }

    public void creatNewBoard()
    {
        Board = new BoradRow[colums];

        for (int i = 0; i < colums; i++)
        {
            Board[i] = new BoradRow(Rows);
        }
    }
}
