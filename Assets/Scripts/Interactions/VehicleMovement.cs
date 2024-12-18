using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VehicleMovement : MonoBehaviour
{
    GameObject hitted;
    Vehicle vehicle;

    Vector3 DirectionFromCarOrigin;
    Vector3 startHitPoint;
    Vector3 moveTo;
    List<GameObject> outlinedCells;

    public LayerMask carLayer;
    public LayerMask exceptCarLayer;

    int[] originalPosition;

    Board boardInstance;
    GameData gameDataInstance;
    SpawnGrid spawnGridInstance;
    SpawnVehicles spawnVehicleInstance;

    float moveDuration = 0.5f;
    float minDistanceFromClosestCell = 2f; // 1.1f

    void Start()
    {
        spawnGridInstance = InstanceCreator.GetSpawnGrid();
        boardInstance = InstanceCreator.GetBoard();
        gameDataInstance = InstanceCreator.GetGameData();
        spawnVehicleInstance = InstanceCreator.GetSpawnVehicles();
    }

    void Update()
    {
        if (gameDataInstance.state == (int)Menu.Game)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Getting the clicked vehicle
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, carLayer))
                {
                    hitted = hit.transform.gameObject;
                    VehicleComponent vehicleComponent = hitted.GetComponent<VehicleComponent>();
                    vehicle = new Vehicle(vehicleComponent.id, vehicleComponent.size, vehicleComponent.startPosition, vehicleComponent.direction, boardInstance.board);

                    DirectionFromCarOrigin = hitted.transform.parent.position - hit.point;
                    startHitPoint = hit.point;
                    vehicle.GetMovablePosition(boardInstance.board);
                    originalPosition = (int[])vehicle.startPosition.Clone();

                    outlinedCells = new List<GameObject>();

                    moveTo = boardInstance.BoardCoordinateToWordSpace(vehicle.possibleMoves[0]);
                    GetOutlineCells();
                    SetOutline();

                    hitted.GetComponent<Outline>().enabled = true;
                }
            }
            else if (hitted != null && (Input.GetMouseButton(0)))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, exceptCarLayer))
                {
                    if (vehicle.direction == Direction.Vertical)
                    {
                        float distance = hit.point.x - startHitPoint.x;
                        if (distance <= vehicle.maxDistanceBackward * spawnGridInstance.distance && distance >= vehicle.maxDistanceForward * -spawnGridInstance.distance)
                        {
                            hitted.transform.parent.position = new Vector3(hit.point.x + DirectionFromCarOrigin.x, hitted.transform.parent.position.y, hitted.transform.parent.position.z);
                        }
                    }
                    else
                    {
                        float distance = hit.point.z - startHitPoint.z;
                        if (distance <= vehicle.maxDistanceBackward * spawnGridInstance.distance && distance >= vehicle.maxDistanceForward * -spawnGridInstance.distance)
                        {
                            hitted.transform.parent.position = new Vector3(hitted.transform.parent.position.x, hitted.transform.parent.position.y, hit.point.z + DirectionFromCarOrigin.z);
                        }
                    }
                }

                if (Vector3.Distance(hitted.transform.parent.position, moveTo) > minDistanceFromClosestCell * vehicle.size / 2)
                {
                    GetClosestCell();
                }

            }
            else if (hitted != null)
            {
                foreach (GameObject cell in outlinedCells)
                {
                    cell.GetComponent<Outline>().enabled = false;
                }

                hitted.transform.parent.position = Vector3.Lerp(hitted.transform.parent.position, new Vector3(moveTo.x, (spawnVehicleInstance.vehicleYOffset * vehicle.size), moveTo.z), Vector3.Distance(hitted.transform.parent.position, moveTo));

                InstanceCreator.GetModifyBoard().MoveVehicle(vehicle, new int[] { (int)((moveTo.x - ((boardInstance.maxBoardSize - boardInstance.size) * spawnGridInstance.offset)) / spawnGridInstance.distance), (int)((moveTo.z - ((boardInstance.maxBoardSize - boardInstance.size) * spawnGridInstance.offset)) / spawnGridInstance.distance) }, boardInstance.board,false);

                if (Vector3.Distance(hitted.transform.parent.position, moveTo) < minDistanceFromClosestCell * vehicle.size / 2)
                {
                    hitted.transform.parent.position = new Vector3(moveTo.x, (spawnVehicleInstance.vehicleYOffset * vehicle.size), moveTo.z);
                    if (originalPosition[0] != vehicle.startPosition[0] || originalPosition[1] != vehicle.startPosition[1])
                    {
                        gameDataInstance.moved++;
                    }
                    hitted.GetComponent<Outline>().enabled = false;
                    hitted = null;
                }
            }
        }
            
    }

    public IEnumerator MoveTo(int vehicleId, int[] position)
    {
        GameObject vehicleToMove = InstanceCreator.GetSpawnVehicles().vehicleGOs[vehicleId - 1];
        Vector3 moveTo = boardInstance.BoardCoordinateToWordSpace(position);
        Vehicle vehicle = InstanceCreator.GetPuzzleGenerator().vehicles[vehicleId - 1];

        float timeElapsed = 0f;

        while (timeElapsed < moveDuration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / moveDuration;
            vehicleToMove.transform.position = Vector3.Lerp(vehicleToMove.transform.position, new Vector3(moveTo.x, (spawnVehicleInstance.vehicleYOffset * vehicle.size), moveTo.z), t);
            yield return null;
        }

        InstanceCreator.GetModifyBoard().MoveVehicle(vehicle, new int[] { (int)((moveTo.x - ((boardInstance.maxBoardSize - boardInstance.size) * spawnGridInstance.offset)) / spawnGridInstance.distance), (int)((moveTo.z - ((boardInstance.maxBoardSize - boardInstance.size) * spawnGridInstance.offset)) / spawnGridInstance.distance) }, boardInstance.board, true);

        vehicleToMove.transform.position = new Vector3(moveTo.x, (spawnVehicleInstance.vehicleYOffset * vehicle.size), moveTo.z);
    }

    public void GetClosestCell()
    {
        if (Vector3.Distance(hitted.transform.parent.position, moveTo) > minDistanceFromClosestCell)
        {
            float minDistance = Vector3.Distance(hitted.transform.parent.position, moveTo);

            for (int pos = 0; pos < vehicle.possibleMoves.Count; pos++)
            {
                Vector3 current = boardInstance.BoardCoordinateToWordSpace(vehicle.possibleMoves[pos]);
                if (Vector3.Distance(hitted.transform.parent.position, current) < minDistance)
                {
                    minDistance = Vector3.Distance(hitted.transform.parent.position, current);
                    moveTo = current;
                }
            }
            SetOutline();
        }
    }

    public void GetOutlineCells()
    {
        foreach (int[] position in vehicle.possibleMoves)
        {
            foreach (GameObject cell in spawnGridInstance.instantiatedCells)
            {
                if (cell.transform.position == boardInstance.BoardCoordinateToWordSpace(position))
                {
                    outlinedCells.Add(cell);
                }
            }
        }

        int[] backSpace;
        if(vehicle.maxDistanceBackward == 0)
        {
            backSpace = vehicle.possibleMoves[0];
        }
        else
        {
            backSpace = vehicle.possibleMoves[vehicle.possibleMoves.Count - 1];
        }

        for (int i = 0; i < vehicle.size; i++)
        {
            foreach (GameObject cell in spawnGridInstance.instantiatedCells)
            {
                if (vehicle.direction == Direction.Vertical && cell.transform.position == boardInstance.BoardCoordinateToWordSpace(new int[] { backSpace[0] + i, backSpace[1] }))
                {
                    outlinedCells.Add(cell);
                }
                else if (vehicle.direction == Direction.Horizontal && cell.transform.position == boardInstance.BoardCoordinateToWordSpace(new int[] { backSpace[0], backSpace[1] + i }))
                {
                    outlinedCells.Add(cell);
                }
            }
        }
    }

    public void SetOutline()
    {
        foreach (GameObject cell in outlinedCells)
        {
            if (cell.transform.position == moveTo)
            {
                cell.GetComponent<Outline>().OutlineWidth = 10;
            }
            else
            {
                cell.GetComponent<Outline>().OutlineWidth = 3;
            }
            cell.GetComponent<Outline>().enabled = true;
        }


        for (int i = 0; i < vehicle.size; i++)
        {
            foreach (GameObject cell in outlinedCells)
            {
                if (vehicle.direction == Direction.Vertical && cell.transform.position == new Vector3(moveTo.x + spawnGridInstance.distance * i, moveTo.y, moveTo.z))
                {
                    cell.GetComponent<Outline>().OutlineWidth = 10;
                }
                else if (vehicle.direction == Direction.Horizontal && cell.transform.position == new Vector3(moveTo.x, moveTo.y, moveTo.z + spawnGridInstance.distance * i))
                {
                    cell.GetComponent<Outline>().OutlineWidth = 10;
                }
            }
        }
    }
}
