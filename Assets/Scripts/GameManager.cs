using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
	public class GameManager : MonoBehaviour
	{
		public static GameManager Instance { get; private set; }

		public GameObject CellPrefab;

		[Tooltip("Dimensions of the maze in cells.")]
		public Vector2Int MazeSize;

		[Tooltip("Chance that a dead end will connect to a random cell.")]
		[Range(0f, 1f)]
		public float DeadEndLinkChance;

		[Tooltip("The player object.")]
		public Player Player;

		public MazeGrid Maze { get; private set; }

		public MazeCell CurrentCell => Player.CurrentCell;

		private void Awake()
		{
			Instance = this;
		}

		private void Start()
		{
			// Generate maze
			Maze = new MazeGrid(MazeSize.x, MazeSize.y, DeadEndLinkChance);
			GenerateMazeGeometry();
			GenerateEndCell();

			// Ensure player is at start
			Player.MoveToCell(0, 0);

			// Rotate player to face door
			// Starting room only has one connection, in S or E
			if (CurrentCell.South != null)
			{
				Player.transform.rotation = Quaternion.Euler(0, 180, 0);
			}

			if (CurrentCell.East != null)
			{
				Player.transform.rotation = Quaternion.Euler(0, 90, 0);
			}
		}

		public void Update()
		{
			if (Keyboard.current[Key.LeftArrow].wasPressedThisFrame)
			{
				Player.TurnLeft();
			}

			if (Keyboard.current[Key.RightArrow].wasPressedThisFrame)
			{
				Player.TurnRight();
			}

			if (Keyboard.current[Key.UpArrow].wasPressedThisFrame)
			{
				Player.MoveForward();
			}
		}

		private void GenerateMazeGeometry()
		{
			foreach (var item in Maze.GetCellArray())
			{
				var newPrefab = Instantiate(CellPrefab);
				newPrefab.transform.position = item.WorldPosition;

				var wallController = newPrefab.GetComponent<CellWallContainer>();

				if (item.North != null)
				{
					wallController.NorthWall.SetActive(false);
				}

				if (item.East != null)
				{
					wallController.EastWall.SetActive(false);
				}

				if (item.South != null)
				{
					wallController.SouthWall.SetActive(false);
				}

				if (item.West != null)
				{
					wallController.WestWall.SetActive(false);
				}

				if (item.West == null && item.East == null && item.North == null && item.South == null)
				{
					wallController.Cover.SetActive(true);
				}

				if (item.X == MazeSize.x - 1 && item.Y == MazeSize.y - 1)
				{
					//wallController.FloorHolePlug.SetActive(false);
					wallController.ExitObjects.SetActive(true);

					if (item.North != null)
					{
						// North connection
						wallController.ExitObjects.transform.rotation = Quaternion.Euler(0, 0, 0);
						wallController.SouthHolePlug.SetActive(false);
					}
					else
					{
						// West connection
						wallController.ExitObjects.transform.rotation = Quaternion.Euler(0, 90, 0);
						wallController.EastHolePlug.SetActive(false);
					}
				}
			}
		}

		private void GenerateEndCell()
		{
			// Starting with starting cell, assign each travelled cell an incrementally higher number
			// Cell with highest number is the furthest cell away from the start.

			var cellArray = Maze.GetCellArray();

			var startingCell = cellArray[0, 0];
			startingCell.depthValue = 0;

			/*while (cellArray.Cast<MazeCell>().ToList().Any(x => x.depthValue == -1))
			{

			}*/
		}

		private void OnDrawGizmos()
		{
			if (Maze == null)
			{
				return;
			}

			Gizmos.color = Color.red;

			foreach (var item in Maze.GetCellArray())
			{
				if (item.North != null)
				{
					Gizmos.DrawLine(item.WorldPosition, item.North.WorldPosition);
				}

				if (item.East != null)
				{
					Gizmos.DrawLine(item.WorldPosition, item.East.WorldPosition);
				}

				if (item.South != null)
				{
					Gizmos.DrawLine(item.WorldPosition, item.South.WorldPosition);
				}

				if (item.West != null)
				{
					Gizmos.DrawLine(item.WorldPosition, item.West.WorldPosition);
				}
			}
		}
	}
}
