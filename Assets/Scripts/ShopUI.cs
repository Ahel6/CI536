using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class ShopUI : MonoBehaviour
	{
		public static ShopUI Instance;

		public GameObject ItemPrefab;

		public GameObject ShopRoot;
		public GameObject PlayerRoot;
		public Text GoldText;

		void Awake()
		{
			Debug.Log($"Awake!");
			Instance = this;
		}

		private List<Item> shopInventory;

		public void SetShopInventory(List<Item> inventory)
		{
			shopInventory = inventory;
			UpdateShopInventory();
		}

		public void UpdateShopInventory()
		{
			foreach (Transform child in ShopRoot.transform)
			{
				Destroy(child.gameObject);
			}

			foreach (var item in shopInventory)
			{
				var instance = Instantiate(ItemPrefab, ShopRoot.transform);
				instance.transform.Find("Name").GetComponent<Text>().text = item.ItemName;
				instance.transform.Find("Price").GetComponent<Text>().text = $"Price: {item.BuyValue}";

				instance.GetComponent<Button>().onClick.AddListener(() => BuyItem(item));
			}
		}

		public void UpdatePlayerInventory()
		{
			foreach (Transform child in PlayerRoot.transform)
			{
				Destroy(child.gameObject);
			}

			foreach (var item in Player.Instance.Inventory)
			{
				var instance = Instantiate(ItemPrefab, PlayerRoot.transform);
				instance.transform.Find("Name").GetComponent<Text>().text = item.ItemName;
				instance.transform.Find("Price").GetComponent<Text>().text = $"Value: {item.SellValue}";

				instance.GetComponent<Button>().onClick.AddListener(() => SellItem(item));
			}

			GoldText.text = $"Gold: {Player.Instance.Gold}";
		}

		public void BuyItem(Item item)
		{
			if (Player.Instance.Gold < item.BuyValue)
			{
				return;
			}

			Player.Instance.Gold -= item.BuyValue;
			shopInventory.Remove(item);
			Player.Instance.Inventory.Add(item);
			UpdatePlayerInventory();
			UpdateShopInventory();
		}

		public void SellItem(Item item)
		{
			Player.Instance.Gold += item.SellValue;
			Player.Instance.Inventory.Remove(item);
			shopInventory.Add(item);
			UpdatePlayerInventory();
			UpdateShopInventory();
		}
	}
}
