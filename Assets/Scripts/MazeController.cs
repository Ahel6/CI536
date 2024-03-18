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

		private void Start()
		{
			Grid = new MazeGrid(10, 10);

			foreach (var item in Grid.GetGrid())
			{
				var newPrefab = Instantiate(CellPrefab);
				newPrefab.transform.position = new Vector3(item.X, 0, item.Y);

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
			}
		}

		private void OnDrawGizmos()
		{
			if (Grid == null)
			{
				return;
			}

			foreach (var item in Grid.GetGrid())
			{
				if (item.North != null)
				{
					Gizmos.DrawLine(new Vector3(item.X, 0, item.Y), new Vector3(item.North.X, 0, item.North.Y));
				}

				if (item.East != null)
				{
					Gizmos.DrawLine(new Vector3(item.X, 0, item.Y), new Vector3(item.East.X, 0, item.East.Y));
				}

				if (item.South != null)
				{
					Gizmos.DrawLine(new Vector3(item.X, 0, item.Y), new Vector3(item.South.X, 0, item.South.Y));
				}

				if (item.West != null)
				{
					Gizmos.DrawLine(new Vector3(item.X, 0, item.Y), new Vector3(item.West.X, 0, item.West.Y));
				}
			}
		}
	}
}
