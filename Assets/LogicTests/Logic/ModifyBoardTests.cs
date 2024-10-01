using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ModifyBoardTests
{
    Board boardInstance;
    ModifyBoard modifyBoardInstance;

    Vehicle vehicleVertical;
    Vehicle vehicleHorizontal;
    Vehicle vehicleSize3;

    [SetUp]
    public void Init()
    {
        boardInstance = InstanceCreator.GetBoard();
        boardInstance.size = 6;
        boardInstance.GenerateBoard();

        vehicleVertical = new Vehicle(1, 2, new int[] { 0, 0 }, Direction.Vertical, boardInstance.board);
        vehicleHorizontal = new Vehicle(2, 2, new int[] { 1, 1 }, Direction.Horizontal, boardInstance.board);
        vehicleSize3 = new Vehicle(3, 3, new int[] { 3, 2 }, Direction.Horizontal, boardInstance.board);

        modifyBoardInstance = InstanceCreator.GetModifyBoard();
        modifyBoardInstance = new ModifyBoard();
        modifyBoardInstance.InsertVehicle(vehicleVertical, boardInstance.board);
        modifyBoardInstance.InsertVehicle(vehicleHorizontal, boardInstance.board);
        modifyBoardInstance.InsertVehicle(vehicleSize3, boardInstance.board);
    }

    [Test]
    public void ShouldInsertVehicle()
    {
        int[,] testBoard = new int[,] {
            { 1,0,0,0,0,0 },
            { 1,2,2,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,3,3,3,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 }
        };

        Assert.AreEqual(testBoard, boardInstance.board);
    }

    [Test]
    public void ShouldRemoveVehicle()
    {

        int[,] testBoardBeforeDelete = new int[,] {
            { 1,0,0,0,0,0 },
            { 1,2,2,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,3,3,3,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 }
        };

        Assert.AreEqual(testBoardBeforeDelete, boardInstance.board);

        modifyBoardInstance.RemoveVehicle(vehicleVertical, boardInstance.board);

        int[,] testBoardAfterDelete = new int[,] {
            { 0,0,0,0,0,0 },
            { 0,2,2,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,3,3,3,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 }
        };

        Assert.AreEqual(testBoardAfterDelete, boardInstance.board);
    }

    [Test]
    public void ShouldMoveVehicle()
    {

        int[,] testBoardBeforeMove = new int[,] {
            { 1,0,0,0,0,0 },
            { 1,2,2,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,3,3,3,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 }
        };

        Assert.AreEqual(testBoardBeforeMove, boardInstance.board);

        modifyBoardInstance.MoveVehicle(vehicleVertical,new int[]{ 1, 0 }, boardInstance.board, true);

        int[,] testBoardAfterMove = new int[,] {
            { 0,0,0,0,0,0 },
            { 1,2,2,0,0,0 },
            { 1,0,0,0,0,0 },
            { 0,0,3,3,3,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 }
        };

        Assert.AreEqual(testBoardAfterMove, boardInstance.board);
    }
}
