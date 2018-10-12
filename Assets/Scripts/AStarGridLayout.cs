using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStarGridLayout : MonoBehaviour
{

    public Terrain MainTerrain;
    public int GridSize;

    private Vector3 _terrainSize;
    private GridNode[,] _grid;
    private float _nodeXWidth;
    private float _nodeZWidth;
    private bool _drawGizmos = false;

    public int MaxSize
    {
        get { return GridSize * GridSize; }
    }

    // Use this for initialization
    void Awake()
    {
        _drawGizmos = true;
        _terrainSize = MainTerrain.terrainData.size;

        _nodeXWidth = (_terrainSize.x / GridSize);
        _nodeZWidth = (_terrainSize.z / GridSize);

        _grid = new GridNode[GridSize, GridSize];

        for (int i = 0; i < GridSize; ++i)
        {
            for (int j = 0; j < GridSize; ++j)
            {
                float xPos = _nodeXWidth * (i + 0.5f) + MainTerrain.transform.position.x;
                float zPos = _nodeZWidth * (j + 0.5f) + MainTerrain.transform.position.z;
                float yPos = _terrainSize.y;
                bool isWalkable = true;

                RaycastHit hit;

                if (Physics.Raycast(new Vector3(xPos, MainTerrain.transform.position.y + _terrainSize.y, zPos),
                    Vector3.down, out hit, _terrainSize.y * 1.1f))
                {
                    yPos = hit.point.y;
                    if (hit.collider.gameObject.CompareTag("Terrain"))
                    {
                        var colliders = Physics.OverlapBox(new Vector3(xPos, yPos, zPos),
                            new Vector3(_nodeXWidth / 2, (_nodeXWidth + _nodeZWidth) / 4, _nodeZWidth / 2));

                        if (colliders.Any(collider1 => !collider1.gameObject.CompareTag("Terrain")))
                        {
                            isWalkable = false;
                        }

                    }
                    else
                    {
                        isWalkable = false;
                    }
                }

                _grid[i, j] = new GridNode(isWalkable, new Vector3(xPos, yPos, zPos), i, j);

            }
        }

    }

    void OnDrawGizmos()
    {
        if (!_drawGizmos)
        {
            return;
        }

        for (int i = 0; i < GridSize; ++i)
        {
            for (int j = 0; j < GridSize; ++j)
            {
                Gizmos.color = _grid[i, j].IsWalkable ? Color.yellow : Color.black;
                if (i - 1 >= 0)
                {
                    Gizmos.color = _grid[i - 1, j].IsWalkable ? Gizmos.color : Color.black;
                    Gizmos.DrawLine(_grid[i, j].PositionInWorld + Vector3.up * 0.05f, _grid[i - 1, j].PositionInWorld + Vector3.up * 0.05f);
                }

                if (j - 1 >= 0)
                {
                    Gizmos.color = _grid[i, j - 1].IsWalkable ? Gizmos.color : Color.black;
                    Gizmos.DrawLine(_grid[i, j].PositionInWorld + Vector3.up * 0.05f, _grid[i, j - 1].PositionInWorld + Vector3.up * 0.05f);
                }

                if (i - 1 >= 0 && j - 1 >= 0)
                {
                    Gizmos.color = _grid[i - 1, j - 1].IsWalkable ? Gizmos.color : Color.black;
                    Gizmos.DrawLine(_grid[i, j].PositionInWorld + Vector3.up * 0.05f, _grid[i - 1, j - 1].PositionInWorld + Vector3.up * 0.05f);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GridNode GetNodeFromPosition(Vector3 positionInWorld)
    {
        int i = Mathf.RoundToInt((positionInWorld.x - MainTerrain.transform.position.x) / _nodeXWidth - 0.5f);
        int j = Mathf.RoundToInt((positionInWorld.z - MainTerrain.transform.position.z) / _nodeZWidth - 0.5f);
        return _grid[i, j];
    }

    public List<GridNode> GetNeighborhood(GridNode node)
    {
        List<GridNode> list = new List<GridNode>();
        for (int i = -1; i <= 1; ++i)
        {
            for (int j = -1; j <= 1; ++j)
            {
                if (i == 0 && j == 0)
                    continue;
                int x = node.X + i;
                int y = node.Y + j;
                if (x < GridSize && x >= 0 && y < GridSize && y >= 0)
                    list.Add(_grid[x, y]);
            }
        }
        return list;
    }
}
