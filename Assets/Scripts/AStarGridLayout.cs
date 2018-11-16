using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStarGridLayout : MonoBehaviour
{

    public Terrain MainTerrain;
    public int GridSize;
    public LayerMask IgnoreLayer;

    private Vector3 _terrainSize;
    public GridNode[,] Grid;
    private float _nodeXWidth;
    private float _nodeZWidth;
    private bool _drawGizmos;
    private Vector3 _terrainPosition;

    private MeshFilter mF;

    public int MaxSize => GridSize * GridSize;
    public float NodeWidth => (_nodeXWidth + _nodeZWidth) / 2.0f;

    public Color[] Colors;
    public List<int>[,] ColorIndex;

    // Use this for initialization
    void Awake()
    {
        _terrainPosition = MainTerrain.transform.position;
        _drawGizmos = false;
        _terrainSize = MainTerrain.terrainData.size;

        _nodeXWidth = (_terrainSize.x / GridSize);
        _nodeZWidth = (_terrainSize.z / GridSize);

        Grid = new GridNode[GridSize, GridSize];

        for (int i = 0; i < GridSize; ++i)
        {
            for (int j = 0; j < GridSize; ++j)
            {
                float xPos = _nodeXWidth * (i + 0.5f) + _terrainPosition.x;
                float zPos = _nodeZWidth * (j + 0.5f) + _terrainPosition.z;
                float yPos = _terrainSize.y;
                bool isWalkable = true;

                RaycastHit hit;
                Ray ray = new Ray(new Vector3(xPos, _terrainPosition.y + _terrainSize.y, zPos),
                    Vector3.down);
                if (Physics.Raycast(ray, out hit, _terrainSize.y * 1.1f, ~IgnoreLayer))
                {
                    yPos = hit.point.y;
                    if (hit.collider.gameObject.CompareTag("Terrain"))
                    {
                        var colliders = Physics.OverlapBox(new Vector3(xPos, yPos, zPos),
                            new Vector3(_nodeXWidth / 2, (_nodeXWidth + _nodeZWidth) / 4, _nodeZWidth / 2));

                        if (colliders.Any(collider1 => !collider1.gameObject.CompareTag("Terrain")))
                            isWalkable = false;

                    }
                    else
                        isWalkable = false;
                }

                Grid[i, j] = new GridNode(isWalkable, new Vector3(xPos, yPos, zPos), i, j);

            }
        }


        // grid drawing code from github
        // drawn in awake so it renders before the game starts
        // https://gist.github.com/mdomrach/a66602ee85ce45f8860c36b2ad31ea14

        mF = GetComponent<MeshFilter>();
        Mesh m = new Mesh();
        List<Vector3> vert = new List<Vector3>();

        List<int> indicies = new List<int>();

        ColorIndex = new List<int>[GridSize, GridSize];
        for (int i = 0; i < GridSize; ++i)
            for (int j = 0; j < GridSize; ++j)
                ColorIndex[i, j] = new List<int>(4);
        int index = 0;
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                if (i + 1 < GridSize)
                {
                    vert.Add(Grid[i, j].PositionInWorld);
                    ColorIndex[i, j].Add(index);
                    indicies.Add(index++);

                    vert.Add(Grid[i + 1, j].PositionInWorld);
                    ColorIndex[i + 1, j].Add(index);
                    indicies.Add(index++);
                }

                if (j + 1 < GridSize)
                {
                    vert.Add(Grid[i, j].PositionInWorld);
                    ColorIndex[i, j].Add(index);
                    indicies.Add(index++);

                    vert.Add(Grid[i, j + 1].PositionInWorld);
                    ColorIndex[i, j + 1].Add(index);
                    indicies.Add(index++);
                }
            }
        }

        m.vertices = vert.ToArray();
        m.SetIndices(indicies.ToArray(), MeshTopology.Lines, 0);
        mF.mesh = m;

        MeshRenderer mR = GetComponent<MeshRenderer>();
        mR.material = new Material(Shader.Find("Sprites/Default"));
        //mR.material.color = Color.white;
        //Debug.Log(vert.Count);
        //Debug.Log(GridSize * GridSize);
        Colors = new Color[vert.Count];
        for (int i = 0; i < Colors.Length; ++i)
        {
            Colors[i] = Color.gray;
        }
        UpdateColor();

    }

    public void UpdateColor()
    {
        mF.mesh.colors = Colors;
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
                Gizmos.color = Grid[i, j].IsWalkable ? Color.yellow : Color.black;
                if (i - 1 >= 0)
                {
                    Gizmos.color = Grid[i - 1, j].IsWalkable ? Gizmos.color : Color.black;
                    Gizmos.DrawLine(Grid[i, j].PositionInWorld + Vector3.up * 0.05f,
                        Grid[i - 1, j].PositionInWorld + Vector3.up * 0.05f);
                }

                if (j - 1 >= 0)
                {
                    Gizmos.color = Grid[i, j - 1].IsWalkable ? Gizmos.color : Color.black;
                    Gizmos.DrawLine(Grid[i, j].PositionInWorld + Vector3.up * 0.05f,
                        Grid[i, j - 1].PositionInWorld + Vector3.up * 0.05f);
                }

                if (i - 1 >= 0 && j - 1 >= 0)
                {
                    Gizmos.color = Grid[i - 1, j - 1].IsWalkable ? Gizmos.color : Color.black;
                    Gizmos.DrawLine(Grid[i, j].PositionInWorld + Vector3.up * 0.05f,
                        Grid[i - 1, j - 1].PositionInWorld + Vector3.up * 0.05f);
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
        int i = Mathf.RoundToInt((positionInWorld.x - _terrainPosition.x) / _nodeXWidth - 0.5f);
        int j = Mathf.RoundToInt((positionInWorld.z - _terrainPosition.z) / _nodeZWidth - 0.5f);
        return Grid[i, j];
    }

    public List<GridNode> GetNeighborhood(GridNode node)
    {
        List<GridNode> list = new List<GridNode>();
        for (int i = -1; i <= 1; ++i)
            for (int j = -1; j <= 1; ++j)
            {
                if (i == 0 && j == 0)
                    continue;
                int x = node.X + i;
                int y = node.Y + j;
                if (x < GridSize && x >= 0 && y < GridSize && y >= 0)
                    list.Add(Grid[x, y]);
            }

        return list;
    }

    public int GetDistanceNodes(GridNode a, GridNode b)
    {
        Vector3 diff = b.PositionInWorld - a.PositionInWorld;
        float horizontal = Mathf.Sqrt(diff.x * diff.x + diff.z * diff.z);
        float vertical = diff.y;
        return Mathf.RoundToInt(100 * horizontal + vertical * (vertical > 0 ? 400 : -100));
    }
}
