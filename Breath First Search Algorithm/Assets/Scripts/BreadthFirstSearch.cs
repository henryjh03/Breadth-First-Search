using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script will contain the main algorithm for generating a path through the graph of gridcells
 * using the Breadth First Seach Algorithm
 */

public class BreadthFirstSearch : MonoBehaviour
{
    [SerializeField] GridCell rootCell;

    [SerializeField] GridCell destCell;

    List<GridCell> queue = new List<GridCell>();

    [SerializeField] List<GridCell> gridCellPath = new List<GridCell>();

    GridCell currentGridCell;

    bool searching = false;

    bool destFound = false;

    bool pathMade = false;

    //events & delegates
    public delegate void SetRootCellDelegate(GridCell _gridCell);
    public static SetRootCellDelegate setRootCellEvent = delegate { };

    public delegate void SetDestCellDelegate(GridCell _gridCell);
    public static SetDestCellDelegate setDestCellEvent = delegate { };

    public delegate void BreadthFirstSearchDelegate();
    public static BreadthFirstSearchDelegate breadthFirstSearchEvent = delegate { };

    public delegate void ClearColourFromCellsDelegate();
    public static ClearColourFromCellsDelegate clearColourFromCellsEvent = delegate { };

    private void OnEnable()
    {
        //subscribe to events
        setDestCellEvent += SetDestCell;
        setRootCellEvent += SetRootCell;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && searching == false)
        {
            if(destCell != null)
            {
                searching = true;
                gridCellPath.Clear();
                //Invoke the clear colour from nodes event
                
                breadthFirstSearchEvent.Invoke();
                clearColourFromCellsEvent.Invoke();
                
                queue.Clear();
                queue.Add(rootCell);
                
                currentGridCell = queue[0];

                destFound = false;

                pathMade = false; 
            }
        }

        if (searching == true)
        {
            if (destFound == false)
            {
                if (currentGridCell == destCell)
                {
                    Debug.Log("Found the destination YIPEE");
                    //clear colour from node event
                    destFound = true;
                }
                else
                {
                    for (int i = 0; i < currentGridCell.neighbours.Count; i++)
                    {
                        if (currentGridCell.neighbours[i].pathCell == null)
                        {
                            //set the current node as the 'parent' of this neighbour
                            currentGridCell.neighbours[i].SetPathCell(currentGridCell);
                        }
                        if (!queue.Contains(currentGridCell.neighbours[i]))
                        {
                            queue.Add(currentGridCell.neighbours[i]);
                        }
                    }
                    queue.Remove(currentGridCell);
                }

                if (queue.Count > 0)
                {
                    currentGridCell = queue[0]; //currentGridCell = stack.Last;
                    currentGridCell.SetColour(2);
                }
                else
                {
                    Debug.Log("Confucius: There is no path");
                    searching = false;
                }
            }
            else
            {
                //return path version: draw out the path and create path list
                if (!pathMade)
                {
                    if(currentGridCell == rootCell)
                    {
                        gridCellPath.Add(rootCell);
                        rootCell.SetColour(1);
                        pathMade = true;
                        searching = false;
                        Debug.Log("Finished making the path :)");
                    }
                    else
                    {
                        if (gridCellPath.Contains(currentGridCell))
                        {
                            gridCellPath.Insert(0, currentGridCell);
                        }
                        currentGridCell.SetColour(1);
                        currentGridCell = currentGridCell.pathCell;
                    }
                }
                //non-returning path version: assign the 'currentGridCell' as 'destination' for AI using this algorithm

            }
        }
    }

    private void SetDestCell(GridCell _gridCell)
    {
        destCell = _gridCell;
    }

    private void SetRootCell(GridCell _gridCell)
    {
        rootCell = _gridCell;
    }

    private void OnDestroy()
    {
        //subscribe to events
        setDestCellEvent -= SetDestCell;
        setRootCellEvent -= SetRootCell;
    }
}
