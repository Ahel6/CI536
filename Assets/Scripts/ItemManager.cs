using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class ItemManager : MonoBehaviour
	{
		public static ItemManager Instance { get; private set; }

		public List<Weapon> Weapons;

		private void Awake()
		{
			Instance = this;
		}
	}
}
