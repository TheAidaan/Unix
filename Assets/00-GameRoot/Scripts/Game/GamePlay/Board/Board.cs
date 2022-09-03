using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum TileState
{
    None,
    Taken,
    Free,
    OutOfBounds
}

public class Board : MonoBehaviour
{
    
    public Tile[,] allTiles; //stores all tiles

    public void Create()
    {
        GameObject[,] map = new GameObject[10,6];

        allTiles = new Tile[GameManager.gameData.GetBoardLength(), GameManager.gameData.GetBoardLength()];

        GameObject _tilePrefab = Resources.Load<GameObject>("Prefabs/Tile");
       
        for (int x=0; x < GameManager.gameData.GetBoardLength(); x++)
        {
            for (int y = 0; y < GameManager.gameData.GetBoardLength(); y++)
            {
                if (GameManager.gameData.ComplexBoard())
                {
                    if ((x == 0 && y == 0) || (x == 0 && y == 1) || (x == 9 && y == 1) || (x == 9 && y == 0))
                    {
                        continue;
                    }
                    if ((x == 0 && y == 9) || (x == 0 && y == 8) || (x == 9 && y == 8) || (x == 9 && y == 9))
                    {
                        continue;
                    }
                }
              

                GameObject newTile = Instantiate(_tilePrefab, transform);

                Transform mtransform = newTile.GetComponent<Transform>();
                mtransform.position = new Vector3Int((x * 10) + 50,0, (y * 10) + 50);

                allTiles[x, y] = newTile.GetComponent<Tile>();
                string tileID = x.ToString() +","+ y.ToString();
                allTiles[x, y].Setup(this, new Vector3Int(x,0,y),tileID);

                if (GameManager.gameData.ComplexBoard())
                    if (y != 0 && y != 1 && y != 8 && y != 9)
                        map[x, y - 2] = newTile;
            }
            
        }

        for (int x = 0; x < GameManager.gameData.GetBoardLength(); x+=2)
        {
            for (int y = 0; y < GameManager.gameData.GetBoardLength(); y++)
            {
                int offset = (y % 2 != 0) ? 0 : 1;
                int finalX = x  + offset;
                Tile tile = allTiles[finalX, y];
                if (tile!=null)
                    tile.GetComponent<Renderer>().material.color = new Color32(219,202,216, 255);
            }
        }

        if (GameManager.gameData.ComplexBoard())
            GenerateMap(map);
    }

    void GenerateMap(GameObject[,] map)
    {
        bool[,] boolMap = new bool[map.GetLength(0), map.GetLength(1)];

        float chanceToStayAlive = 0.55f;

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                float rand = UnityEngine.Random.Range(0, 100);
                rand /= 100;
                if (rand < chanceToStayAlive)
                {
                    boolMap[x, y] = true;
                }
            }
        }

        boolMap = ConditonMap(boolMap);

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {

                if (!boolMap[x, y])
                {
                    map[x, y].GetComponent<MeshRenderer>().enabled = false;
                    Destroy(map[x, y].GetComponent<Tile>());
                }

            }
        }
    }

    bool[,] ConditonMap(bool[,] boolMapOld)
    {
        int activeNeighbourLimit = 3;
        bool[,] boolMapNew = new bool[boolMapOld.GetLength(0), boolMapOld.GetLength(1)];

        for(int x = 0; x < boolMapOld.GetLength(0); x++)
        {
            for(int y=0;y< boolMapOld.GetLength(1); y++)
            {
                int activeNeighbours = countActiveNeighbours(boolMapOld, x, y);
                if (boolMapOld[x, y])
                {
                    if (activeNeighbours < activeNeighbourLimit)
                        boolMapNew[x, y] = false;
                    else
                        boolMapNew[x, y] = true;
                }else
                {
                    if (activeNeighbours > activeNeighbourLimit)
                        boolMapNew[x, y] = true;
                    else
                        boolMapNew[x, y] = false;
                }
            }
        }

        return boolMapNew;

        Debug.Log("board");
    }

    public int countActiveNeighbours(bool[,] map, int x, int y)
    {
        int count = 0;

        for(int i = -1; i < 2; i++) //x-1 is left, x+1 is right, 0 is the vertical row the tile is on
        {
            for(int j = -1; j < 2; j++)//y-1 is above, y+1 is below, 0 is the horizontal row the tile is on
            {
                int neigbourX = x + i;
                int neigbourY = y + j;

                if (i == 0 && j == 0) //ignore self
                    continue;

                if (neigbourX < 0 || neigbourY < 0 || neigbourX >= map.GetLength(0) || neigbourY >= map.GetLength(1))
                    count++;
                else if (map[neigbourX, neigbourY])
                    count++;
            }
        }

        return count;
    }

    public TileState ValidateTile(int targetX, int targetY, BaseUnit checkingUnit)
    {
        //is Target on the board (Bounds Check)
        if (targetX < 0 || targetX >= GameManager.gameData.GetBoardLength())                                                           
            return TileState.OutOfBounds;

        if (targetY < 0 || targetY >= GameManager.gameData.GetBoardLength())
            return TileState.OutOfBounds;

        Tile targetTile = allTiles[targetX, targetY]; // get the specific tile

        if (targetTile != null)
        {
            if (targetTile.currentUnit != null) // is there a unit on the target tile?
            {
                if (targetTile.currentUnit.gameObject.activeSelf) // is that unit active
                {
                    return TileState.Taken;
                }
            }

            return TileState.Free;
        }

            return TileState.OutOfBounds;
    }
}
