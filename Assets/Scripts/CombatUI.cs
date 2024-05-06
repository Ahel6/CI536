using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class CombatUI : MonoBehaviour
	{
		public static CombatUI Instance;

		public Image PlayerHealthbar;
		public Image EnemyHealthbar;

		public Enemy Enemy;

		private void Awake()
		{
			Instance = this;
		}

		public void LoadEnemy(Enemy enemy)
		{
			Enemy = enemy;
		}

		public void UpdateHealthbars()
		{
			PlayerHealthbar.fillAmount = Player.Instance.Health / Player.Instance.MaxHealth;
			EnemyHealthbar.fillAmount = Enemy.CurrentHealth / Enemy.MaxHealth;
		}
	}
}
