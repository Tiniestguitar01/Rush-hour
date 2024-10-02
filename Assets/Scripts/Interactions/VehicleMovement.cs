using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;


public class VehicleMovement : MonoBehaviour
{
    GameObject hitted;
    Vehicle vehicle;

    Vector3 DirectionFromCarOrigin;
    Vector3 startHitPoint;
    Vector3 moveTo;

    public LayerMask carLayer;
    public LayerMask exceptCarLayer;

    int[] originalPosition;

    Board boardInstance;
    GameData gameDataInstance;
    SpawnGrid spawnGridInstance;
    SpawnVehicles spawnVehicleInstance;

    float moveDuration = 0.5f;
    float minDistanceFromClosestCell = 1.1f;

    void Start()
    {
        spawnGridInstance = InstanceCreator.GetSpawnGrid();
        boardInstance = InstanceCreator.GetBoard();
        gameDataInstance = InstanceCreator.GetGameData();
        spawnVehicleInstance = InstanceCreator.GetSpawnVehicles();
    }

    void Update()
    {
        if (InstanceCreator.GetUIManager().state == (int)Menu.Game)
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

            }
            else if (hitted != null)
            {
                //Correction if not moved to a spot
                Vector3 moveTo = boardInstance.BoardCoordinateToWordSpace(vehicle.possibleMoves[0]);
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

                hitted.transform.parent.position = Vector3.Lerp(hitted.transform.parent.position, new Vector3(moveTo.x, (spawnVehicleInstance.vehicleYOffset * vehicle.size), moveTo.z), Vector3.Distance(hitted.transform.parent.position, moveTo));

                InstanceCreator.GetModifyBoard().MoveVehicle(vehicle, new int[] { (int)((moveTo.x - ((boardInstance.maxBoardSize - boardInstance.size) * spawnGridInstance.offset)) / spawnGridInstance.distance), (int)((moveTo.z - ((boardInstance.maxBoardSize - boardInstance.size) * spawnGridInstance.offset)) / spawnGridInstance.distance) }, boardInstance.board,false);

                if (Vector3.Distance(hitted.transform.parent.position, moveTo) < minDistanceFromClosestCell * vehicle.size / 2)
                {
                    hitted.transform.parent.position = new Vector3(moveTo.x, (spawnVehicleInstance.vehicleYOffset * vehicle.size), moveTo.z);
                    if (originalPosition[0] != vehicle.startPosition[0] || originalPosition[1] != vehicle.startPosition[1])
                    {
                        gameDataInstance.moved++;
                    }
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

        InstanceCreator.GetModifyBoard().MoveVehicle(vehicle, new int[] { (int)(moveTo.x / spawnGridInstance.distance), (int)(moveTo.z / spawnGridInstance.distance) }, boardInstance.board, false);

        vehicleToMove.transform.position = new Vector3(moveTo.x, (spawnVehicleInstance.vehicleYOffset * vehicle.size), moveTo.z);
    }
}
