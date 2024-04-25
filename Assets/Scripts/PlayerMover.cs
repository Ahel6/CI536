using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
	public class PlayerMover : MonoBehaviour
	{
		public MazeCell CurrentCell { get; private set; }

		public void TurnLeft()
		{
			transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 45, 0);
		}

		public void TurnRight()
		{
			transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 45, 0);
		}

		public CellDirection GetCurrentDirection()
		{
			Debug.Log($"GetCurrentDirection");
			var modAngle = Mathf.RoundToInt(transform.rotation.eulerAngles.y % 360);
			Debug.Log($" - modAngle:{modAngle}");
			if (modAngle == 0)
			{
				return CellDirection.NORTH;
			}
			else if (modAngle == 90)
			{
				return CellDirection.EAST;
			}
			else if (modAngle == 180)
			{
				return CellDirection.SOUTH;
			}
			else if (modAngle == 270)
			{
				return CellDirection.WEST;
			}
			else
			{
				return CellDirection.NONE;
			}
		}

		public void MoveForward()
		{
			Debug.Log($"Move forward");
			var direction = GetCurrentDirection();
			Debug.Log($" - dir:{direction}");
			var cellToMoveTo = CurrentCell.GetCellInDirection(direction);
			Debug.Log($" - cell:{cellToMoveTo}");

			if (cellToMoveTo == null)
			{
				return;
			}

			MoveToCell(cellToMoveTo);
		}

		public void MoveToCell(int x, int y)
		{
			transform.position = new Vector3(x * 3, 0, y * -3);
			CurrentCell = GameManager.Instance.Maze.GetCellArray()[x, y];
		}

		public void MoveToCell(MazeCell cell)
		{
			transform.position = cell.WorldPosition;
			CurrentCell = GameManager.Instance.Maze.GetCellArray()[cell.X, cell.Y];
		}
	}
}
