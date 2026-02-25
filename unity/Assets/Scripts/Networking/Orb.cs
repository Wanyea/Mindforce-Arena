using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Netcode;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

/// <summary>
/// Base class defining behavior for all spells
/// </summary>
public class Orb : NetworkBehaviour
{
    /// <summary>
    /// Property for orb speed.
    /// </summary>
    public NetworkVariable<int> speed;
    /// <summary>
    /// Property for the "power level" of the orb.
    /// </summary>
    public NetworkVariable<int> size;

    /// <summary>
    /// Property for the spell type of the orb
    /// </summary>
    public NetworkVariable<int> type;
    /// <summary>
    /// float value to adjust how much an orb scales per unit of power/size
    /// </summary>
    public float orbScaling;
    //public Material[] orbTypes;
    /// <summary>
    /// Holds references to the particle effects that define spell type, activated by set Type value;
    /// </summary>
    public GameObject[] spellTypes;

    Rigidbody orbRigidBody;
    public TMP_Text text;
    private bool orbColliding;

    
    void Start(){
        text = GetComponentInChildren<TMP_Text>();
        orbRigidBody = GetComponent<Rigidbody>();
        UpdateOrbVisual();
        orbColliding = false;
        orbRigidBody.velocity = transform.forward * speed.Value;
        

    }
    void Update(){
        //update text elements
        if(text != null)
        {
            text.text = size.Value.ToString();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        
        //block of code testing despawn on colliding with tagged objects.
        if (other.gameObject.tag == "Orb")
        {
            
            //Server side orb state tracking, server runs logic and syncs to remote client orbs.
            if(IsHost && !orbColliding){
                //set orb colliding flag to prevent double triggers.
                orbColliding = true;

                int myPower = size.Value;
                int oppPower = other.gameObject.GetComponent<Orb>().size.Value;

                int myType = type.Value;
                int oppType = other.gameObject.GetComponent<Orb>().type.Value;

                //Elemental type logic goes before final interaction calc
                //0,1,2,3 Fire, Water, Lightning Earth
                //Orbs only need to know if they should be stronger or weaker vs a type
                if(myType == 0){
                    if(oppType == 3){
                        myPower += 1;
                    }
                    if(oppType == 1){
                        oppPower += 1;
                    }  
                }
                else if (myType == 1){
                    if(oppType == 0){
                        myPower += 1;
                    }
                    if(oppType == 2){
                        oppPower += 1;
                    }  
                }
                else if (myType == 2){
                    if(oppType == 1){
                        myPower += 1;
                    }
                    if(oppType == 3){
                        oppPower += 1;
                    }  
                }
                else if (myType == 3){
                    if(oppType == 2){
                        myPower += 1;
                    }
                    if(oppType == 0){
                        oppPower += 1;
                    }  
                }
                
                //after type bonuses are applied, now run calculation.
                if(myPower <= oppPower){
                    GetComponent<NetworkObject>().Despawn();
                    
                }
                else{
                    size.Value = myPower - oppPower;
                    UpdateOrbVisual();
                    orbColliding = false;
                }
            }
        }
        //Does not yet do damage to unique players, need some solution where each player is assigned their backstop on spawn.
        //Damage to players is now handled in the backstop gameObject near each player position.
        if (other.gameObject.tag == "Backstop")
        {
            
            GetComponent<NetworkObject>().Despawn();
        }

        
    }

    

    /// <summary>
    /// changes the size of the spell depending on the size value
    /// spawns the correct particle effects depending on what spell is created
    /// handles scaling of particle effects
    /// </summary>
    void UpdateOrbVisual(){

        //just basic scaling change for now.
        float orbSize = size.Value*orbScaling;
        gameObject.transform.localScale = new Vector3(orbSize,orbSize,orbSize);

        spellTypes[type.Value].SetActive(true);
    }
        
}
