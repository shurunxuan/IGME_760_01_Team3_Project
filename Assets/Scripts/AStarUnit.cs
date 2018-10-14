using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AStarUnit : Unit
{
    public GameObject GridLayout;
    public GameObject TargetPosition;

    public GameObject PathCube;
    private List<GameObject> _pathCubes;


    private AStarGridLayout _grid;
    private Rigidbody _rigidbody;

    private bool _pathReady;

    private List<GridNode> _path;

    private Vector3 _currentPosition;
    private Vector3 _targetPosition;

    private bool _findPathRunning;
    // Use this for initialization
    void Start()
    {
        // Get Rigidbody
        _rigidbody = GetComponent<Rigidbody>();
        _findPathRunning = false;
        _pathReady = false;
        _path = new List<GridNode>();
        _grid = GridLayout.GetComponent<AStarGridLayout>();
        _targetPosition = transform.position;
        useVelocityAsDirection = false;
        PathCube.transform.localScale = Vector3.one * 0.5f;
        _pathCubes = new List<GameObject>();
    }

    void FixedUpdate()
    {
        MoveUnit(_rigidbody);
    }

    // Update is called once per frame
    void Update()
    {
        _currentPosition = transform.position;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Camera.main.farClipPlane))
            {
                if (hit.collider.gameObject.CompareTag("Terrain"))
                {
                    _targetPosition = hit.point;
                    _pathReady = false;
                }
            }
        }

        for (int i = 0; i < Mathf.Min(_path.Count, _pathCubes.Count); ++i)
        {
            _pathCubes[i].transform.position = _path[i].PositionInWorld;
        }

        if (_pathCubes.Count > _path.Count)
        {
            for (int i = _path.Count; i < _pathCubes.Count; ++i)
            {
                Object.Destroy(_pathCubes[i]);
            }
            _pathCubes.RemoveRange(_path.Count, _pathCubes.Count - _path.Count);
        }
        else
        {
            for (int i = _pathCubes.Count; i < _path.Count; ++i)
            {
                GameObject newCube = Instantiate(PathCube);
                newCube.transform.position = _path[i].PositionInWorld;
                newCube.SetActive(true);
                _pathCubes.Add(newCube);
            }
        }

        if (!_pathReady)
            RunFindPath();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (_path != null)
            foreach (var gridNode in _path)
                Gizmos.DrawCube(gridNode.PositionInWorld, Vector3.one * 0.5f);
    }

    // This is the direction the unit will head to when GetMoveDirection().x > 0
    protected override void SetLocalRight()
    {
        localRight = Vector3.right;
    }

    // This is the direction the unit will head to when GetMoveDirection().y > 0
    protected override void SetLocalForward()
    {
        localForward = Vector3.forward;
    }

    protected override Vector2 GetMoveDirection()
    {
        if (_path == null || _path.Count == 0 || !_pathReady)
            return new Vector2(0, 0);

        GridNode node = _path[0];
        Vector3 diff = node.PositionInWorld - transform.position;
        diff.y = 0;
        if (diff.magnitude > _grid.NodeWidth * 4.0f) RunFindPath();
        if (diff.magnitude < _grid.NodeWidth * 2.0f) _path.RemoveAt(0);
        return new Vector2(diff.x, diff.z);
    }

    void FindPath()
    {
        GridNode startNode = _grid.GetNodeFromPosition(_currentPosition);
        GridNode endNode = _grid.GetNodeFromPosition(_targetPosition);


        Heap<GridNode> openSet = new Heap<GridNode>(_grid.MaxSize);
        HashSet<GridNode> closeSet = new HashSet<GridNode>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            GridNode curNode = openSet.RemoveFirst();
            closeSet.Add(curNode);

            // Found target node
            if (curNode == endNode)
            {
                GeneratePath(startNode, endNode);
                return;
            }

            // Find a node with minimal cost
            foreach (var item in _grid.GetNeighborhood(curNode))
            {
                if (!item.IsWalkable || closeSet.Contains(item)) continue;

                int newCost = curNode.GCost + _grid.GetDistanceNodes(curNode, item);
                if (newCost < item.GCost || !openSet.Contains(item))
                {
                    item.GCost = newCost;
                    item.HCost = _grid.GetDistanceNodes(item, endNode);
                    item.Parent = curNode;
                    if (!openSet.Contains(item))
                        openSet.Add(item);
                }
            }
        }

        GeneratePath(startNode, null);

    }

    void GeneratePath(GridNode startNode, GridNode endNode)
    {
        List<GridNode> path = new List<GridNode>();
        if (endNode != null)
        {
            GridNode temp = endNode;
            while (temp != startNode)
            {
                path.Add(temp);
                temp = temp.Parent;
            }
            path.Reverse();
        }
        _path = path;
        _pathReady = true;
        _findPathRunning = false;
    }

    Task RunFindPath()
    {
        if (_findPathRunning) return null;
        Task t = new Task(FindPath);
        _findPathRunning = true;
        t.Start();

        return t;
    }

}
