using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    
   
  public string CharacterName;
  public string level;
  public int damage;
  public int maxHP;
 public int currentHP;

//Calculate damage taken by the unit
 public bool TakeDamage(int dmg)
	{
        //If defending reduce damage by 80%
        if(IsDefending)
        {
            dmg = Mathf.RoundToInt(dmg * 0.2f);
            StopDefending();
        }
		currentHP -= dmg;

		if (currentHP <= 0)
			return true;
		else
			return false;
	}

	


 public void Heal(int amount){
currentHP += amount;

if(currentHP > maxHP){

    currentHP = maxHP;
}

 }

 
    public bool IsDefending { get; private set; }

    public void Defend()
    {
        IsDefending = true;
    }

    public void StopDefending()
    {
        IsDefending = false;
    }
    

}
