using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
	public class UIManager : MonoBehaviour
	{
		public static UIManager Instance { get; private set; }

		public UIState CurrentState { get; private set; }

		public GameObject MapView;
		public GameObject CombatView;
		public GameObject ShopView;

		private void Awake()
		{
			Instance = this;
		}

		public void ChangeUIState(UIState newState)
		{
			if (CurrentState == newState)
			{
				return;
			}

			MapView.SetActive(false);
			CombatView.SetActive(false);

			switch (newState)
			{
				case UIState.EXPLORE:
					MapView.SetActive(true);
					break;
				case UIState.COMBAT:
					CombatView.SetActive(true);
					break;
				case UIState.SHOP:
					ShopView.SetActive(true);
					break;
			}

			CurrentState = newState;
		}
	}

	public enum UIState
	{
		EXPLORE,
		COMBAT,
		SHOP
	}
}
