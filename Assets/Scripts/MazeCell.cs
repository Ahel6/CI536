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

	// store other info here, like if this is a shop etc
}
