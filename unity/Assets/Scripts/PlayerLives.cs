using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Synchronizes player lives over the network
/// </summary>
public class PlayerLives : NetworkBehaviour
{
    public NetworkVariable<int> playerOneLives;
    public NetworkVariable<int> playerTwoLives;
    public HealthBar playerOneHealthBar;
    public HealthBar playerTwoHealthBar;

    public EndMatch matchEnder;

    public int startingLives = 10;

    /// <summary>
    /// Checks for when one of the player loses all of their lives
    /// </summary>
    void Update()
    {
        if(playerOneLives.Value <= 0)
        {
            Debug.Log("Player 1 lost all lives. Player 2 wins!");
            StartCoroutine(matchEnder.playerWin(1));
        }
        else if(playerTwoLives.Value <= 0)
        {
            Debug.Log("Player 2 lost all lives. Player 1 wins!");
            StartCoroutine(matchEnder.playerWin(0));
        }
    }
    void Awake(){
        playerOneLives.Value = 10;
        playerTwoLives.Value = 10;
    }

    /// <summary>
    /// Sets and synchronizes both player lives to ten
    /// </summary>
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        //placed objects are server owned by default.
        if(IsOwner){
            Debug.Log("Player Lives started on Server");
            playerOneLives.Value = startingLives;
            playerTwoLives.Value = startingLives;
        }
        
    }
    
    /// <summary>
    /// Sync both of the players lives and properly reduce them with damage
    /// </summary>
    /// <param name="player">
    /// is an integer to differentiate between player one and player two
    /// </param>
    /// <param name="damage">
    /// is an integer that will subtract that many lives from the player
    /// </param>
    [ServerRpc]
    public void DamagePlayerServerRPC(int player, int damage)
    {
        if(player == 0){
            playerOneLives.Value -= damage;
        }
        else{
            playerTwoLives.Value -= damage;
        }
    }

    [ServerRpc]
    public void ResetPlayerHealthServerRPC(){
        playerOneLives.Value = startingLives;
        playerTwoLives.Value = startingLives;
    }    
}
