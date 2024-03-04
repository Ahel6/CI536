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

		private void Start()
		{
			Grid = new MazeGrid(20, 20);
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
