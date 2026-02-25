using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;

/// <summary>
/// Handles collision between spell and an invisible 'backstop' to damage player health
/// </summary>
public class BackstopCollision : NetworkBehaviour
{
    /// <summary>
    /// Reference to Player Lives tracker.
    /// </summary>
    public PlayerLives playerTracker;
    /// <summary>
    /// indicates what player this backstop should damage, 0 = p1, 1 = p2.
    /// </summary>
    public int assignedPlayer;

    /// <summary>
    /// Synchronizes collision over network
    /// </summary>
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        //Only run collision tracking on server.
        if(!IsOwner){
            this.enabled = false;
        }
    }

    /// <summary>
    /// Checks for collision between spell and backstop
    /// </summary>
    /// <param name="other">
    /// refers to the object colliding with the backstop
    /// </param>
    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Orb"){
            
            playerTracker.DamagePlayerServerRPC(assignedPlayer, other.gameObject.GetComponent<Orb>().size.Value);
            
        }
    }
}
