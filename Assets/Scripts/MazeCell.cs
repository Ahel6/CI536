using Assets.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MazeCell
{
	public MazeCell(int x, int y)
	{
		X = x;
		Y = y;
	}

	public int X;
	public int Y;

	public MazeCell North;
	public MazeCell East;
	public MazeCell South;
	public MazeCell West;

	public bool vistedByGenerator;
	public int depthValue = -1;

	public CellWallContainer prefab;

	public Vector3 WorldPosition => new(X * 3, 0, Y * -3);

	public MazeCell[] GetConnectingCells()
	{
		var cellList = new List<MazeCell>();

		void AddCellToList(MazeCell cell)
		{
			if (cell != null)
			{
				cellList.Add(cell);
			}
		}

		AddCellToList(North);
		AddCellToList(East);
		AddCellToList(South);
		AddCellToList(West);

		return cellList.ToArray();
	}

	public bool LinkedTo(MazeCell cell)
	{
		if (North == cell || East == cell || South == cell || West == cell)
		{
			return true;
		}

		if (cell.North == this || cell.East == this || cell.South == this || cell.West == this)
		{
			return true;
		}

		return false;
	}

	public MazeCell GetCellInDirection(CellDirection dir)
	{
		return dir switch
		{
			CellDirection.NORTH => North,
			CellDirection.EAST => East,
			CellDirection.SOUTH => South,
			CellDirection.WEST => West,
			_ => null
		};
	}

	public bool IsDeadEnd()
	{
		return GetConnectingCells().Length == 1;
	}

	// store other info here, like if this is a shop etc
}
