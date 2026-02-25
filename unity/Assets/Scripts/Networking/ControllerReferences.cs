using Oculus.Interaction.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.BuildingBlocks;
using static Meta.XR.BuildingBlocks.ControllerButtonsMapper;

/// <summary>
/// Central script to create and set OVRInput bindings.
/// </summary>
public class ControllerReferences : MonoBehaviour
{
    //DONT REFER TO META TUTORIAL, this is how you should reference which controller this mapping is set to
    public OVRInput.Controller controller = OVRInput.Controller.None;
    
    //see OG controller buttons mapper for ButtonClickAction definition.^^^
    [SerializeField]
    private List<ButtonClickAction> _buttonClickActions;

    public List<ButtonClickAction> ButtonClickActions
    {
        get => _buttonClickActions;
        set => _buttonClickActions = value;
    }

    /// <summary>
    /// Checks for button clicks
    /// </summary>
    private void Update()
    {
        //OVRInput.Update();
        //basic test example
        /*if (OVRInput.GetDown(OVRInput.Button.One, controller))
        {
            Debug.Log("PRESSED");
        }*/

        foreach (var buttonClickAction in ButtonClickActions)
        {
            ButtonClickAction.ButtonClickMode buttonMode = buttonClickAction.ButtonMode;
            OVRInput.Button button = buttonClickAction.Button;

            if ((buttonMode == ButtonClickAction.ButtonClickMode.OnButtonUp && OVRInput.GetUp(button, controller)) ||
                (buttonMode == ButtonClickAction.ButtonClickMode.OnButtonDown && OVRInput.GetDown(button, controller)) ||
                (buttonMode == ButtonClickAction.ButtonClickMode.OnButton && OVRInput.Get(button,controller)))
            {
                buttonClickAction.Callback?.Invoke();
                Debug.Log(buttonClickAction.Title);
            }
        }
    }
    
}
