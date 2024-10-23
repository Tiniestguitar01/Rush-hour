using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.TestTools;

public class PuzzleGeneratorTests
{
    Board boardInstance;
    PuzzleGenerator puzzleGeneratorInstance;
    ModifyBoard modifyBoardInstance;
    SpawnVehicles spawnVehicleInstance;

    [SetUp]
    public void Init()
    {
        boardInstance = GameObject.Find("GenerationManager").GetComponent<Board>();
        modifyBoardInstance = GameObject.Find("GenerationManager").GetComponent<ModifyBoard>();
        modifyBoardInstance.Start();
        boardInstance.size = 6;
        boardInstance.Start();
        puzzleGeneratorInstance = GameObject.Find("GenerationManager").GetComponent<PuzzleGenerator>();
        puzzleGeneratorInstance.Start();
        spawnVehicleInstance = GameObject.Find("VisualManager").GetComponent<SpawnVehicles>();
    }

    [Test]
    public void ShouldCreateVehicle()
    {
        puzzleGeneratorInstance.boardInstance = boardInstance;
        Vehicle vehicle = puzzleGeneratorInstance.CreateVehicle(1, 2, new int[] { 0, 2 }, Direction.Vertical);

        Assert.AreEqual(1, vehicle.id);
        Assert.AreEqual(2, vehicle.size);
        Assert.AreEqual(new int[] { 0, 2 }, vehicle.startPosition);
        Assert.AreEqual(Direction.Vertical, vehicle.direction);
    }


    [Test]
    public async void ShouldInsertVehicleToBoard()
    {
        Vehicle vehicle = puzzleGeneratorInstance.CreateVehicle(1, 2, new int[] { 0, 2 }, Direction.Vertical);
        bool result = await puzzleGeneratorInstance.InsertVehicle(vehicle, boardInstance.board);

        int[,] testBoard = new int[,] {
            { 0,0,1,0,0,0 },
            { 0,0,1,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 }
        };

        Assert.AreEqual(testBoard, boardInstance.board);
        Assert.AreEqual(true, result);
        Assert.AreEqual(1, puzzleGeneratorInstance.vehicles.Count);

        Assert.AreEqual(1, puzzleGeneratorInstance.vehicles[0].id);
        Assert.AreEqual(2, puzzleGeneratorInstance.vehicles[0].size);
        Assert.AreEqual(new int[] { 0, 2 }, puzzleGeneratorInstance.vehicles[0].startPosition);
        Assert.AreEqual(Direction.Vertical, puzzleGeneratorInstance.vehicles[0].direction);
        Assert.AreEqual(1, puzzleGeneratorInstance.carCount);

        for (int i = 0; i < boardInstance.places.Count; i++)
        {
            Assert.AreNotEqual(new int[] { 0, 2 }, boardInstance.places[i].placePosition);
            Assert.AreNotEqual(new int[] { 1, 2 }, boardInstance.places[i].placePosition);
        }
    }

    [Test]
    public async void ShouldDeleteVehicles()
    {
        ShouldInsertVehicleToBoard();

        puzzleGeneratorInstance.DeleteVehicles();

        Assert.AreEqual(0, puzzleGeneratorInstance.vehicles.Count);
        Assert.AreEqual(0, spawnVehicleInstance.vehicleGOs.Count);
    }
}
