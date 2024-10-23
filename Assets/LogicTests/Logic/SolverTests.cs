using NUnit.Framework;
using UnityEngine;

public class SolverTests
{
    Board boardInstance;
    ModifyBoard modifyBoardInstance;
    Solver solverInstance;
    GameData gameDataInstance;

    [SetUp]
    public void Init()
    {
        boardInstance = GameObject.Find("GenerationManager").GetComponent<Board>();
        modifyBoardInstance = GameObject.Find("GenerationManager").GetComponent<ModifyBoard>();
        solverInstance = GameObject.Find("GenerationManager").GetComponent<Solver>();
        modifyBoardInstance.Start();
        boardInstance.size = 6;
        boardInstance.Start();
        solverInstance.Start();
    }

    [Test]
    public async void ShouldGetTrueForOneCar()
    {
        int[,] testBoard = new int[,] {
            { 0,0,0,0,0,0 },
            { 0,0,1,0,0,0 },
            { 0,0,1,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 }
        };

        bool result = await solverInstance.Search(testBoard, true);
        Assert.AreEqual(true, result);
    }

    [Test]
    public async void ShouldGetTrueForBlockingCar()
    {

        int[,] testBoard = new int[,] {
            { 0,0,0,0,0,0 },
            { 0,2,2,0,0,0 },
            { 0,0,1,0,0,0 },
            { 0,0,1,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 }
        };
        bool result = await solverInstance.Search(testBoard, true);
        Assert.AreEqual(true, result);
    }

    [Test]
    public async void ShouldGetFalseForCarInFrontOfTargetCar()
    {

        int[,] testBoard = new int[,] {
            { 0,0,0,0,0,0 },
            { 0,0,2,0,0,0 },
            { 0,0,2,0,0,0 },
            { 0,0,1,0,0,0 },
            { 0,0,1,0,0,0 },
            { 0,0,0,0,0,0 }
        };

        bool result = await solverInstance.Search(testBoard, true);
        Assert.AreEqual(false, result);
    }

    
    [Test]
    public async void ThinkFunMostDifficultPuzzle()
    {

        int[,] testBoard = new int[,] {
            { 12,0,0,7,7,7 },
            { 12,13,13,6,0,0 },
            { 12,10,0,6,5,5 },
            { 9,10,11,11,3,4 },
            { 9,0,1,2,3,4 },
            { 8,8,1,2,0,4 }
        };

        bool result = await solverInstance.Search(testBoard, true);
        Assert.AreEqual(true, result);
    }

    [Test]
    public async void HardestPossiblePuzzle()
    {

        int[,] testBoard = new int[,] {
            { 0,2,2,2,4,0 },
            { 3,3,1,0,4,5 },
            { 0,0,1,6,6,5 },
            { 13,10,10,9,7,7 },
            { 13,11,11,9,0,8 },
            { 12,12,12,9,0,8 }
        };

        bool result = false;
        for (int i = 0; i < 100; i++)
        {
            result = await solverInstance.Search(testBoard, true);
        } 
        Assert.AreEqual(true, result);
    }
}
