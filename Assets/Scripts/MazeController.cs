using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
	public class MazeController : MonoBehaviour
	{
		public MazeGrid Grid;

		public GameObject CellPrefab;

		[Tooltip("Dimensions of the maze in cells.")]
		public Vector2Int MazeSize;

		[Tooltip("Chance that a dead end will connect to a random cell.")]
		[Range(0f, 1f)]
		public float DeadEndLinkChance;

		private void Start()
		{
			Grid = new MazeGrid(MazeSize.x, MazeSize.y, DeadEndLinkChance);

			foreach (var item in Grid.GetCellArray())
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
			}
		}

		
	}
}
