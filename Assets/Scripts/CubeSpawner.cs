using System;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{

    public GameObject Black;
    public GameObject Yellow;
    public GameObject Blue;
    public GameObject White;

    public int gridSize;

    public AStarGridLayout GridLayout;

    private int[,] strength;

    private void Awake()
    {


    }

    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

        // green side
        // q - black
        // w - yellow
        // e - blue
        // r - white
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Camera.main.farClipPlane))
            {
                if (hit.collider.gameObject.CompareTag("Terrain"))
                {
                    GameObject spawn = Black;
                    spawn.transform.position = GridLayout.GetNodeFromPosition(hit.point).PositionInWorld + Vector3.up;
                    spawn.GetComponent<Influence>().side = false;
                    Instantiate(spawn);

                }
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Camera.main.farClipPlane))
            {
                if (hit.collider.gameObject.CompareTag("Terrain"))
                {
                    GameObject spawn = Yellow;
                    spawn.transform.position = GridLayout.GetNodeFromPosition(hit.point).PositionInWorld + Vector3.up;
                    spawn.GetComponent<Influence>().side = false;
                    Instantiate(spawn);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Camera.main.farClipPlane))
            {
                if (hit.collider.gameObject.CompareTag("Terrain"))
                {
                    GameObject spawn = Blue;
                    spawn.transform.position = GridLayout.GetNodeFromPosition(hit.point).PositionInWorld + Vector3.up;
                    spawn.GetComponent<Influence>().side = false;
                    Instantiate(spawn);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Camera.main.farClipPlane))
            {
                if (hit.collider.gameObject.CompareTag("Terrain"))
                {
                    GameObject spawn = White;
                    spawn.transform.position = GridLayout.GetNodeFromPosition(hit.point).PositionInWorld + Vector3.up;
                    spawn.GetComponent<Influence>().side = false;
                    Instantiate(spawn);
                }
            }
        }

        // red side
        // a - black
        // s - yellow
        // d - blue
        // f - white
        if (Input.GetKeyDown(KeyCode.A))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Camera.main.farClipPlane))
            {
                if (hit.collider.gameObject.CompareTag("Terrain"))
                {
                    GameObject spawn = Black;
                    spawn.transform.position = GridLayout.GetNodeFromPosition(hit.point).PositionInWorld + Vector3.up;
                    spawn.GetComponent<Influence>().side = true;
                    Instantiate(spawn);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Camera.main.farClipPlane))
            {
                if (hit.collider.gameObject.CompareTag("Terrain"))
                {
                    GameObject spawn = Yellow;
                    spawn.transform.position = GridLayout.GetNodeFromPosition(hit.point).PositionInWorld + Vector3.up;
                    spawn.GetComponent<Influence>().side = true;
                    Instantiate(spawn);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Camera.main.farClipPlane))
            {
                if (hit.collider.gameObject.CompareTag("Terrain"))
                {
                    GameObject spawn = Blue;
                    spawn.transform.position = GridLayout.GetNodeFromPosition(hit.point).PositionInWorld + Vector3.up;
                    spawn.GetComponent<Influence>().side = true;
                    Instantiate(spawn);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Camera.main.farClipPlane))
            {
                if (hit.collider.gameObject.CompareTag("Terrain"))
                {
                    GameObject spawn = White;
                    spawn.transform.position = GridLayout.GetNodeFromPosition(hit.point).PositionInWorld + Vector3.up;
                    spawn.GetComponent<Influence>().side = true;
                    Instantiate(spawn);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            GameObject[] all = GameObject.FindGameObjectsWithTag("Influence");
            foreach (GameObject g in all)
            {
                Destroy(g);
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (GameObject.Find("GridLayout").GetComponent<MeshRenderer>().enabled)
            {
                GameObject.Find("GridLayout").GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                GameObject.Find("GridLayout").GetComponent<MeshRenderer>().enabled = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            // generate influence map
            CalculateInfluenceMap();
        }
    }

    private void CalculateInfluenceMap()
    {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Influence");
        strength = new int[GridLayout.GridSize, GridLayout.GridSize];
        foreach (var cube in cubes)
        {
            //Debug.Log(cube.name);
            Influence cubeInfluence = cube.GetComponent<Influence>();
            GridNode node = GridLayout.GetNodeFromPosition(cube.transform.position);
            for (int i = -cubeInfluence.strength + 1; i < cubeInfluence.strength; ++i)
            {
                for (int j = -cubeInfluence.strength + 1; j < cubeInfluence.strength; ++j)
                {
                    // Laziness
                    try
                    {
                        strength[node.X + i, node.Y + j] += (cubeInfluence.strength - Mathf.Max(Mathf.Abs(i), Mathf.Abs(j))) * (cube.GetComponent<Influence>().side ? 1 : -1);
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        // Index out of rage
                        Debug.Log(e);
                    }
                    //strength[node.X + i, node.Y + j] += (cubeInfluence.strength - Mathf.Max(Mathf.Abs(i), Mathf.Abs(j))) * (cube.GetComponent<Influence>().side ? 1 : -1);
                    //Debug.Log(cube.GetComponent<Influence>().side);
                }
            }
        }

        for (int i = 0; i < GridLayout.GridSize; ++i)
        {
            for (int j = 0; j < GridLayout.GridSize; ++j)
            {
                for (int k = 0; k < GridLayout.ColorIndex[i, j].Count; ++k)
                {
                    //Debug.Log(Mathf.Abs(strength[i, j]) / 3.0f);
                    GridLayout.Colors[GridLayout.ColorIndex[i, j][k]] = Color.Lerp(Color.gray,
                        strength[i, j] > 0 ? Color.red : Color.green, Mathf.Abs(strength[i, j]) / 3.0f);
                }
            }
        }
        GridLayout.UpdateColor();
    }
}
