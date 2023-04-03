using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script will be attached to GridCell prefab object
 * Will contain methods for changing the colour when mouse goes over it
 * Automatically setting up neighbours etc etc
 */

public class GridCell : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    [SerializeField] Material greenMat;
    [SerializeField] Material redMat;
    [SerializeField] Material blueMat;

    public List<GridCell> neighbours = new List<GridCell>();

    [SerializeField] List<Vector3> neighbourDirections = new List<Vector3>();

    public GridCell pathCell { get; private set; }

    private bool activeGridCell;

    private void Awake()
    {
        activeGridCell = false;

        if(!TryGetComponent<MeshRenderer>(out meshRenderer))
        {
            Debug.LogError("You need to attach a MeshRenderer to this object dumass");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer.material = greenMat;

        //find neighbouring gridcells
        FindNeighbours();

        BreadthFirstSearch.breadthFirstSearchEvent += ClearPathCell;
        BreadthFirstSearch.clearColourFromCellsEvent += ResetColour;
    }

    private void OnDestroy()
    {
        BreadthFirstSearch.breadthFirstSearchEvent -= ClearPathCell;
        BreadthFirstSearch.clearColourFromCellsEvent -= ResetColour;
    }

    // Update is called once per frame
    void Update()
    {
        if (activeGridCell)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Selected " + gameObject.name + " as the destination");
                BreadthFirstSearch.setDestCellEvent(this);
            }
        }
    }

    private void FindNeighbours()
    {
        RaycastHit hit;
        GridCell gridCell;

        for(int i = 0; i < neighbourDirections.Count; i++)
        {
            if (Physics.Raycast(transform.position, neighbourDirections[i], out hit, 2f))
            {
                if (hit.collider.TryGetComponent<GridCell>(out gridCell))
                {
                    neighbours.Add(gridCell);
                }
            }
        }
    }

    private void ClearPathCell()
    {
        pathCell = null;
    }

    public void SetPathCell(GridCell _pathCell)
    {
        pathCell = _pathCell;
    }

    private void ResetColour()
    {
        meshRenderer.material = greenMat;
    }

    public void SetColour(int _choice)
    {
        if (_choice == 0)
        {
            meshRenderer.material = greenMat;
        }
        else if (_choice == 1)
        {
            meshRenderer.material = redMat;
        }
        else if (_choice == 2)
        {
            meshRenderer.material = blueMat;
        }
    }

    private void OnMouseEnter()
    {
        meshRenderer.material = blueMat;
        activeGridCell = true;
    }

    private void OnMouseExit()
    {
        meshRenderer.material = greenMat;
        activeGridCell = false;
    }
}
