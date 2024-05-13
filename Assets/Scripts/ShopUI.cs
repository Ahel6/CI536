using System;
using UnityEngine;

namespace Assets.Scripts
{
	public class ShopUI : MonoBehaviour
	{
		public static ShopUI Instance;
		
		public Shop Shop;

		private void Awake()
		{
			Instance = this;
		}
		
		public void LoadShop(Shop shop)
		{
			Shop = shop;
		}
	}
}
