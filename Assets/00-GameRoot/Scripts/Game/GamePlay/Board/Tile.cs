using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector3Int boardPosition = Vector3Int.zero;
    public Board board = null;

    public BaseUnit currentUnit = null;

    public string tileID;

    
    public void Setup(Board newBoard, Vector3Int BoardPosition ,string TileID)
    {
        boardPosition = BoardPosition;
        board = newBoard;
        tileID = TileID;
            

    }

}
