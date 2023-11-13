using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Dependencies.Sqlite.SQLite3;

[CreateAssetMenu(fileName = "ListBlock", menuName = "DataBlock/ListBlockSO")]
public class BlockSO : ScriptableObject
{
    public BlockData[] listBlockData;
}
[System.Serializable]
public class BlockData
{
    public ColorType colorType;
    public Sprite sprite;
}
public enum ColorType
{
    red,
    green,
    blue,
    yellow,
    purple,
    none
}
