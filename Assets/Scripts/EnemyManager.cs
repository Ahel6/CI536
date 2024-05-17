using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class EnemyManager : MonoBehaviour
	{
		public static EnemyManager Instance { get; private set; }

		public List<Enemy> Enemies;

		private void Awake()
		{
			Instance = this;
		}
	}
}
