using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NodeTests
{
    Board boardInstance;
    ModifyBoard modifyBoardInstance;

    [SetUp]
    public void Init()
    {
        boardInstance = GameObject.Find("GenerationManager").GetComponent<Board>();
        modifyBoardInstance = GameObject.Find("GenerationManager").GetComponent<ModifyBoard>();
        boardInstance.size = 6;
        boardInstance.Start();
    }

    [Test]
    public void ShouldGetChildren()
    {

        int[,] testBoard = new int[,] {
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,1,0,0,0 },
            { 0,0,1,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,2,2,0,0,0 }
        };

        Node node = new NodeForSolution(testBoard, 0);
        //List<Node> children = node.GetChildren();

        List<Node> childrenTest = new List<Node>();

        int[,] firstMove = new int[,] {
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,1,0,0,0 },
            { 0,0,1,0,0,0 },
            { 0,2,2,0,0,0 }
        };

        int[,] secondMove = new int[,] {
            { 0,0,0,0,0,0 },
            { 0,0,1,0,0,0 },
            { 0,0,1,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,2,2,0,0,0 }
        };

        int[,] thirdMove = new int[,] {
            { 0,0,1,0,0,0 },
            { 0,0,1,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,2,2,0,0,0 }
        };

        childrenTest.AddRange(new List<Node> { new NodeForSolution(firstMove, 0), new NodeForSolution(secondMove, 0), new NodeForSolution(thirdMove, 0) });

        //Assert.AreEqual(childrenTest[0].board, children[0].board);
        //Assert.AreEqual(childrenTest[1].board, children[1].board);
        //Assert.AreEqual(childrenTest[2].board, children[2].board);
    }

    [Test]
    public void ShouldGetVehicles()
    {

        int[,] testBoard = new int[,] {
            { 0,0,3,3,3,0 },
            { 0,0,0,0,0,0 },
            { 0,0,1,0,0,0 },
            { 0,0,1,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,2,2,0,0,0 }
        };

        Node node = new NodeForSolution(testBoard, 0);

       // List<Vehicle> vehicles = node.GetVehicles();

        List<Vehicle> vehiclesTest = new List<Vehicle>();

        Vehicle vehicle1 = new Vehicle(1, 2, new int[] { 2, 2 }, Direction.Vertical, testBoard);
        Vehicle vehicle2 = new Vehicle(2, 2, new int[] { 5, 1 }, Direction.Horizontal, testBoard);
        Vehicle vehicle3 = new Vehicle(3, 3, new int[] { 0, 2 }, Direction.Horizontal, testBoard);

        vehiclesTest.AddRange(new List<Vehicle> { vehicle3, vehicle1, vehicle2});

        //Assert.AreEqual(vehiclesTest[0].id, vehicles[0].id);
        //Assert.AreEqual(vehiclesTest[1].direction, vehicles[1].direction);
        //Assert.AreEqual(vehiclesTest[2].size, vehicles[2].size);
    }

    [Test]
    public void ShouldCreateChild()
    {

        int[,] testBoard = new int[,] {
            { 0,0,3,3,3,0 },
            { 0,0,0,0,0,0 },
            { 0,0,1,0,0,0 },
            { 0,0,1,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,2,2,0,0,0 }
        };

        Node node = new NodeForSolution(testBoard, 0);

        Vehicle vehicle1 = new Vehicle(1, 2, new int[] { 2, 2 }, Direction.Vertical, testBoard);

        Node child = node.CreateChild(vehicle1, new int[] { 3, 2 }, testBoard);

        int[,] firstMove = new int[,] {
            { 0,0,3,3,3,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,1,0,0,0 },
            { 0,0,1,0,0,0 },
            { 0,2,2,0,0,0 }
        };

        Node resultNode = new NodeForSolution(firstMove, 0);

        Assert.AreEqual(resultNode.board, child.board);
        Assert.AreEqual(10, child.cost);
    }
}
