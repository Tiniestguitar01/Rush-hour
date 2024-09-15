using UnityEngine;


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

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //if clicked on other car before the previous car finished moving to its closest position
                //place the previous car to its closest point immediately
                /* if (hitted != null && moveTo != Vector3.zero)
                 {
                     hitted.transform.parent.position = new Vector3(moveTo.x, (0.5f * vehicle.size), moveTo.z);
                     hitted = null;
                 }*/


                //Getting the clicked vehicle
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, carLayer))
                {
                    hitted = hit.transform.gameObject;
                    VehicleComponent vehicleComponent = hitted.GetComponent<VehicleComponent>();
                    vehicle = new Vehicle(vehicleComponent.id, vehicleComponent.size, vehicleComponent.startPosition, vehicleComponent.direction, Board.Instance.board);

                    DirectionFromCarOrigin = hitted.transform.parent.position - hit.point;
                    startHitPoint = hit.point;
                    vehicle.GetMovablePosition(Board.Instance.board);

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
                        if (distance <= vehicle.maxDistanceBackward * 3 && distance >= vehicle.maxDistanceForward * -3)
                        {
                            hitted.transform.parent.position = new Vector3(hit.point.x + DirectionFromCarOrigin.x, hitted.transform.parent.position.y, hitted.transform.parent.position.z);
                        }
                    }
                    else
                    {
                        float distance = hit.point.z - startHitPoint.z;
                        if (distance <= vehicle.maxDistanceBackward * 3 && distance >= vehicle.maxDistanceForward * -3)
                        {
                            hitted.transform.parent.position = new Vector3(hitted.transform.parent.position.x, hitted.transform.parent.position.y, hit.point.z + DirectionFromCarOrigin.z);
                        }
                    }
                }

            }
            else if (hitted != null)
            {
                //Correction if not moved to a spot
                Vector3 moveTo = Board.Instance.BoardCoordinateToWordSpace(vehicle.possibleMoves[0]);
                float minDistance = Vector3.Distance(hitted.transform.parent.position, moveTo);

                for (int position = 0; position < vehicle.possibleMoves.Count; position++)
                {
                    Vector3 current = Board.Instance.BoardCoordinateToWordSpace(vehicle.possibleMoves[position]);
                    if (Vector3.Distance(hitted.transform.parent.position, current) < minDistance)
                    {
                        minDistance = Vector3.Distance(hitted.transform.parent.position, current);
                        moveTo = current;
                    }
                }

                hitted.transform.parent.position = Vector3.Lerp(hitted.transform.parent.position, new Vector3(moveTo.x, (0.5f * vehicle.size), moveTo.z), Vector3.Distance(hitted.transform.position.normalized, moveTo.normalized));

                ModifyBoard.Instance.MoveVehicle(vehicle, new int[] { (int)moveTo.x / 3, (int)moveTo.z / 3 }, Board.Instance.board);

                if (Vector3.Distance(hitted.transform.parent.position, moveTo) < 1.1f * vehicle.size / 2)
                {
                    hitted.transform.parent.position = new Vector3(moveTo.x, (0.5f * vehicle.size), moveTo.z);
                    if (originalPosition[0] != vehicle.startPosition[0] || originalPosition[1] != vehicle.startPosition[1])
                    {
                        GameData.Instance.moved++;
                    }
                    hitted = null;
                }
            }
        }
    }
