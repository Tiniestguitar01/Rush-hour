using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class BoardSpawnTests
{
    SpawnGrid spawnGridInstance;
    Board boardInstance;
    PuzzleGenerator puzzleGeneratorInstance;

    private GameObject cell = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Cell.prefab");

    [SetUp]
    public void Init()
    {
        GameObject InstanceCreatorGO = new GameObject("InstanceCreator");
        GameObject GenerationManagerGO = new GameObject("GenerationManager");
        GameObject VisualManagerGO = new GameObject("VisualManager");

        InstanceCreatorGO.AddComponent<InstanceCreator>();
        GenerationManagerGO.AddComponent<Board>();
        GenerationManagerGO.AddComponent<PuzzleGenerator>();
        VisualManagerGO.AddComponent<SpawnGrid>();

        boardInstance = InstanceCreator.GetBoard();
        puzzleGeneratorInstance = InstanceCreator.GetPuzzleGenerator();
        puzzleGeneratorInstance.vehicles.Add(new Vehicle(1, 2, new int[] { 0, 2 }, Direction.Vertical, boardInstance.board));
        spawnGridInstance = InstanceCreator.GetSpawnGrid();

        spawnGridInstance.cell = cell;

        boardInstance.GenerateBoard();
    }

    [Test]
    public void VehicleSpawnTestsSimplePasses()
    {
        spawnGridInstance.Spawn();

        for (int x = 0; x < InstanceCreator.GetBoard().size; x++)
        {
            for (int z = 0; z < InstanceCreator.GetBoard().size; z++)
            {
                Assert.AreEqual(boardInstance.BoardCoordinateToWordSpace(new int[] { x, z }), spawnGridInstance.instantiatedCells[z * (x+1)].transform.position);
            }
        }

    }

}
