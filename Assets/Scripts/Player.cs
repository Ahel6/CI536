using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class Player : MonoBehaviour
	{
		public static Player Instance;

		public float MaxHealth = 10;
		public float Health = 10;
		public int Gold;

		public Weapon Weapon;

		public List<Item> Inventory;

		public MazeCell CurrentCell { get; private set; }

		public bool CanMove;

		private Light _light;
		
		private bool _movingToCell;
		private bool _turning;
		private float _targetRotation;
		private bool _isFadingOut;
		private bool _isFadingIn;
		private bool _descending;
		private bool _enteringCombat;
		private bool _inCombat;
		private bool _exitingCombat;

		private void Start()
		{
			Instance = this;
			CanMove = true;
			_light = GetComponentInChildren<Light>();
		}

		private CellDirection GetCurrentDirection()
		{
			var modAngle = Mathf.RoundToInt(transform.rotation.eulerAngles.y % 360);
			return modAngle switch
			{
				0 => CellDirection.NORTH,
				90 => CellDirection.EAST,
				180 => CellDirection.SOUTH,
				270 => CellDirection.WEST,
				_ => CellDirection.NONE
			};
		}

		public void ExitCombat()
		{
			_exitingCombat = true;
		}

		public void TurnLeft()
		{
			if (_turning || !CanMove)
			{
				return;
			}

			_targetRotation = transform.localEulerAngles.y - 90;
			_turning = true;
		}

		public void TurnRight()
		{
			if (_turning || !CanMove)
			{
				return;
			}

			_targetRotation = transform.localEulerAngles.y + 90;
			_turning = true;
		}

		public void FixedUpdate()
		{
			if (_movingToCell)
			{
				Vector3 targetPosition = CurrentCell.WorldPosition;
				var newPos = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 10);
				transform.position = newPos;

				if (Mathf.Approximately(targetPosition.x, newPos.x)
				    && Mathf.Approximately(targetPosition.y, newPos.y)
				    && Mathf.Approximately(targetPosition.z, newPos.z))
				{
					transform.position = targetPosition;
					_movingToCell = false;

					if (CurrentCell.IsEnemy)
					{
						CurrentCell.IsEnemy = false;
						_enteringCombat = true;
					}

					UIManager.Instance.EnterShopButton.SetActive(CurrentCell.IsShop);
				}
			}

			if (_turning)
			{
				float newRotation = Mathf.MoveTowardsAngle(transform.localEulerAngles.y, _targetRotation, Time.deltaTime * 240);
				transform.localEulerAngles = new Vector3(0, newRotation, 0);

				if (Mathf.Approximately(_targetRotation, newRotation))
				{
					transform.localEulerAngles = new Vector3(0, _targetRotation, 0);
					_turning = false;
				}
			}

			if (_isFadingOut)
			{
				_light.intensity = Mathf.MoveTowards(_light.intensity, 0, Time.deltaTime);

				if (Mathf.Approximately(_light.intensity, 0))
				{
					_isFadingOut = false;
					_light.intensity = 0;
				}
			}

			if (_isFadingIn)
			{
				_light.intensity = Mathf.MoveTowards(_light.intensity, 1, Time.deltaTime);

				if (Mathf.Approximately(_light.intensity, 1))
				{
					CanMove = true;
					_isFadingIn = false;
					_light.intensity = 1;
				}
			}

			if (_descending)
			{
				if (_light.intensity == 0)
				{
					int currentWidth = GameManager.Instance.Maze.GetCellArray().GetLength(0);
					int currentHeight = GameManager.Instance.Maze.GetCellArray().GetLength(0);
					GameManager.Instance.StartNewLayer(currentWidth + 1, currentHeight + 1);
					_isFadingIn = true;
					_descending = false;
				}
				else if (!_isFadingOut)
				{
					CanMove = false;
					_isFadingOut = true;
				}
			}

			if (_enteringCombat)
			{
				if (_light.intensity == 0)
				{
					_inCombat = true;
					_enteringCombat = false;
					UIManager.Instance.ChangeUIState(UIState.COMBAT);

					var enemy = EnemyManager.Instance.Enemies[0];
					CombatSystem.Instance.LoadEnemy(enemy);
				}
				else if (!_isFadingOut)
				{
					CanMove = false;
					_isFadingOut = true;
				}
			}

			if (!_exitingCombat) return;
			
			if (Mathf.Approximately(_light.intensity, 1))
			{
				_inCombat = false;
				_exitingCombat = false;
				CanMove = true;
			}
			else if (!_isFadingIn)
			{
				UIManager.Instance.ChangeUIState(UIState.EXPLORE);
				_isFadingIn = true;
				CanMove = false;
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
				Debug.LogError("Can't ExitLayer() when not in exit!");
				return;
			}

			_descending = true;
		}

		public void MoveToCell(int x, int y, bool instantly = false)
		{
			if (_movingToCell || (!CanMove && !instantly))
			{
				return;
			}	

			if (instantly)
			{
				transform.position = new Vector3(x * 3, 0, y * -3);
			}
			else
			{
				_movingToCell = true;
			}

			CurrentCell = GameManager.Instance.Maze.GetCellArray()[x, y];
		}

		private void MoveToCell(MazeCell cell, bool instantly = false)
		{
			if (_movingToCell || (!CanMove && !instantly))
			{
				return;
			}

			if (instantly)
			{
				transform.position = cell.WorldPosition;
			}
			else
			{
				_movingToCell = true;
			}
			
			CurrentCell = GameManager.Instance.Maze.GetCellArray()[cell.X, cell.Y];
		}
	}
}
