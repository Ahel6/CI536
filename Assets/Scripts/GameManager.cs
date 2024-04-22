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
		public PlayerMover Player;

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

			// Ensure player is at start
			Player.MoveToCell(0, 0);

			// Rotate player to face door
			// Starting room only has one connection, in N or E
			if (CurrentCell.North != null)
			{
				Player.transform.rotation = Quaternion.identity;
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
				newPrefab.transform.position = new Vector3(item.X * 3, 0, item.Y * 3);

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
					wallController.FloorHolePlug.SetActive(false);
				}
			}
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
					Gizmos.DrawLine(new Vector3(item.X * 3, 0, item.Y * 3), new Vector3(item.North.X * 3, 0, item.North.Y * 3));
				}

				if (item.East != null)
				{
					Gizmos.DrawLine(new Vector3(item.X * 3, 0, item.Y * 3), new Vector3(item.East.X * 3, 0, item.East.Y * 3));
				}

				if (item.South != null)
				{
					Gizmos.DrawLine(new Vector3(item.X * 3, 0, item.Y * 3), new Vector3(item.South.X * 3, 0, item.South.Y * 3));
				}

				if (item.West != null)
				{
					Gizmos.DrawLine(new Vector3(item.X * 3, 0, item.Y * 3), new Vector3(item.West.X * 3, 0, item.West.Y * 3));
				}
			}
		}
	}
}
