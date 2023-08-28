using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Category
{
    public string category_name;

    public bool IscategroyOpned;

    public List<BoardData> PuzzleBoards;

    public int coinsToUnlock;

}
