using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		private void Start()
		{

		}
	}
}
