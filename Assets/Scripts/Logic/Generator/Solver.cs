using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Solver : MonoBehaviour
{
    public Node resultNode;
    public float solutionPlaySpeed = 1.0f;
    VehicleMovement vehicleMovementInstance;
    PuzzleGenerator puzzleGeneratorInstance;

    private void Start()
    {
        vehicleMovementInstance = InstanceCreator.GetVehicleMovement();
        puzzleGeneratorInstance = InstanceCreator.GetPuzzleGenerator();
    }

    public async Task<bool> Search(int[,] firstBoard, bool forSolution)
    {
        Graph graph = new Graph();

        Node firstNode;
        if (forSolution)
        {
            firstNode = new NodeForSolution(firstBoard, 0);
        }
        else
        {
            firstNode = new NodeToCompareBoard(firstBoard, 0, (int[,])puzzleGeneratorInstance.resultBoard.Clone());
        }

        graph.openList.Add(firstNode);
        while (graph.openList.Count != 0)
        {
            graph.openList.Sort();
            Node bestNode = graph.openList.First();

            graph.openList.RemoveAt(0);
            graph.closedList.Add(bestNode);

            List<Node> children = bestNode.GetChildren();

            for (int nodeIndex = 0; nodeIndex < children.Count; nodeIndex++)
            {

                if ((children[nodeIndex].cost - children[nodeIndex].depth) == 0 && forSolution == true)
                {
                    resultNode = children[nodeIndex];
                    return await Task.FromResult(true);
                }
                else if(children[nodeIndex].cost == 0 && forSolution == false)
                {
                    resultNode = children[nodeIndex];
                    return await Task.FromResult(true);
                }
                else
                {
                    if (!graph.openList.Any((node) => node.Equals(children[nodeIndex])) && !graph.closedList.Any((node) => node.Equals(children[nodeIndex])))
                    {
                        graph.openList.Add(children[nodeIndex]);
                    }
                }
            }
            await Task.Yield();
        }
        return await Task.FromResult(false);
    }

    public async void Solve()
    {
        await GetSteps(true);
    }

    public async void ResetBoard()
    {
        await GetSteps(false);
        InstanceCreator.GetGameData().moved = 0;
    }

    async Task<bool> GetSteps(bool forSolution)
    {
        List<Node> solution = new List<Node>();

        await Search(InstanceCreator.GetBoard().board, forSolution);

        Node node = resultNode;

        while (node != null)
        {
            solution.Add(node);
            node = node.parent;
        }

        solution.Reverse();

        StartCoroutine(Move(solution));

        return await Task.FromResult(true);
    }

    IEnumerator Move(List<Node> solution)
    {
        for (int nodeIndex = 1; nodeIndex < solution.Count; nodeIndex++)
        {
            yield return InstanceCreator.GetVehicleMovement().MoveTo(solution[nodeIndex].vehicle.id, solution[nodeIndex].vehicle.startPosition);
        }
    }
}
