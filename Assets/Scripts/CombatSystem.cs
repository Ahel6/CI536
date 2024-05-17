using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class CombatSystem : MonoBehaviour
	{
		public static CombatSystem Instance;

		public Image PlayerHealthbar;
		public Image EnemyHealthbar;

		public Text EnemyName;

		public Text EventLog;
		private string _eventLogString;

		public Enemy Enemy;

		public Button AttackButton;
		public Button DefendButton;
		public Button HealButton;

		private BattleState state;
		private bool isDefending;

		private void Awake()
		{
			Instance = this;
		}

		public void LoadEnemy(Enemy enemy)
		{
			Enemy = enemy;
			enemy.CurrentHealth = enemy.MaxHealth;
			EnemyName.text = enemy.Name;

			UpdateHealthbars();

			WriteToEventLog($"- Encountered {enemy.Name}!");

			state = BattleState.PLAYERTURN;
		}

		public void PlayerAttackEnemy(Weapon weapon)
		{
			var damage = weapon.Damage;

			WriteToEventLog($"- Player attacked {Enemy.Name} for {damage} damage!");
			EnemyTakeDamage(damage);
		}

		public void EnemyAttackPlayer(Weapon weapon)
		{
			var damage = weapon.Damage;

			if (isDefending)
			{
				damage /= 2;
			}

			isDefending = false;
			WriteToEventLog($"- {Enemy.Name} attacked for {damage} damage!");
			PlayerTakeDamage(damage);
		}

		public void PlayerTakeDamage(float damage)
		{
			Player.Instance.Health -= damage;

			if (Player.Instance.Health <= 0)
			{
				state = BattleState.LOST;
				// end battle
			}
			else
			{
				state = BattleState.PLAYERTURN;
			}

			UpdateHealthbars();
		}

		public void EnemyTakeDamage(float damage)
		{
			Enemy.CurrentHealth -= damage;

			if (Enemy.CurrentHealth <= 0)
			{
				state = BattleState.WON;
				WriteToEventLog($"- {Enemy.name} was defeated! You win!");
				WriteToEventLog($"- You earnt 5 gold.");
				Player.Instance.Gold += 5;
				Invoke("ExitCombat", 1f);
			}
			else
			{
				state = BattleState.ENEMYTURN;
				Invoke("StartEnemyTurn", 1f);
			}

			UpdateHealthbars();
		}

		public void ExitCombat()
		{
			ClearEventLog();
			Player.Instance.ExitCombat();
		}

		void StartEnemyTurn()
		{
			state = BattleState.ENEMYTURN;

			EnemyAttackPlayer(Enemy.Weapon);
		}

		public void UpdateHealthbars()
		{
			var playerHealth = Player.Instance.Health / Player.Instance.MaxHealth;
			var redPlayerHealthbar = PlayerHealthbar.transform.GetChild(0).GetComponent<RectTransform>();
			redPlayerHealthbar.offsetMax = new Vector2(-(2 + (376.49f * (1 -playerHealth))), redPlayerHealthbar.offsetMax.y);
			PlayerHealthbar.GetComponentInChildren<Text>().text = Player.Instance.Health.ToString();

			var enemyHealth = Enemy.CurrentHealth / Enemy.MaxHealth;
			var redEnemyHealthbar = EnemyHealthbar.transform.GetChild(0).GetComponent<RectTransform>();
			redEnemyHealthbar.offsetMax = new Vector2(-(2 + (376.49f * (1 - enemyHealth))), redEnemyHealthbar.offsetMax.y);
			EnemyHealthbar.GetComponentInChildren<Text>().text = Enemy.CurrentHealth.ToString();
		}

		private const int LINE_COUNT = 14;
		private const int CHAR_COUNT = 30;

		private readonly ListStack<string> _messages = new();
		private readonly string[] _lines = new string[LINE_COUNT];

		public void ClearEventLog()
		{
			_messages.Clear();
			for (var i = 0; i < _lines.Length; i++)
			{
				_lines[i] = null;
			}

			EventLog.text = string.Empty;
		}

		public void WriteToEventLog(string message)
		{
			_messages.Push(message);

			if (_messages.Count > LINE_COUNT)
			{
				_messages.PopFromBack();
			}

			var currentLineIndex = LINE_COUNT - 1;

			foreach (var msg in _messages.Reverse())
			{
				var characterCount = msg.Length;
				var linesNeeded = Mathf.CeilToInt((float)characterCount / CHAR_COUNT);
				var chunk = 0;
				for (var i = linesNeeded - 1; i >= 0; i--)
				{
					if (currentLineIndex - i < 0)
					{
						chunk++;
						continue;
					}

					var chunkString = string.Concat(msg.Skip(CHAR_COUNT * chunk).Take(CHAR_COUNT));
					_lines[currentLineIndex - i] = chunkString;
					chunk++;
				}

				currentLineIndex -= linesNeeded;

				if (currentLineIndex < 0)
				{
					break;
				}
			}

			var finalText = "";
			foreach (var line in _lines)
			{
				var msg = line;

				if (line == default)
				{
					finalText += Environment.NewLine;
				}
				else if (msg.Length == CHAR_COUNT + 1)
				{
					finalText += msg;
				}
				else
				{
					finalText += $"{msg}{Environment.NewLine}";
				}

			}

			EventLog.text = finalText;
		}

		public void OnAttackButton()
		{
			if (state != BattleState.PLAYERTURN)
			{
				return;
			}

			PlayerAttackEnemy(Player.Instance.Weapon);
		}

		public void OnHealButton()
		{
			if (state != BattleState.PLAYERTURN)
			{
				return;
			}

			WriteToEventLog($"- Player is healing...");

			Player.Instance.Health += 1;
			if (Player.Instance.Health > Player.Instance.MaxHealth)
			{
				Player.Instance.Health = Player.Instance.MaxHealth;
			}
			UpdateHealthbars();

			state = BattleState.ENEMYTURN;
			Invoke("StartEnemyTurn", 1f);
		}

		public void OnDefendButton()
		{
			if (state != BattleState.PLAYERTURN)
			{
				return;
			}

			WriteToEventLog($"- Player is defending! Damage taken is halfed...");
			isDefending = true;
			state = BattleState.ENEMYTURN;
			Invoke("StartEnemyTurn", 1f);
		}
	}
}
