using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

enum TileType
{
    WALL = 0,
    FLOOR = 1,
}

public class Maze : MonoBehaviour {
    public int width = 23;
    public int length = 23;
    public int middle = 11;
    public float height = 2.15f;
    public GameObject bird;
    public GameObject claw;
    public GameObject crosshair;
    public GameObject dude;
    public GameObject fish;
    public GameObject hole;
    public GameObject pointy;
    public GameObject temple;
    public GameObject trefoil;
    public GameObject fps_player_obj;
    internal bool solved_puzzle = false;

    private Bounds bounds;
    private float timestamp_last_msg = 0.0f;
    private int function_calls = 0;
    private bool started = true;
    public Material wallMaterial;
    public Material outerWallMaterial;
    private SymbolPuzzle puzzleScript;
    public GameObject puzzle;
    private GameObject npc1, npc2, npc3, npc4;

    private void Shuffle<T>(ref List<T> list)
    {
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    void Start() {
        puzzleScript = puzzle.GetComponent<SymbolPuzzle>();

        List<GameObject> npc_order = new List<GameObject> { pointy, hole, dude, fish, claw, bird, crosshair, temple, trefoil }; 

        bounds = GetComponent<Collider>().bounds; 
        timestamp_last_msg = 0.0f;
        function_calls = 0;

        List<TileType>[,] grid = new List<TileType>[width, length];
        List<int[]> unassigned = new List<int[]>();

        bool success = false;        
        while (!success)
        {
            for (int w = 0; w < width; w++)
                for (int l = 0; l < length; l++)
                    if (w == 0 || l == 0 || w == width - 1 || l == length - 1) {
                        grid[w, l] = new List<TileType> { TileType.WALL };
                    } else {
                        if (grid[w, l] == null)
                        {
                            List<TileType> candidate_assignments = new List<TileType> { TileType.WALL, TileType.FLOOR };
                            Shuffle<TileType>(ref candidate_assignments);

                            grid[w, l] = candidate_assignments;
                            unassigned.Add(new int[] { w, l });
                        }
                    }

            success = BackTrackingSearch(grid, unassigned);
            if (!success) {
                unassigned.Clear();
                grid = new List<TileType>[width, length];
                function_calls = 0; 
            }
        }
        puzzleScript.puzzleLength = 4;

        npc1 = npc_order[puzzleScript.solution[0]];
        npc2 = npc_order[puzzleScript.solution[1]];
        npc3 = npc_order[puzzleScript.solution[2]];
        if (puzzleScript.puzzleLength == 4) { npc4 = npc_order[puzzleScript.solution[3]]; }
        // set npcs active
        npc1.SetActive(true);
        npc2.SetActive(true);
        npc3.SetActive(true);
        if (puzzleScript.puzzleLength == 4) { npc4.SetActive(true); }

        DrawMaze(grid);
    }


    bool TooManyWalls(List<TileType>[,] grid) {
        int[] number_of_assigned_elements = new int[] { 0, 0 };
        for (int w = 0; w < width; w++)
            for (int l = 0; l < length; l++)
            {
                if (w == 0 || l == 0 || w == width - 1 || l == length - 1)
                    continue;
                if (grid[w, l].Count == 1)
                    number_of_assigned_elements[(int)grid[w, l][0]]++;
            }

        if ((number_of_assigned_elements[(int)TileType.WALL] > 70))
            return true;
        else
            return false;
    }


    bool TooFewWalls(List<TileType>[,] grid) {
        int[] number_of_potential_assignments = new int[] { 0, 0 };
        for (int w = 0; w < width; w++)
            for (int l = 0; l < length; l++)
            {
                if (w == 0 || l == 0 || w == width - 1 || l == length - 1)
                    continue;
                for (int i = 0; i < grid[w, l].Count; i++) {
                    number_of_potential_assignments[(int)grid[w, l][i]]++;
                }
            }

        if ((number_of_potential_assignments[(int)TileType.WALL] < 50))
            return true;
        else
            return false;
    }


    bool TooLongWall(List<TileType>[,] grid) {
        int max_len = 4;

        // vertical walls
        for (int l = 1; l < length - 1; l++) {
            int num_walls = 0;
            for (int w = 1; w < width - 1; w++) {
                if ((grid[w, l].Count == 1) && (grid[w, l][0] == TileType.WALL)) {
                    num_walls++;
                    if (num_walls > max_len) {
                        return true;
                    }
                } else {
                    num_walls = 0;
                }
            }
        }

        // horizontal walls
        for (int w = 1; w < width - 1; w++) {
            int num_walls = 0;
            for (int l = 1; l < length - 1; l++) {
                if ((grid[w, l].Count == 1) && (grid[w, l][0] == TileType.WALL)) {
                    num_walls++;
                    if (num_walls > max_len) {
                        return true;
                    }
                } else {
                    num_walls = 0;
                }
            }
        }

        return false;
    }


    bool NoCornerWalls(List<TileType>[,] grid) {
        for (int w = 1; w < width - 1; w++) {
            for (int l = 1; l < length - 1; l++) {
                if ((grid[w, l].Count == 1) && (grid[w, l][0] == TileType.WALL)) {

                    if ((grid[w - 1, l - 1].Count >= 1) && (grid[w - 1, l - 1][0] == TileType.WALL)) { return true; }
                    if ((grid[w - 1, l + 1].Count >= 1) && (grid[w - 1, l + 1][0] == TileType.WALL)) { return true; }
                    if ((grid[w + 1, l - 1].Count >= 1) && (grid[w + 1, l - 1][0] == TileType.WALL)) { return true; }
                    if ((grid[w + 1, l + 1].Count >= 1) && (grid[w + 1, l + 1][0] == TileType.WALL)) { return true; }

                }
            }
        }

        return false;
    }

    bool CenterWall(List<TileType>[,] grid) { // make sure the center tile is a wall for puzzle placement
        if ((grid[middle, middle].Count >= 1) && grid[middle, middle][0] == TileType.WALL) { return false; }
        return true;
    }


    bool CheckConsistency(List<TileType>[,] grid, int[] cell_pos, TileType t) {
        int w = cell_pos[0];
        int l = cell_pos[1];

        List<TileType> old_assignment = new List<TileType>();
        old_assignment.AddRange(grid[w, l]);
        grid[w, l] = new List<TileType> { t };

        bool areWeConsistent = !TooFewWalls(grid) && !TooManyWalls(grid) 
                            && !TooLongWall(grid) && !NoCornerWalls(grid) && !CenterWall(grid);

        grid[w, l] = new List<TileType>();
        grid[w, l].AddRange(old_assignment);
        return areWeConsistent;
    }


    bool BackTrackingSearch(List<TileType>[,] grid, List<int[]> unassigned) {
        if (function_calls++ > 100000)
            return false;

        if (unassigned.Count == 0)
            return true;

        Shuffle<int[]>(ref unassigned);
        int[] value = unassigned[unassigned.Count - 1];
        unassigned.RemoveAt(unassigned.Count - 1);
        List<TileType> domain = grid[value[0], value[1]];
        Shuffle<TileType>(ref domain);
        foreach (TileType tiletype in domain) {
            if (CheckConsistency(grid, value, tiletype)) {
                grid[value[0], value[1]] = new List<TileType> { tiletype };
                if (BackTrackingSearch(grid, unassigned)) { return true; }
                grid[value[0], value[1]] = domain;

            }
        }
        unassigned.Add(value);
        return false;

    }


    void DrawMaze(List<TileType>[,] solution) {
        GetComponent<Renderer>().material.color = Color.grey;

        int wr = 0;
        int lr = 0;
        while (true) {
            wr = Random.Range(1, width - 1);
            lr = Random.Range(1, length - 1);

            if (solution[wr, lr][0] == TileType.FLOOR)
            {
                float x = bounds.min[0] + (float)wr * (bounds.size[0] / (float)width);
                float z = bounds.min[2] + (float)lr * (bounds.size[2] / (float)length);
                fps_player_obj.transform.position = new Vector3(x + 0.5f, 2.0f * height, z + 0.5f); 
                break;
            }
        }


        int w = 0;
        for (float x = bounds.min[0]; x < bounds.max[0]; x += bounds.size[0] / (float)width - 1e-6f, w++) {
            int l = 0;
            for (float z = bounds.min[2]; z < bounds.max[2]; z += bounds.size[2] / (float)length - 1e-6f, l++) {
                if ((w >= width) || (l >= width))
                    continue;

                float y = bounds.min[1];
                if (solution[w, l][0] == TileType.WALL) {

                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.name = "WALL";
                    cube.transform.localScale = new Vector3(bounds.size[0] / (float)width, height, bounds.size[2] / (float)length);
                    cube.transform.position = new Vector3(x + 0.5f, y + height * 0.5f, z + 0.5f);
                    cube.GetComponent<Renderer>().material = wallMaterial;
                    NavMeshObstacle navObstacle = cube.AddComponent<NavMeshObstacle>();
                    navObstacle.carving = true;

                    GameObject lowercube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    lowercube.name = "WALL";
                    lowercube.transform.localScale = new Vector3(bounds.size[0] / (float)width, height, bounds.size[2] / (float)length);
                    lowercube.transform.position = new Vector3(x + 0.5f, y - height * 2.5f, z + 0.5f);
                    lowercube.GetComponent<Renderer>().material = wallMaterial;
                    NavMeshObstacle lowernavObstacle = lowercube.AddComponent<NavMeshObstacle>();
                    lowernavObstacle.carving = true;

                    if (w == middle && l == middle) { 
                        //make center puzzle location unique
                        cube.name = "MIDDLE";
                        cube.transform.position = new Vector3(x + 0.5f, y - 0.4f * height, z + 0.5f);
                        lowercube.transform.position = new Vector3(x + 0.5f, y - 2.55f * height, z + 0.5f);
                    }

                    if (w == 0 || l == 0 || w == width - 1 || l == length - 1) {
                        // add additional layers to outer walls
                        cube.GetComponent<Renderer>().material = outerWallMaterial;
                        lowercube.GetComponent<Renderer>().material = outerWallMaterial;

                        GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube2.name = "WALL";
                        cube2.transform.localScale = new Vector3(bounds.size[0] / (float)width, height, bounds.size[2] / (float)length);
                        cube2.transform.position = new Vector3(x + 0.5f, y + height * 1.5f, z + 0.5f);
                        cube2.GetComponent<Renderer>().material = outerWallMaterial;

                        GameObject lowercube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        lowercube2.name = "WALL";
                        lowercube2.transform.localScale = new Vector3(bounds.size[0] / (float)width, height, bounds.size[2] / (float)length);
                        lowercube2.transform.position = new Vector3(x + 0.5f, y - height * 1.5f, z + 0.5f);
                        lowercube2.GetComponent<Renderer>().material = outerWallMaterial;

                        GameObject lowercube3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        lowercube3.name = "WALL";
                        lowercube3.transform.localScale = new Vector3(bounds.size[0] / (float)width, height, bounds.size[2] / (float)length);
                        lowercube3.transform.position = new Vector3(x + 0.5f, y - height * 0.5f, z + 0.5f);
                        lowercube3.GetComponent<Renderer>().material = outerWallMaterial;
                    }
                }
            }
        }

        // Quadrant I
        int wn = 0;
        int ln = 0;
        while (true) {
            wn = Random.Range(1, middle);
            ln = Random.Range(1, middle);

            if (wn == wr && ln == lr) { // ensure player doesn't spawn on top of the npc 
                continue;
            }

            if (solution[wn, ln][0] == TileType.FLOOR) {
                float x = bounds.min[0] + (float)wn * (bounds.size[0] / (float)width);
                float z = bounds.min[2] + (float)ln * (bounds.size[2] / (float)length);
                npc1.transform.position = new Vector3(x + 0.5f, 0, z + 0.5f); 
                npc1.GetComponent<Rigidbody>().useGravity = true;
                npc1.GetComponent<Rigidbody>().mass = 1000;
                break;
            }
        }

        // Quadrant II
        while (true) {
            wn = Random.Range(middle, width - 1);
            ln = Random.Range(1, middle);

            if (wn == wr && ln == lr) {
                continue;
            }

            if (solution[wn, ln][0] == TileType.FLOOR) {

                float x = bounds.min[0] + (float)wn * (bounds.size[0] / (float)width);
                float z = bounds.min[2] + (float)ln * (bounds.size[2] / (float)length);
                npc2.transform.position = new Vector3(x + 0.5f, 0, z + 0.5f); 
                npc2.GetComponent<Rigidbody>().useGravity = true;
                break;
            }
        }

        // Quadrant III
        while (true) {
            wn = Random.Range(1, middle);
            ln = Random.Range(middle, length - 1);

            if (wn == wr && ln == lr) {
                continue;
            }

            if (solution[wn, ln][0] == TileType.FLOOR) {
                float x = bounds.min[0] + (float)wn * (bounds.size[0] / (float)width);
                float z = bounds.min[2] + (float)ln * (bounds.size[2] / (float)length);
                npc3.transform.position = new Vector3(x + 0.5f, 0, z + 0.5f); 
                npc3.GetComponent<Rigidbody>().useGravity = true;
                break;
            }
        }

        if (puzzleScript.puzzleLength == 4) { 
            //Quadrant IV
            while (true) {
                wn = Random.Range(middle, width - 1);
                ln = Random.Range(middle, length - 1);

                if (wn == wr && ln == lr) {
                    continue;
                }

                if (solution[wn, ln][0] == TileType.FLOOR) {
                    float x = bounds.min[0] + (float)wn * (bounds.size[0] / (float)width);
                    float z = bounds.min[2] + (float)ln * (bounds.size[2] / (float)length);
                    npc4.transform.position = new Vector3(x + 0.5f, 0, z + 0.5f);
                    npc4.GetComponent<Rigidbody>().useGravity = true; 
                    break;
                }
            }
        }
    }


    void Update() {
        if (solved_puzzle) {
            started = false;
            return;
        }
    }
}


   


    