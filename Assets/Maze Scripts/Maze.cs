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
    public int middle = 12;
    public float height = 2.5f;
    public GameObject npc1;
    public GameObject npc2;
    public GameObject npc3;
    public GameObject npc4;
    public GameObject fps_player_obj;
    internal bool solved_puzzle = false;

    private Bounds bounds;
    private float timestamp_last_msg = 0.0f;
    private int function_calls = 0;
    private bool started = true;
    public Material wallMaterial;
    // public NavMeshSurface navMesh;

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
            if (!success)
            {
                Debug.Log("Could not find valid solution - will try again");
                unassigned.Clear();
                grid = new List<TileType>[width, length];
                function_calls = 0; 
            }
        }

        DrawMaze(grid);
        // NavMesh.BuildNavMesh();
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

        if ((number_of_assigned_elements[(int)TileType.WALL] > 65))
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

        if ((number_of_potential_assignments[(int)TileType.WALL] < 24))
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
                    cube.transform.position = new Vector3(x + 0.5f, y + height / 2.0f, z + 0.5f);
                    cube.GetComponent<Renderer>().material = wallMaterial;

                    if (w == middle && l == middle) { 
                        //make center puzzle location unique
                        cube.transform.position = new Vector3(x + 0.5f, y - 0.25f * height, z + 0.5f);
                        // put a puzzle on top of this short wall?
                    }

                    if (w == 0 || l == 0 || w == width - 1 || l == length - 1) {
                        // add additional layer to outer walls
                        GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube2.name = "WALL";
                        cube2.transform.localScale = new Vector3(bounds.size[0] / (float)width, height, bounds.size[2] / (float)length);
                        cube2.transform.position = new Vector3(x + 0.5f, y + height * 1.5f, z + 0.5f);
                        cube2.GetComponent<Renderer>().material = wallMaterial;

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


    void Update() {
        if (solved_puzzle) {
            started = false;
            return;
        }
    }
}


   


    