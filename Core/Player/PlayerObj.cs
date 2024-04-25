
using Items;
using Shop;
using Enemy;
namespace Player
{

    /**
     * Methods return true if successful for cases where the action might not be possible 
     * (selling an item to a shop without enough gold, equipping an item in a slot already used etc)
     * 
     */
    public static class PlayerObj
    {
        static int MaxHealth = 100;
        static int CurrentHealth = MaxHealth;
        static int Gold = 30;
        static int ArmourRating = 0;
        static int AttackPower = 10;
        public static int FloorMult = 1; //multiplier to make enemies more difficult as player advances
        static int CurrentXP = 0;
        static int RequiredXP = 50;



        static List<Item> Inventory = new(25);
        static List<int> EqArmour = new(4); //track equipped Armour - only one num of each in list at any time
        static Weapon EqWeapon = new(1, "Basic Sword", 5, 8, 3);

        //equip item
        private static bool Equip(Armour target)
        {
            if (EqArmour.Contains(target.SlotId)) { return false; }
            EqArmour.Add(target.SlotId);
            return true;
        }

        //Use healing item
        private static void UseHeal(HealthItem item)
        {
            PlayerObj.CurrentHealth = (PlayerObj.CurrentHealth + item.HealValue > MaxHealth) ? MaxHealth : PlayerObj.CurrentHealth + item.HealValue;
        }

        //Sell Item at shop 
        private static bool SellItem(Item soldItem, ShopObj targetShop)
        {
            if (targetShop.ShopGold < soldItem.SellValue) { return false; }
            Inventory.Remove(soldItem);
            Gold += targetShop.PlayerSells(soldItem);
            return true;

        }

        //Buy Item from Shop
        private static bool BuyItem(Item boughtItem, ShopObj targetShop)
        {
            if (Gold < boughtItem.BuyValue) { return false; }
            Inventory.Add(boughtItem);
            Gold -= targetShop.PlayerSells(boughtItem);
            return true;
        }

        private static int CalcArmourRating(List<Armour> Equipped)
        {
            PlayerObj.ArmourRating = 0;
            foreach (var armour in Equipped)
            {
                PlayerObj.ArmourRating += armour.ProtValue;
            }
            return PlayerObj.ArmourRating;
        }

        private static int CalcAttack(Weapon weapon)
        {
            return AttackPower =+ weapon.WeaponAttack;
        }

        private static void EnemeyKill(EnemyObj enemy)
        {
            if (enemy.IsDead())
            {
                PlayerObj.CurrentXP += enemy.XPDropped;
                if (PlayerObj.CurrentXP > RequiredXP)
                {
                    PlayerObj.LevelUp();
                }
            }

        }

        private static void LevelUp()
        {
            PlayerObj.RequiredXP = (int)(12 * Math.Pow(1.25, FloorMult));
            PlayerObj.MaxHealth = (int)(MaxHealth * 1.1);
            PlayerObj.AttackPower = (int)(AttackPower * 1.1);
            PlayerObj.CurrentHealth = MaxHealth;
        }
    }



}
