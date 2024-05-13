using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
