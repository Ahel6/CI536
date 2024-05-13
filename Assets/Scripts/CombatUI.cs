using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

namespace Assets.Scripts
{
	public class CombatUI : MonoBehaviour
	{
		public static CombatUI Instance;

		public Image PlayerHealthbar;
		public Image EnemyHealthbar;

		public Text EnemyName;

		public Text EventLog;
		private string _eventLogString;

		public Enemy Enemy;

		private void Awake()
		{
			Instance = this;
		}

		public void LoadEnemy(Enemy enemy)
		{
			Enemy = enemy;
			EnemyName.text = enemy.Name;

			UpdateHealthbars();

			WriteToEventLog($"- Encountered {enemy.Name}!");
		}

		public void PlayerAttackEnemy(Weapon weapon)
		{
			var damage = weapon.Damage;

			WriteToEventLog($"- Player attacked {Enemy.Name} for {damage}!");
		}

		public void EnemyAttackPlayer(Weapon weapon)
		{

		}

		public void UpdateHealthbars()
		{
			PlayerHealthbar.fillAmount = Player.Instance.Health / Player.Instance.MaxHealth;
			PlayerHealthbar.GetComponentInChildren<Text>().text = Player.Instance.Health.ToString();
			EnemyHealthbar.fillAmount = Enemy.CurrentHealth / Enemy.MaxHealth;
			EnemyHealthbar.GetComponentInChildren<Text>().text = Enemy.CurrentHealth.ToString();
		}

		private const int LINE_COUNT = 14;
		private const int CHAR_COUNT = 20;

		private readonly ListStack<string> _messages = new(false);
		private readonly string[] _lines = new string[LINE_COUNT];

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
	}
}
