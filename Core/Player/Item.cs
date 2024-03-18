namespace Items
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

    public class HealthItem : Item
    {
        public int HealValue;
        public HealthItem(int SetItemID, string SetitemName, int SetSellValue, int SetBuyValue, int setHealValue) : base(SetItemID, SetitemName, SetSellValue, SetBuyValue)
        {
            this.HealValue = setHealValue;
        }

    }

    public class Weapon : Item
    {
        public int WeaponAttack;
        const int SlotID = 4;
        public Weapon(int SetItemID, string SetitemName, int SetSellValue, int SetBuyValue, int SetAttack) : base(SetItemID, SetitemName, SetSellValue, SetBuyValue)
        {
            this.WeaponAttack = SetAttack;
        }
    }

    public class Armour : Item
    {

        public int SlotId;  //0 for head, 1 chest, 2 leg, 3 boot, 4 weapon
        public int ProtValue;
        public Armour(int SetItemID, string SetitemName, int SetSellValue, int SetBuyValue, int SetSlotId, int SetProtValue) : base(SetItemID, SetitemName, SetSellValue, SetBuyValue)
        {
            this.SlotId = SetSlotId;
            this.ProtValue = SetProtValue;
        }



        static void Main(string[] args)
        {
            Armour testArmour = new(1, "Iron chestpiece", 20, 30, 2, 3);
            Armour testArmour2 = testArmour;

            Item testItem = new(2, "Generic item", 10, 20);

        }
    }
}
