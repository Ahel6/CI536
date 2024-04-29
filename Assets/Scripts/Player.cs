using System;
using System.Collections;
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

		private bool _isTurning;
		private bool _isMoving;

		public void TurnLeft()
		{
			if (_isTurning)
			{
				return;
			}

			StartCoroutine(TurnCoroutine(-90));
		}

		public void TurnRight()
		{
			if (_isTurning)
			{
				return;
			}

			StartCoroutine(TurnCoroutine(90));
		}

		private IEnumerator MoveCoroutine(Vector3 targetPosition)
		{
			_isMoving = true;
			while (true)
			{
				var newPos = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 10);

				if (Mathf.Approximately(targetPosition.x, newPos.x)
					&& Mathf.Approximately(targetPosition.y, newPos.y)
					&& Mathf.Approximately(targetPosition.z, newPos.z))
				{
					newPos = targetPosition;
					transform.position = newPos;
					_isMoving = false;
					break;
				}

				transform.position = newPos;
				yield return null;
			}
		}

		private IEnumerator TurnCoroutine(float turnBy)
		{
			_isTurning = true;
			var target = transform.rotation.eulerAngles.y + turnBy;
			while (true)
			{
				var newRotation = Mathf.MoveTowardsAngle(transform.localEulerAngles.y, target, Time.deltaTime * 240);
				if (Mathf.Approximately(target, newRotation))
				{
					newRotation = target;
					transform.localEulerAngles = new Vector3(0, newRotation, 0);
					_isTurning = false;
					break;
				}

				transform.localEulerAngles = new Vector3(0, newRotation, 0);

				yield return null;
			}
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

		public void MoveToCell(int x, int y, bool instantly = false)
		{
			if (_isMoving)
			{
				return;
			}	

			if (instantly)
			{
				transform.position = new Vector3(x * 3, 0, y * -3);
			}
			else
			{
				StartCoroutine(MoveCoroutine(new Vector3(x * 3, 0, y * -3)));
			}

			CurrentCell = GameManager.Instance.Maze.GetCellArray()[x, y];
		}

		public void MoveToCell(MazeCell cell, bool instantly = false)
		{
			if (_isMoving)
			{
				return;
			}

			if (instantly)
			{
				transform.position = cell.WorldPosition;
			}
			else
			{
				StartCoroutine(MoveCoroutine(cell.WorldPosition));
			}
			
			CurrentCell = GameManager.Instance.Maze.GetCellArray()[cell.X, cell.Y];
		}
	}
}
