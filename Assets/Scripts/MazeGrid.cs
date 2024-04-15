using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class MazeGrid
{
	public bool IsFinished { get; private set; } = false;

	private MazeCell[,] _grid;

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
		for (var y = 0; y < height; y++)
		{
			for (var x = 0; x < width; x++)
			{
				_grid[x, y] = new MazeCell(x, y);
			}
		}

		var startingCellX = 0;
		var startingCellY = 0;

		var startingCell = _grid[startingCellX, startingCellY];
		startingCell.vistedByGenerator = true;

		var cellStack = new Stack<MazeCell>();
		var backtracking = false;
		var visitedCount = 1;

		while (true)
		{
			if (!backtracking)
			{
				var current = cellStack.Count > 0 ? cellStack.Peek() : startingCell;

				var (randNeigbour, dir) = GetRandomUnvistedNeighbour(current);

				if (randNeigbour == null)
				{
					if (UnityEngine.Random.Range(0f, 1f) <= deadEndLinkChance);
					{
						// link to random cell to make maze more connected
						var (connect, connectDir) = GetRandomUnvistedNeighbour(current, true);

						if (connect != null && connect.X != width - 1 && connect.Y != height - 1)
						{
							LinkCells(current, connect, connectDir);
						}
					}

					// reached a dead end, backtrack to last cell with unvisited neighbour
					backtracking = true;
					continue;
				}

				randNeigbour.vistedByGenerator = true;

				LinkCells(current, randNeigbour, dir);

				cellStack.Push(randNeigbour);
				visitedCount++;

				if (visitedCount == width * height)
				{
					IsFinished = true;
					break;
				}

				if (randNeigbour.X == width - 1 && randNeigbour.Y == height - 1)
				{
					// Force corner opposite of start to be a dead end - the exit.
					cellStack.Pop();
					backtracking = true;
				}
			}
			else
			{
				if (GetRandomUnvistedNeighbour(cellStack.Peek()).cell == null)
				{
					cellStack.Pop();
				}
				else
				{
					backtracking = false;
				}
			}
		}
	}

	private (MazeCell cell, CellDirection directionFromCurrent) GetRandomUnvistedNeighbour(MazeCell current, bool includeVisited = false)
	{
		var availableDirections = CellDirection.NORTH | CellDirection.EAST | CellDirection.SOUTH | CellDirection.WEST;
		ProcessDirection(CellDirection.NORTH);
		ProcessDirection(CellDirection.EAST);
		ProcessDirection(CellDirection.SOUTH);
		ProcessDirection(CellDirection.WEST);

		var matching = Enum.GetValues(typeof(CellDirection))
				   .Cast<CellDirection>()
				   .Where(dir => availableDirections.HasFlag(dir))
				   .ToArray();

		if (matching == null || matching.Length == 0)
		{
			return (null, CellDirection.NORTH);
		}

		var randomAvailableDirection = matching[new System.Random().Next(matching.Length)];

		return (GetCellInDirection(current, randomAvailableDirection), randomAvailableDirection);

		void ProcessDirection(CellDirection direction)
		{
			var cell = GetCellInDirection(current, direction);

			if (includeVisited)
			{
				if (cell == null || cell.LinkedTo(current))
				{
					availableDirections &= ~direction;
				}
			}
			else
			{
				if (cell == null || cell.vistedByGenerator)
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

	private void LinkCells(MazeCell cellA, MazeCell cellB, CellDirection dir)
	{
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
		}
	}
}
