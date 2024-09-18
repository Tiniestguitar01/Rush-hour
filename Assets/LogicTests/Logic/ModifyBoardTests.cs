using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ModifyBoardTests
{
    Vehicle vehicleVertical;
    Vehicle vehicleHorizontal;
    Vehicle vehicleSize3;

    [Test]
    public void ShouldInsertVehicle()
    {
        Init();

        int[,] testBoard = new int[,] {
            { 1,0,0,0,0,0 },
            { 1,2,2,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,3,3,3,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 }
        };

        Assert.AreEqual(testBoard, Board.Instance.board);
    }

    [Test]
    public void ShouldRemoveVehicle()
    {
        Init();

        int[,] testBoardBeforeDelete = new int[,] {
            { 1,0,0,0,0,0 },
            { 1,2,2,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,3,3,3,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 }
        };

        Assert.AreEqual(testBoardBeforeDelete, Board.Instance.board);

        ModifyBoard.Instance.RemoveVehicle(vehicleVertical, Board.Instance.board);

        int[,] testBoardAfterDelete = new int[,] {
            { 0,0,0,0,0,0 },
            { 0,2,2,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,3,3,3,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 }
        };

        Assert.AreEqual(testBoardAfterDelete, Board.Instance.board);
    }

    [Test]
    public void ShouldMoveVehicle()
    {
        Init();

        int[,] testBoardBeforeMove = new int[,] {
            { 1,0,0,0,0,0 },
            { 1,2,2,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,3,3,3,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 }
        };

        Assert.AreEqual(testBoardBeforeMove, Board.Instance.board);

        ModifyBoard.Instance.MoveVehicle(vehicleVertical,new int[]{ 1, 0 }, Board.Instance.board);

        int[,] testBoardAfterMove = new int[,] {
            { 0,0,0,0,0,0 },
            { 1,2,2,0,0,0 },
            { 1,0,0,0,0,0 },
            { 0,0,3,3,3,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 }
        };

        Assert.AreEqual(testBoardAfterMove, Board.Instance.board);
    }

    public void Init()
    {
        Board.Instance = new Board();
        Board.Instance.size = 6;
        Board.Instance.GenerateBoard();

        vehicleVertical = new Vehicle(1, 2, new int[] { 0, 0 }, Direction.Vertical, Board.Instance.board);
        vehicleHorizontal = new Vehicle(2, 2, new int[] { 1, 1 }, Direction.Horizontal, Board.Instance.board);
        vehicleSize3 = new Vehicle(3, 3, new int[] { 3, 2 }, Direction.Horizontal, Board.Instance.board);

        ModifyBoard.Instance = new ModifyBoard();
        ModifyBoard.Instance.InsertVehicle(vehicleVertical, Board.Instance.board);
        ModifyBoard.Instance.InsertVehicle(vehicleHorizontal, Board.Instance.board);
        ModifyBoard.Instance.InsertVehicle(vehicleSize3, Board.Instance.board);
    }
}
