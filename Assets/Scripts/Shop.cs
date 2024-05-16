using UnityEngine;

namespace Assets.Scripts
{
	[CreateAssetMenu(fileName = "Shop", menuName = "ScriptableObjects/Shop")]
	public class Shop : ScriptableObject
	{
		public ShopItem[] items;
		public int[] itemCosts;
	}
}
