using System;
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
		Debug.Log($"{X},{Y} GetCellInDirection {dir}");
		if (dir == CellDirection.NORTH)
		{
			Debug.Log($"- NORTH - null:{North == null}");
			return North;
		}
		else if (dir == CellDirection.EAST)
		{
			Debug.Log($"- EAST - null:{East == null}");
			return East;
		}
		else if (dir == CellDirection.SOUTH)
		{
			Debug.Log($"- SOUTH - null:{South == null}");
			return South;
		}
		else if (dir == CellDirection.WEST)
		{
			Debug.Log($"- WEST - null:{West == null}");
			return West;
		}

		return null;
	}

	public bool IsDeadEnd()
	{
		var sum = 0;

		void SumDir(MazeCell dir)
		{
			if (dir != null)
			{
				sum += 1;
			}
		}

		SumDir(North);
		SumDir(East);
		SumDir(South);
		SumDir(West);

		return sum == 1;
	}

	// store other info here, like if this is a shop etc
}
