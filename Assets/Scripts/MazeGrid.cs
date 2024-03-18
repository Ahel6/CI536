using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class MazeGrid
{
	private MazeCell[,] _grid;

	public MazeGrid(int width, int height)
	{
		_grid = new MazeCell[width, height];
		GenerateMaze(width, height);
	}

	public MazeCell[,] GetGrid() => _grid;

	private void GenerateMaze(int width, int height)
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

		// Pick random starting point
		var startingCellX = UnityEngine.Random.Range(0, width - 1);
		var startingCellY = UnityEngine.Random.Range(0, height - 1);

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
					// reached a dead end, back track to last cell with unvisited neighbour
					backtracking = true;
					continue;
				}

				randNeigbour.vistedByGenerator = true;

				// remove wall
				switch (dir)
				{
					case CellDirection.NORTH:
						current.North = randNeigbour;
						randNeigbour.South = current;
						break;
					case CellDirection.EAST:
						current.East = randNeigbour;
						randNeigbour.West = current;
						break;
					case CellDirection.SOUTH:
						current.South = randNeigbour;
						randNeigbour.North = current;
						break;
					case CellDirection.WEST:
						current.West = randNeigbour;
						randNeigbour.East = current;
						break;
				}

				cellStack.Push(randNeigbour);
				visitedCount++;

				if (visitedCount == width * height)
				{
					break;
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

	private (MazeCell cell, CellDirection directionFromCurrent) GetRandomUnvistedNeighbour(MazeCell current)
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
			if (cell == null || cell.vistedByGenerator)
			{
				if (cell != null)
				{
					if (new Random().Next(6) == 1)
					{
						return;
					}
				}

				availableDirections &= ~direction;
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
}
