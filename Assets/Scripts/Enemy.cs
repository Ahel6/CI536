using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
	[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy", order = 1)]
	public class Enemy : ScriptableObject
	{
		public int EnemyID;
		public string Name;
		public float MaxHealth;
		public float CurrentHealth;
		public Weapon Weapon;
	}
}
