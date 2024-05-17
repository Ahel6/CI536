using Assets.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class MazeCell
{
	public MazeCell(int x, int y)
	{
		X = x;
		Y = y;
	}

	public override string ToString()
	{
		return $"({X},{Y})";
	}

	public int X;
	public int Y;

	public MazeCell North;
	public MazeCell East;
	public MazeCell South;
	public MazeCell West;

	[FormerlySerializedAs("vistedByGenerator")] public bool visitedByGenerator; // FIXME in editor
	public int depthValue = -1;

	public CellWallContainer prefab;

	public Vector3 WorldPosition => new(X * 3, 0, Y * -3);
	
	// store other info here, like if this is a shop etc
	public bool IsStart;
	public bool IsExit;
	public bool IsShop;
	public bool IsEnemy;

	public MazeCell[] GetConnectingCells()
	{
		var cellList = new List<MazeCell>();

		AddCellToList(North);
		AddCellToList(East);
		AddCellToList(South);
		AddCellToList(West);

		return cellList.ToArray();
		
		void AddCellToList(MazeCell cell)
		{
			if (cell != null)
			{
				cellList.Add(cell);
			}
		}
	}

	public bool LinkedTo(MazeCell cell)
	{
		return North == cell || East == cell || South == cell || West == cell || cell.North == this ||
		       cell.East == this || cell.South == this || cell.West == this;
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

	public bool IsDeadEnd() => GetConnectingCells().Length == 1;
}
