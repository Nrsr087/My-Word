using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class DiffData : ScriptableObject
{
    public GameLevelData gameLevelData;
    public List<string> easyList;
    public List<string> normalList;
    public List<string> hardList;
    public List<string> veryhardList;
}
