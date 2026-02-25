using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Handles the spawning of the spells and syncs them over the network
/// </summary>
public class Spawner : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private NetworkObject prefab;
    [SerializeField] private PowerTracker playerPower;
    [SerializeField] private ManaBar manaBar;
    [SerializeField] private UltimateRadialMenu radialMenu;

    [SerializeField] private ParticleSystem spawnEffect;


    //How to handle dynamic spawning of different orb types and powers?

    //Each spawner has a reference to the players local "power" tracking

    //Power tracking, orb type, orb power, is modified by the local UI and EEG

    //When a player spawns an orb, pass the current power tracking into this spawner

    //Instantiate a new object on the server, then set it's network variables?

    //Do they have to be network variables? 



    /// <summary>
    /// Calls an RPC to instantiate the spell so it is synced on the network
    /// </summary>
    public void SpawnObject()
    {

        if (prefab != null && playerPower.comboPower > 0 && manaBar.HasEnoughMana(30))
        {
            SpawnPrefabServerRPC(transform.position, transform.rotation, playerPower.comboPower, playerPower.speed, playerPower.typeSelected);
            PlaySpawnEffectServerRpc();
            playerPower.ResetTracker();
            manaBar.UseMana(30);
            radialMenu.Disable();
        }

        
    }

    /// <summary>
    /// spawns the spell with an RPC and synchronizes the spells transform so both players see it
    /// </summary>
    /// <param name="spawnPos"> Where the spell spawns</param>
    /// <param name="spawnRot"> The spawn orientation</param>
    /// <param name="size"> Spawned power </param>
    /// <param name="speed"> How fast the spell should move </param>
    /// <param name="type"> Type of the spell (fire, water, shock, earth)</param>
    
    [ServerRpc(RequireOwnership = false)]

    public void SpawnPrefabServerRPC(Vector3 spawnPos, Quaternion spawnRot, int size, int speed, int type)
    {
        Debug.Log("Spawn ServerRPC Called");
        var instance = Instantiate(prefab, spawnPos, spawnRot);
        //testing whether local changes to the object are synced across Network.
        //it works, so I should be able to get a component that stores the orbs behavior, modify then spawn it.

        var Orb = instance.GetComponent<Orb>();
        Orb.speed.Value = speed;
        Orb.size.Value = size;
        Orb.type.Value = type;
        instance.GetComponent<NetworkObject>().Spawn();
        
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlaySpawnEffectServerRpc(){
        PlaySpawnEffectClientRpc();
    }
    
    [ClientRpc]
    public void PlaySpawnEffectClientRpc(){
        spawnEffect.Play();
    }
}
