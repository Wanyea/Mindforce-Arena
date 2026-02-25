using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the end of a match when a player has lost all health.
/// </summary>
public class EndMatch : NetworkBehaviour
{
    public GameObject WinDisplay;
    public TMP_Text WinText;
    public PlayerLives playerLives;
    public int gameEndTimeSec;

    public IEnumerator playerWin(int player){
        if (player == 0){
            WinText.text = "Player 1 Wins!";
        }
        else{
            WinText.text = "Player 2 Wins!";
        }
        
        WinDisplay.SetActive(true);
        
        yield return new WaitForSeconds(gameEndTimeSec);
        if(IsHost){
            playerLives.ResetPlayerHealthServerRPC();
        }
        //Play explosion animation and set losing player invisible.
        WinDisplay.SetActive(false);
    }

}   
