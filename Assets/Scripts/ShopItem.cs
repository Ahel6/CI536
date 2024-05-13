using UnityEngine;

namespace Assets.Scripts
{
	[CreateAssetMenu(fileName = "ShopItem", menuName = "ScriptableObjects/ShopItem")]
	public class ShopItem : ScriptableObject
	{
		public string Name;
		public Sprite Icon;
	}
}
