using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tracks different attributes of spells such as the type of spell, size, speed, etc.
/// </summary>
public class PowerTracker : MonoBehaviour
{   
    /// <summary>
    /// Represents compo power
    /// </summary>
    public int comboPower;

    /// <summary>
    /// Respresents what speed (m/s) the spell should have
    /// </summary>
    public int speed;

    //Indicates what type of spell is selected 
    public int typeSelected;
    
    //Initializes speed to 1 and defaults to the fire spell.
    void Awake(){
        typeSelected = -1;
        speed = 1;
        comboPower = 1;

        // 0 - Fire
        // 1 - Water
        // 2 - Shock
        // 3 - Earth
    }

    /// <summary>
    /// Method for reseting the power tracker after a spell is cast
    /// </summary>
    public void ResetTracker(){
        comboPower = 1;
        speed = 1;
        typeSelected = -1;
        Debug.Log("ResetTracker function call");
    }

    /// <summary>
    /// Public method for increasing the combo meter
    /// </summary>
    public void IncrementPower(){
        comboPower += 1;
    }
    /// <summary>
    /// Public method for setting power to a specified value.
    /// </summary>
    /// <param name="power"> Value to set power to</param>
    public void SetPower(int power){
        comboPower = power;
    }
    
    /// <summary>
    /// Increments spell speed
    /// </summary>
    public void IncrementSpeed(){
        speed += 1;
    }

    /// <summary>
    /// Public method for to select fire, intended for use with Unity's event system.
    /// </summary>
    public void SelectFire(){
        typeSelected = 0;
    }
    /// <summary>
    /// Public method for to select water, intended for use with Unity's event system.
    /// </summary>
    public void SelectWater(){
        typeSelected = 1;
    }
    /// <summary>
    /// Public method for to select lightning, intended for use with Unity's event system.
    /// </summary>
    public void SelectLightning(){
        typeSelected = 2;
    }
    /// <summary>
    /// Public method for to select earth, intended for use with Unity's event system.
    /// </summary>
    public void SelectEarth(){
        typeSelected = 3;
    }
    
}
