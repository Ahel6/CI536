using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class MazeGrid
{
	public bool IsFinished { get; private set; } = false;

	private readonly MazeCell[,] _grid;

	public MazeGrid(int width, int height, float deadEndLinkChance)
	{
		_grid = new MazeCell[width, height];
		GenerateMaze(width, height, deadEndLinkChance);
	}

	public MazeCell[,] GetCellArray() => _grid;

	private void GenerateMaze(int width, int height, float deadEndLinkChance)
	{
		// Generates a maze using a depth-first algorithm.
		// Top left of the maze is (0,0)

		// Initialize grid
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				_grid[x, y] = new MazeCell(x, y);
			}
		}

		int startingCellX = 0;
		int startingCellY = 0;

		MazeCell startingCell = _grid[startingCellX, startingCellY];
		startingCell.visitedByGenerator = true;
		startingCell.IsStart = true;

		var cellStack = new Stack<MazeCell>();
		bool backtracking = false;
		int visitedCount = 1;

		while (true)
		{
			if (!backtracking)
			{
				var current = cellStack.Count > 0 ? cellStack.Peek() : startingCell;

				var (randNeighbour, dir) = GetRandomUnvisitedNeighbour(current);

				if (randNeighbour == null)
				{
					Debug.Log($"Reached a dead end.");

					if (Random.Range(0f, 1f) <= deadEndLinkChance)
					{
						Debug.Log($"Trying to link to another cell...");

						// Link to random cell to make maze more connected
						var (connect, connectDir) = GetRandomUnvisitedNeighbour(current, true);

						if (connect != null && connect.X != width - 1 && connect.Y != height - 1)
						{
							LinkCells(current, connect, connectDir);
						}
					}

					// Reached a dead end, backtrack to last cell with unvisited neighbour
					backtracking = true;
					continue;
				}

				randNeighbour.visitedByGenerator = true;

				LinkCells(current, randNeighbour, dir);

				cellStack.Push(randNeighbour);
				visitedCount++;

				if (visitedCount != width * height) continue;
				IsFinished = true;
				break;
			}

			Debug.Log($"Attempting backtrack...");

			if (GetRandomUnvisitedNeighbour(cellStack.Peek()).cell == null)
			{
				cellStack.Pop();
			}
			else
			{
				Debug.Log($"Finished backtrack.");
				backtracking = false;
			}
		}
	}

	private (MazeCell cell, CellDirection directionFromCurrent) GetRandomUnvisitedNeighbour(MazeCell current, bool includeVisited = false)
	{
		CellDirection availableDirections = CellDirection.NORTH | CellDirection.EAST | CellDirection.SOUTH | CellDirection.WEST;
		ProcessDirection(CellDirection.NORTH);
		ProcessDirection(CellDirection.EAST);
		ProcessDirection(CellDirection.SOUTH);
		ProcessDirection(CellDirection.WEST);

		CellDirection[] matching = Enum.GetValues(typeof(CellDirection))
				   .Cast<CellDirection>()
				   .Where(dir => availableDirections.HasFlag(dir) && dir != CellDirection.NONE)
				   .ToArray();

		if (matching.Length == 0)
		{
			return (null, CellDirection.NORTH);
		}

		CellDirection randomAvailableDirection = matching[Random.Range(0, matching.Length)];

		return (GetCellInDirection(current, randomAvailableDirection), randomAvailableDirection);

		void ProcessDirection(CellDirection direction)
		{
			MazeCell cell = GetCellInDirection(current, direction);

			if (includeVisited)
			{
				if (cell == null || cell.LinkedTo(current))
				{
					availableDirections &= ~direction;
				}
			}
			else
			{
				if (cell == null || cell.visitedByGenerator)
				{
					availableDirections &= ~direction;
				}
			}
		}
	}

	private MazeCell GetCellInDirection(MazeCell current, CellDirection direction)
	{
		var cellX = current.X;
		var cellY = current.Y;

		return direction switch
		{
			CellDirection.NORTH => cellY == 0 ? null : _grid[cellX, cellY - 1],
			CellDirection.EAST => cellX == _grid.GetLength(0) - 1 ? null : _grid[cellX + 1, cellY],
			CellDirection.SOUTH => cellY == _grid.GetLength(1) - 1 ? null : _grid[cellX, cellY + 1],
			CellDirection.WEST => cellX == 0 ? null : _grid[cellX - 1, cellY],
			_ => null
		};
	}

	private static void LinkCells(MazeCell cellA, MazeCell cellB, CellDirection dir)
	{
		Debug.Log($"Linking {cellA} {dir} to {cellB}");
		switch (dir)
		{
			case CellDirection.NORTH:
				cellA.North = cellB;
				cellB.South = cellA;
				break;
			case CellDirection.EAST:
				cellA.East = cellB;
				cellB.West = cellA;
				break;
			case CellDirection.SOUTH:
				cellA.South = cellB;
				cellB.North = cellA;
				break;
			case CellDirection.WEST:
				cellA.West = cellB;
				cellB.East = cellA;
				break;
			case CellDirection.NONE:
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(dir), dir, "Invalid direction.");
		}
	}
}
