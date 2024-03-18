
using Player;
using System.Runtime.CompilerServices;
using static System.Random;
namespace Enemy
{
    public class EnemyObj
    {
        public int Health;
        public int EnemyAttack;
        public const double Multiplier = 1.3;
        public int GoldDropped;
        public int XPDropped;

        public static Random rnd = new Random();

        public static void CreateStats(EnemyObj enemy)
        {
            enemy.Health = (int)(12 + Math.Pow(Multiplier, PlayerObj.FloorMult) + rnd.Next(-5, 11));
            enemy.GoldDropped = (int)(7 + Math.Pow(Multiplier, PlayerObj.FloorMult) + rnd.Next(-3, 12));
            enemy.XPDropped = (int)(10 * Math.Pow(1.2, PlayerObj.FloorMult) + rnd.Next(-5, 11));
        }

        public bool IsDead()
        {
            if (this.Health <= 0)
            {
                return true;
            }
            return false;
        }
    }


}
