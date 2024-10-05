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
    UIManager uiManagerInstance;

    private void Start()
    {
        vehicleMovementInstance = InstanceCreator.GetVehicleMovement();
        puzzleGeneratorInstance = InstanceCreator.GetPuzzleGenerator();
        uiManagerInstance = InstanceCreator.GetUIManager();
    }

    public async Task<bool> Search(int[,] firstBoard, bool forSolution)
    {
        uiManagerInstance.SetMenuActive(Menu.Loading);

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

        int steps = 0;
        while (graph.openList.Count != 0 && steps < 100)
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
                    uiManagerInstance.SetMenuActive(Menu.Game);
                    return await Task.FromResult(true);
                }
                else if(children[nodeIndex].cost == 0 && forSolution == false)
                {
                    resultNode = children[nodeIndex];
                    uiManagerInstance.SetMenuActive(Menu.Game);
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
            steps++;
            //Debug.Log("solver: "+ steps);
            await Task.Yield();
        }
        uiManagerInstance.SetMenuActive(Menu.Game);
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

        while (node.parent != null)
        {
            solution.Add(node);
            node = node.parent;
        }

        solution.Reverse();

        for (int nodeIndex = 0; nodeIndex < solution.Count; nodeIndex++)
        {
            Debug.Log(solution[nodeIndex].vehicle.ToString());
            puzzleGeneratorInstance.PrintBoard(solution[nodeIndex].board);
        }

        StartCoroutine(Move(solution));

        return await Task.FromResult(true);
    }

    IEnumerator Move(List<Node> solution)
    {
        for (int nodeIndex = 0; nodeIndex < solution.Count; nodeIndex++)
        {
            Debug.Log("MOveTo");
            Debug.Log(solution[nodeIndex].vehicle.startPosition[0] + ", " + solution[nodeIndex].vehicle.startPosition[1]);
            yield return InstanceCreator.GetVehicleMovement().MoveTo(solution[nodeIndex].vehicle.id, solution[nodeIndex].vehicle.startPosition);
        }
    }
}
