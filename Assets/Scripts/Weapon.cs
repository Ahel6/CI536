using UnityEngine;

namespace Assets.Scripts
{
	[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 1)]
	public class Weapon : Item
	{
		public float Damage;
	}
}
