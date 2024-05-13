using Items;
using Player;
namespace Shop
{
    public class ShopObj
    {
        public int ShopGold = 100;
        public List<Item> ForSale = new List<Item>(25);

        //player sells item to shop
        public int PlayerSells(Item Sold)
        {
            ShopGold = ShopGold - Sold.SellValue;
            ForSale.Add(Sold);
            return Sold.BuyValue;
        }

        //PlayerObj buys item from shop
        public int PlayerBuys(Item Bought)
        {
            ShopGold = ShopGold + Bought.SellValue;
            ForSale.Remove(Bought);
            return Bought.SellValue;
        }

    }
}
