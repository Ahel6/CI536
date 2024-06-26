﻿using System;
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
		public GameObject EnterShopButton;

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
			ShopView.SetActive(false);

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
				default:
					throw new ArgumentOutOfRangeException(nameof(newState), newState, "Invalid UI state!");
			}

			CurrentState = newState;
		}

		public void EnterShop()
		{
			ChangeUIState(UIState.SHOP);
			ShopUI.Instance.UpdatePlayerInventory();
		}

		public void ReturnFromShop()
		{
			ChangeUIState(UIState.EXPLORE);
		}
	}

	public enum UIState
	{
		EXPLORE,
		COMBAT,
		SHOP
	}
}
