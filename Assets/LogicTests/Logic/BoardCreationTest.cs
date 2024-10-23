using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BoardCreationTest
{
    Board boardInstance;

    [SetUp]
    public void Init()
    {
        boardInstance = GameObject.Find("GenerationManager").GetComponent<Board>();
        boardInstance.size = 6;
        boardInstance.Start();
    }

    [Test]
    public void ShouldCreateEmptyBoard()
    {

        int[,] testBoard = new int[,] {
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 }
        };

        Assert.AreEqual(testBoard, boardInstance.board);
    }

    [Test]
    public void ShouldResultInBoardCreationError()
    {

        int[,] testBoard = new int[,] {
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,1,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 }
        };

        Assert.AreNotEqual(testBoard, boardInstance.board);
    }

    [Test]
    public void ShouldConvertCoordinate()
    {
        Assert.AreEqual(new Vector3(5.5f, 0f, 5.5f), boardInstance.BoardCoordinateToWordSpace(new int[] {1, 1}));
    }
}
