using UnityEngine;

namespace Assets.Scripts
{
	[CreateAssetMenu(fileName = "Shop", menuName = "ScriptableObjects/Shop")]
	public class Shop : ScriptableObject
	{
		[SerializeField]
		private ShopItem[] items;
		[SerializeField]
		private int[] itemCosts;
	}
}
