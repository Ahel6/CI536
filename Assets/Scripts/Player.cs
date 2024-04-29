using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
	public class Player : MonoBehaviour
	{
		public const double Multiplier = 1.3;

		public int Health;
		public int EnemyAttack;
		public int GoldDropped;
		public int XPDropped;

		public MazeCell CurrentCell { get; private set; }

		public CellDirection GetCurrentDirection()
		{
			var modAngle = Mathf.RoundToInt(transform.rotation.eulerAngles.y % 360);
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


		public void TurnLeft()
		{
			transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 45, 0);
		}

		public void TurnRight()
		{
			transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 45, 0);
		}

		public void MoveForward()
		{
			var direction = GetCurrentDirection();
			var cellToMoveTo = CurrentCell.GetCellInDirection(direction);

			if (cellToMoveTo == null)
			{
				return;
			}

			MoveToCell(cellToMoveTo);
		}

		public void ExitLayer()
		{
			if (GameManager.Instance.EndingCell != CurrentCell)
			{
				Debug.LogError($"Can't ExitLayer() when not in exit!");
				return;
			}

			// fade out screen

			// reset maze
			var currentWidth = GameManager.Instance.Maze.GetCellArray().GetLength(0);
			var currentHeight = GameManager.Instance.Maze.GetCellArray().GetLength(0);
			GameManager.Instance.StartNewLayer(currentWidth + 1, currentHeight + 1);

			// fade screen back in
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
