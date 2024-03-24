using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Events;
using Enums;
using UI.Controllers;
using UI.Views;
using System;

/// <summary>
/// Singelton for all LightsOut game logic
/// tracks amount of time spent
/// tacks number of moves taken
/// generates new boards
/// checks if board is valid
/// checks for end state
/// listens for UI events to updated board
/// </summary>
public class LightsOutModel : Singelton<LightsOutModel>
{

    [Header("GameInfo")]
    [SerializeField]
    Vector2Int gridSize;

    [Header("Prefabs")]
    [SerializeField]
    GameObject LightButtonPrefab;

    // graph for tracking neightbors
    Dictionary<LightButton, List<LightButton>> gameGrid = new Dictionary<LightButton, List<LightButton>>();
    // keyed lookup for each game node
    Dictionary<string, LightButton> m_matrixMap = new Dictionary<string, LightButton>();
    public Dictionary<string, LightButton> MatrixMap { get => m_matrixMap; }
    int[,] gameMatrix;

    bool m_gameActive;
    public bool GameActive { get => m_gameActive; }
    float m_totalTime;
    public float TotalTime { get => m_totalTime; }
    int m_totalMoves = 0;
    public int TotalMoves { get => m_totalMoves; }

    IEnumerator HeartBeat()
    {
        while (m_gameActive)
        {
            m_totalTime += 1;
            // wait for 1 second before the next update
            //Using scaled time for a future pause feature.
            yield return new WaitForSeconds(1);
        }
    }
    private void ClearGrid()
    {
        //nothing to clear ignore
        if (gameMatrix == null || gameMatrix.Length == 0)
            return;

        Array.Clear(gameMatrix, 0, gameMatrix.Length);
        m_matrixMap.Clear();
        gameGrid.Clear();
        m_totalMoves = 0;
        m_totalTime = 0;
        m_gameActive = false;
    }

    /// <summary>
    /// Only gets called when a new game has started
    /// </summary>
    /// <param name="x">width of the board</param>
    /// <param name="y">height of the baord</param>
    public void GenerateGrid()
    {
        // clear any old game states
        ClearGrid();

        gameMatrix = new int[gridSize.x, gridSize.y];

        //Map the graph and relationships of the nodes
        for (int i = 0; i < gameMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < gameMatrix.GetLength(1); j++)
            {
                string matrixKey = $"{i},{j}";
                GameObject newNode = Instantiate(LightButtonPrefab);
                newNode.gameObject.name = matrixKey;
                LightButton lightButton = newNode.GetComponent<LightButton>();
                //Add light to matrix map for easy lookup later
                m_matrixMap.Add(matrixKey, lightButton);
                //Add light to graph for easy neightbor checking later empty array for now untill we add neiighbors
                gameGrid.Add(lightButton, new List<LightButton>());
            }
        }
        BuildNeighborGraph();
        GenerateRandomValidBoard();
        m_gameActive = true;
        StartCoroutine(HeartBeat());
    }

    public void EndGame()
    {
        ClearGrid();
    }

    /// <summary>
    /// Updates the state of the neighbors of the target LightButton
    /// </summary>
    /// <param name="obj">Object that has been clicked</param>
    /// <param name="value">The state the object has toggeled to</param>
    public void UpdatedBoard(LightButton obj, bool value)
    {
        foreach (LightButton neighbor in gameGrid[obj])
        {
            neighbor.ToggleState();
        }

        // Only update the number of moves if we have finished making the game borad.
        if (m_gameActive)
        {
            ++m_totalMoves;
        }

        //After each update check if any lights are still on
        if (IsGameComplete())
        {
            m_gameActive = false;
        }
    }

    /// <summary>
    /// iterates through all the buttons to check the game state
    /// </summary>
    /// <returns>True if all lights are off, False if any light is on</returns>
    bool IsGameComplete()
    {
        foreach (LightButton b in m_matrixMap.Values)
        {
            if (b.IsOn)
                return false;
        }
        return true;
    }

    /// <summary>
    /// Generate a random board by simulating clicks on up to 10 random buttons
    /// </summary>
    void GenerateRandomValidBoard()
    {
        for (int i = 0; i < UnityEngine.Random.Range(1, 10); i++)
        {
            int randomRow = UnityEngine.Random.Range(0, gameMatrix.GetLength(0) - 1);
            int randomCol = UnityEngine.Random.Range(0, gameMatrix.GetLength(1) - 1);
            string key = $"{randomRow},{randomCol}";
            m_matrixMap[key].ToggleState();
            UpdatedBoard(m_matrixMap[key], m_matrixMap[key].IsOn);
        }
    }

    /// <summary>
    /// Build the ralational graph for each node
    /// checks all directions for a valid index
    /// if valid add the neighbor to a list of neighbors
    /// </summary>
    void BuildNeighborGraph()
    {
        for (int r = 0; r < gameMatrix.GetLength(0); r++)
        {
            for (int c = 0; c < gameMatrix.GetLength(1); c++)
            {
                string key = $"{r},{c}";
                if (CheckBounds(gameMatrix, r + 1, c))
                {
                    gameGrid[m_matrixMap[key]].Add(m_matrixMap[$"{r + 1},{c}"]);
                }
                if (CheckBounds(gameMatrix, r - 1, c))
                {
                    gameGrid[m_matrixMap[key]].Add(m_matrixMap[$"{r - 1},{c}"]);
                }
                if (CheckBounds(gameMatrix, r, c - 1))
                {
                    gameGrid[m_matrixMap[key]].Add(m_matrixMap[$"{r},{c - 1}"]);
                }
                if (CheckBounds(gameMatrix, r, c + 1))
                {
                    gameGrid[m_matrixMap[key]].Add(m_matrixMap[$"{r},{c + 1}"]);
                }
            }
        }
    }

    //Checks if an index is a valid index in the gameMatrix
    bool CheckBounds(int[,] grid, int row, int col)
    {

        //Check if x is in bounds
        bool rowInBounds = 0 <= row && row < grid.GetLength(0);
        //Check if y is in bounds
        bool colInBounds = 0 <= col && col < grid.GetLength(1);
        //invalid location
        if (!rowInBounds || !colInBounds)
        {
            return false;
        }

        return true;
    }


}
