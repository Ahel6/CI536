using System.Collections.Generic;

namespace Assets.Scripts
{
	public class ShopManager
	{
		public static ShopManager Instance;
		
		public List<Shop> Shops;

		public void Awake()
		{
			Instance = this;
		}
	}
}
