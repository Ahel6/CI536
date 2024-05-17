using System;

namespace Assets.Scripts
{
	[Flags]
	public enum CellDirection
	{
		NONE = 0,
		NORTH = 1,
		EAST = 2,
		SOUTH = 4,
		WEST = 8
	}
}