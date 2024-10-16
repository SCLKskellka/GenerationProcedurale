using UnityEngine;

public struct Room
{
    public Color Color;
    public Vector2Int Size;
    public Vector2Int Anchor;

    public Room(int x, int y, int i, int j) {
        Size = new Vector2Int(x, y);
        Anchor = new Vector2Int(i, j);
        Color = Random.ColorHSV();
    }

    public Room(Room origin)
    {
        Size = origin.Size;
        Anchor = origin.Anchor;
        Color = Random.ColorHSV();
    }

    public Room Clone(){
        Room clone = new Room(Size.x, Size.y, Anchor.x, Anchor.y);
        return clone;
    }
}