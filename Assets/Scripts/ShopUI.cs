using System;
using UnityEngine;

namespace Assets.Scripts
{
	public class ShopUI : MonoBehaviour
	{
		public static ShopUI Instance;
		
        [SerializeField]
        private GameObject ShopItemPrefab;
        
		public Shop Shop;

		private void Awake()
		{
			Instance = this;
		}
		
		public void LoadShop(Shop shop)
		{
			Shop = shop;

			Transform parent = Instance.gameObject.transform.GetChild(0);
			for (var index = 0; index < Shop.items.Length; index++)
			{
				ShopItem item = Shop.items[index];
				int cost = Shop.itemCosts[index];
				
				// Shop Item
				//     - Image
				//     - Name
				//     - Cost
				GameObject shopItem = Instantiate(ShopItemPrefab, parent);
				shopItem.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = item.Icon;
				shopItem.transform.GetChild(1).GetComponent<TextMesh>().text = item.Name;
				shopItem.transform.GetChild(2).GetComponent<TextMesh>().text = "$" + cost;
			}
		}
	}
}
