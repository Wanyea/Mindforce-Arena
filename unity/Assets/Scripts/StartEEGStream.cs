using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEEGStream : MonoBehaviour
{
    public void CheckEEGStream()
    {
        MentalCommands mentalCommandsInstance = GameObject.FindObjectOfType<MentalCommands>();

        if(mentalCommandsInstance != null)
        {
            mentalCommandsInstance.CheckEEGStream();
        }
        else
        {
            Debug.LogError("MentalCommands script instance not found in the scene.");
        }
    }
}
