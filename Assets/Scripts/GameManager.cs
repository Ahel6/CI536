using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

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

		public MazeCell EndingCell;

		private GameObject mazeParent;

		private void Awake()
		{
			Instance = this;
		}

		private void Start()
		{
			StartNewLayer(MazeSize.x, MazeSize.y);
		}

		public void StartNewLayer(int width, int height)
		{
			Debug.Log($"Start new layer width:{width} height:{height}");

			// Generate maze
			Maze = new MazeGrid(width, height, DeadEndLinkChance);
			GenerateMazeGeometry();
			GenerateEndCell();
			GenerateShop();

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

			if (Keyboard.current[Key.Enter].wasPressedThisFrame)
			{
				Player.ExitLayer();
			}
		}

		private void GenerateMazeGeometry()
		{
			if (mazeParent != null)
			{
				Destroy(mazeParent);
			}

			mazeParent = new GameObject("MazeParent");

			foreach (var item in Maze.GetCellArray())
			{
				var newPrefab = Instantiate(CellPrefab, mazeParent.transform);
				newPrefab.transform.position = item.WorldPosition;

				var wallController = newPrefab.GetComponent<CellWallContainer>();

				item.prefab = wallController;

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
			}
		}

		private void GenerateEndCell()
		{
			// Starting with starting cell, assign each travelled cell an incrementally higher number
			// Cell with highest number is the furthest cell away from the start.

			var cellArray = Maze.GetCellArray();

			var startingCell = cellArray[0, 0];
			startingCell.depthValue = 0;

			var currentCells = new List<MazeCell> { startingCell };
			var nextCells = new List<MazeCell>();

			while (cellArray.Cast<MazeCell>().ToList().Any(x => x.depthValue == -1))
			{
				foreach (var item in currentCells)
				{
					var connectingCells = item.GetConnectingCells();
					foreach (var connect in connectingCells)
					{
						if (connect.depthValue == -1)
						{
							connect.depthValue = item.depthValue + 1;
							nextCells.Add(connect);
						}
					}
				}

				currentCells.Clear();
				currentCells.AddRange(nextCells);
			}

			EndingCell = cellArray.Cast<MazeCell>().ToList().MaxBy(x => x.depthValue);
			Debug.Log($"Generating exit at {EndingCell.X},{EndingCell.Y}");

			var wallController = EndingCell.prefab;

			wallController.ExitObjects.SetActive(true);

			if (EndingCell.North != null)
			{
				// North connection
				wallController.ExitObjects.transform.rotation = Quaternion.Euler(0, 0, 0);
				wallController.SouthHolePlug.SetActive(false);
			}
			else if (EndingCell.West != null)
			{
				// West connection
				wallController.ExitObjects.transform.rotation = Quaternion.Euler(0, 90, 0);
				wallController.EastHolePlug.SetActive(false);
			}
			else if (EndingCell.East != null)
			{
				// East connection
				wallController.ExitObjects.transform.rotation = Quaternion.Euler(0, 270, 0);
				wallController.WestHolePlug.SetActive(false);
			}
			else if (EndingCell.South != null)
			{
				// South connection
				wallController.ExitObjects.transform.rotation = Quaternion.Euler(0, 180, 0);
				wallController.NorthHolePlug.SetActive(false);
			}
		}

		private void GenerateShop()
		{
			// Generate shop halfway through maze

			var startingDepth = 0;
			var endingDepth = EndingCell.depthValue;
			var midpoint = (endingDepth - startingDepth) / 2;

			var possibleShopLocations = Maze.GetCellArray().Cast<MazeCell>().ToList().Where(x => x.depthValue == midpoint).ToList();

			var shopCell = possibleShopLocations[UnityEngine.Random.Range(0, possibleShopLocations.Count)];

			Debug.Log($"Generating shop at {shopCell.X},{shopCell.Y}");

			// todo - implement
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
