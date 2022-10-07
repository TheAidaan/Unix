using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector3Int boardPosition = Vector3Int.zero;
    public Board board = null;

    public BaseUnit currentUnit = null;

    public string tileID;

    Color _black = new Color(.254f, .254f, .254f);
    Color _white = new Color(1, 1, 1);
    Color _tile;


    public void Setup(Board newBoard, Vector3Int BoardPosition ,string TileID)
    {
        boardPosition = BoardPosition;
        board = newBoard;
        tileID = TileID;
    }

    public void Focus()
    {
        Color focusColor = UXManager.inDarkMode ? _white :_black;

        GetComponent<Renderer>().material.color = focusColor; 
    }

    public void RemoveFocus()
    {
        GetComponent<Renderer>().material.color = _tile;
    }

    public void StoreColor()
    {
        _tile = GetComponent<Renderer>().material.color;

    }

}
