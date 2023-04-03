using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script is going to take in a prefab game object (grid node or similar) and spawn them
 * In a regular square grid. The script will allow us to modify the width and height in the Unity Editor
 */


public class Grid_Generator : MonoBehaviour
{
    [SerializeField] GameObject gridCellPrefab;

    [SerializeField] int gridWidth;
    [SerializeField] int gridHeight;



    // Start is called before the first frame update
    void Start()
    {
        //spawn in grid which is he specified width and height
        GenerateGrid(gridWidth, gridHeight);
    }

    private void GenerateGrid(int _width, int _height)
    {
        GameObject newGridCell;

        GridCell rootGridCell;

        bool rootGridCellSet = false; 

        for(int x = 0; x < _width; x++)
        {
            for(int y = 0; y < _height; y++)
            {
                newGridCell = Instantiate(gridCellPrefab, new Vector3(x, 1, y), gameObject.transform.rotation, gameObject.transform);
                newGridCell.name = "Gridcell " + x.ToString() + "," + y.ToString();

                if (!rootGridCellSet)
                {
                    //set this new gridcell instance as the root node in the BFS algorithm
                    if (newGridCell.TryGetComponent<GridCell>(out rootGridCell))
                    {
                        BreadthFirstSearch.setRootCellEvent(rootGridCell);
                        rootGridCellSet = true;
                    }
                }
            }
        }

    }
   
}
