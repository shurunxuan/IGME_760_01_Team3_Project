using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode
{

    public Vector3 PositionInWorld;
    public bool IsWalkable;
    public int X;
    public int Y;
    public GridNode Parent;

    public int gCost;
    public int hCost;

    public int walkstatus;

    public GridNode(bool isWalkable, Vector3 position, int x, int y)
    {
        IsWalkable = isWalkable;
        PositionInWorld = position;
        X = x;
        Y = y;
    }
}
