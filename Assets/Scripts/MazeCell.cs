using System;

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

	// store other info here, like if this is a shop etc
}
