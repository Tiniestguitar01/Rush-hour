using System.Collections.Generic;
using UnityEngine;

    public class ModifyBoard : MonoBehaviour
    {
        public static ModifyBoard Instance;

        void Awake()
        {
            Instance = this;
        }

        public void InsertVehicle(Vehicle vehicle, int[,] board)
        {
            List<int[]> position = vehicle.GetPosition();

            for (int x = 0; x < position.Count; x++)
            {
                board[position[x][0], position[x][1]] = vehicle.id;
            }
        }

        public void RemoveVehicle(Vehicle vehicle, int[,] board)
        {
            List<int[]> position = vehicle.GetPosition();

            for (int x = 0; x < position.Count; x++)
            {
                board[position[x][0], position[x][1]] = 0;
            }
        }

        public void MoveVehicle(Vehicle vehicle, int[] position, int[,] board)
        {
            RemoveVehicle(vehicle, board);
            vehicle.Move(position);
            InsertVehicle(vehicle, board);
        }

    }
