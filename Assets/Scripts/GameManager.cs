using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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

		private MazeGrid _maze;

		public MazeCell CurrentCell { get; private set; }

		private void Awake()
		{
			Instance = this;
		}

		private void Start()
		{
			_maze = new MazeGrid(MazeSize.x, MazeSize.y, DeadEndLinkChance);
			GenerateMazeGeometry();
		}

		private void GenerateMazeGeometry()
		{
			foreach (var item in _maze.GetCellArray())
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
			if (_maze == null)
			{
				return;
			}

			Gizmos.color = Color.red;

			foreach (var item in _maze.GetCellArray())
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
