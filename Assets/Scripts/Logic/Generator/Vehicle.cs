using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Vertical,
    Horizontal
}

public class Vehicle
{
        public int id;
        public int size;
        public int[] startPosition;
        public Direction direction;

        public int maxDistanceForward;
        public int maxDistanceBackward;

        public List<int[]> possibleMoves;

        public Vehicle(int id, int size, int[] startPosition, Direction direction, int[,] board)
        {
            this.id = id;
            this.size = size;
            this.startPosition = startPosition;
            this.direction = direction;
            possibleMoves = new List<int[]>();
            GetMovablePosition(board);
        }

        public List<int[]> GetPosition()
        {

            List<int[]> position = new List<int[]>();

            for (int coordinate = 0; coordinate < this.size; coordinate++)
            {
                position.Add(new int[] { startPosition[0] + (direction == Direction.Vertical ? coordinate : 0), startPosition[1] + (direction == Direction.Horizontal ? coordinate : 0) });
            }

            return position;
        }

    public void GetMovablePosition(int[,] board)
    {
        possibleMoves = new List<int[]>();

        possibleMoves.Add(startPosition);

        int size = InstanceCreator.GetBoard().size;

        maxDistanceForward = 0;
        maxDistanceBackward = 0;

        if (this.direction == Direction.Vertical)
        {
            for (int coordinate = startPosition[0] + this.size; coordinate < size; coordinate++)
            {
                if (board[coordinate, startPosition[1]] == 0)
                {
                    possibleMoves.Add(new int[] { coordinate - this.size + 1, startPosition[1] });
                    maxDistanceBackward++;
                }
                else
                {
                    break;
                }
            }

            for (int coordinate = startPosition[0] - 1; coordinate >= 0; coordinate--)
            {
                if (board[coordinate, startPosition[1]] == 0)
                {
                    possibleMoves.Add(new int[] { coordinate, startPosition[1] });
                    maxDistanceForward++;
                }
                else
                {
                    break;
                }
            }
        }
        else
        {
            for (int coordinate = startPosition[1] + this.size; coordinate < size; coordinate++)
            {
                if (board[startPosition[0], coordinate] == 0)
                { 
                    possibleMoves.Add(new int[] { startPosition[0] , coordinate - this.size + 1});
                    maxDistanceBackward++;
                }
                else
                {
                    break;
                }
            }

            for (int coordinate = startPosition[1] - 1; coordinate >= 0; coordinate--)
            {
                if (board[startPosition[0], coordinate] == 0)
                {
                    possibleMoves.Add(new int[] { startPosition[0], coordinate });
                    maxDistanceForward++;
                }
                else
                {
                    break;
                }
            }
        }
    }

    public void Move(int[] position)
    {
        startPosition[0] = position[0];
        startPosition[1] = position[1];
    }

    public override string ToString()
    {
        return "Vehicle: {Id: " + id + ", Size: " + size + ", Startposition: {" + startPosition[0] + "," + startPosition[1] + "}, Direction: " + direction + ", backSpace: " + maxDistanceBackward + ", frontSpace: " + maxDistanceForward + "}";
    }
}