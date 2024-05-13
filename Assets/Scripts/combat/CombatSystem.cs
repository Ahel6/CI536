
using UnityEngine;
using UnityEngine.UI;

//Enum different states of the battle
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class CombatSystem : MonoBehaviour
{
    //Enemy and player prefabs
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    //Refrence to GO's
     Unit playerUnit;
     Unit enemyUnit;

     public Text Dialogue;

    //Text box with info about combat/actions taken
    public Text combatInfo;
    //Player HUD displaying name, lvl, hp
    public CombatHUD playerHUD;
    public CombatHUD enemyHUD;

    //Current state
    public BattleState state;

    //Gold balance (will need to be made persistant)
     private int goldBalance;

    void Start()
    {
        //Initilize battle state and setup battle
        state = BattleState.START;
        SetupBattle();
    }

    //Set up inital comabat configuration
    void SetupBattle()
    {   

        if (playerPrefab == null || enemyPrefab == null)
    {
        Debug.LogError("PlayerPrefab or EnemyPrefab not assigned!");
        return;
    }
        
        GameObject playerGO = Instantiate(playerPrefab);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab);
        enemyUnit = enemyGO.GetComponent<Unit>();
       
        //Initial comabt info displayed
        Dialogue.text = "You must defeat the" + "" + enemyUnit.CharacterName + "" + "to contiune....";

        //Set up HUDs for player and enemy
         playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);
       
        //Set initial state to players turn
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }


    //Handle actions during players turn
    void PlayerTurn()                                 
    {
        
        //Prompt for player 
        combatInfo.text = "Make your descion:";
    }

    //Handle players attack action
     void PlayerAttack()
    {   
        //Inflict damage on ememy
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        enemyHUD.UpdateHealthBar(enemyUnit.currentHP, enemyUnit.maxHP);
        combatInfo.text = "The attack is successful!";

        //Check if enemy is dead
        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            //Proceed to enemy's turn
            state = BattleState.ENEMYTURN;
            //EnemyTurn();
            Invoke("StartEnemyTurn", 2f); 
        }
    }

    //Player heal actions
    void PlayerHeal()                                                           
    {   
        //Increase player's HP and update HUD
        playerUnit.Heal(50);
        playerHUD.UpdateHealthBar(playerUnit.currentHP, playerUnit.maxHP);
        combatInfo.text = "Your health has increased!";
        
        //Proceed to enemy's turn
        state = BattleState.ENEMYTURN;
        EnemyTurn();
    }

    void StartEnemyTurn(){
        state = BattleState.ENEMYTURN;
    EnemyTurn();
    }
    //Handle enemy's turn
    void EnemyTurn()
    {
        //Enemy attacks the player
        combatInfo.text = enemyUnit.CharacterName + " attacks!";
        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
        playerHUD.UpdateHealthBar(playerUnit.currentHP, playerUnit.maxHP);

        //Check if player is dead
        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            //Proceed to player's turn 
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    //Handle end of battle
    void EndBattle()
    {
        //Display correct message from outcome of battle
        if (state == BattleState.WON)
        {
            combatInfo.text = "You live to fight again!";
            
            //Random amount of gold dropped and added to balance.
             goldBalance = 0;                               
                int goldReward = Random.Range(1,26);                   
                goldBalance += goldReward;
                combatInfo.text = ("Recieved" + goldReward + "gold!");
        }
        else if (state == BattleState.LOST)
        {
            combatInfo.text = "You have died.";
        }
    }
   
    //Button click event handlers for player actions
      public void OnAttackButton()                                      
    {
        if (state != BattleState.PLAYERTURN)
            return;
        PlayerAttack();
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        PlayerHeal();
    }

    public void onDefendButton(){                     

        if(state !=BattleState.PLAYERTURN)
            return;

            //Apply defend action than proceed to enemy's turn
            playerUnit.Defend(); 
            combatInfo.text = "You are defending against the enemy's attack!";
            state = BattleState.ENEMYTURN; 
         EnemyTurn();
            

        
    }

}