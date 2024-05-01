using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatHUD : MonoBehaviour
{
    public Text nameText;                                   
    public Image healthBar;

   public void SetHUD(Unit unit){                            //setup UI for enemy/player

    nameText.text = unit.CharacterName; 
    UpdateHealthBar(unit.currentHP, unit.maxHP);
               
    

   }

   public void UpdateHealthBar(int currentHP, int maxHP){   
        float fillAmount = (float)currentHP/maxHP;
        healthBar.fillAmount = fillAmount;

   }

    
}