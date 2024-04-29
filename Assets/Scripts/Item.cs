namespace Assets.Scripts
{
	public class Item
	{
		public int ItemID;
		public string ItemName;
		public int SellValue;
		public int BuyValue;

		public Item(int SetItemID, string SetitemName, int SetSellValue, int SetBuyValue)
		{
			this.ItemID = SetItemID;
			this.ItemName = SetitemName;
			this.SellValue = SetSellValue;
			this.BuyValue = SetBuyValue;
		}

		public int GetID() => this.ItemID;
	}
}