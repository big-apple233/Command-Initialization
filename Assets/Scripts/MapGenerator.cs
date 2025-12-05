using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    None,
    Grass,
    Stone
}

[Serializable]
public class MapTileData
{
    public TileType tileType;
    public GameObject tilePrefab;
}

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance { get; private set; }

    [SerializeField] private List<TextAsset> mapList = new List<TextAsset>();
    [SerializeField] private List<MapTileData> tileDataList = new List<MapTileData>();
    [SerializeField] private Transform mapParent;

    // 二维数组存储地图格子
    private MapTile[,] mapArray;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(this);
    }

    private GameObject GetTilePrefab(TileType tileType)
    {
        foreach (var tileData in tileDataList)
        {
            if (tileData.tileType == tileType)
                return tileData.tilePrefab;
        }
        return null;
    }

    private TileType SwitchStringToTileType(string s)
    {
        switch (s)
        {
            case "Grass": return TileType.Grass;
            case "Stone": return TileType.Stone;
            default: return TileType.None;
        }
    }

    private void ClearMap()
    {
        if (mapParent == null) mapParent = this.transform;

        for (int i = mapParent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(mapParent.GetChild(i).gameObject);
        }
    }

    public void GenerateMap(TextAsset textAsset)
    {
        if (textAsset == null)
        {
            Debug.LogError("地图文件为空！");
            return;
        }

        ClearMap();

        string[] mapText = textAsset.text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        int rows = mapText.Length;
        if (rows == 0) return;

        string[] firstRow = mapText[0].Split(',');
        int cols = firstRow.Length;

        mapArray = new MapTile[cols, rows];

        float offsetX = (cols - 1) / 2f;
        float offsetZ = (rows - 1) / 2f;

        for (int row = 0; row < rows; row++)
        {
            string[] cells = mapText[row].Split(',');

            for (int col = 0; col < cols; col++)
            {
                TileType type = SwitchStringToTileType(cells[col]);

                Vector3 pos = new Vector3(col - offsetX, 0, row - offsetZ);
                GameObject obj = null;
                if (type != TileType.None)
                {
                    GameObject prefab = GetTilePrefab(type);
                    if (prefab != null)
                        obj = Instantiate(prefab, pos, Quaternion.identity, mapParent);
                }

                // 用 MapTile 保存格子信息
                mapArray[col, row] = obj.GetComponent<MapTile>();
            }
        }
    }

    // 获取 MapTile 二维数组
    public MapTile[,] GetMapArray()
    {
        return mapArray;
    }
}
