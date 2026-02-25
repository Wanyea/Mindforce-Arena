using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/// <summary>
/// Interface for translating the players local VR rig to networked references on their spawned player.
/// </summary>
public class NetworkPlayer : NetworkBehaviour
{

    public Transform root;
    public Transform hmd;
    public Transform leftHand;
    public Transform rightHand;
    // Start is called before the first frame update
    public GameObject[] componentsToDisable;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
       
        
        if (IsOwner)
        {
            
            // Vector3 spawnPosition;
            // Quaternion spawnRotation;
            // SpawnManager.instance.GetPlayerSpawnPosition(out spawnPosition, out spawnRotation);
            // GameObject localPlayerRig = GameObject.Find("OVRCameraRigInteraction");
            // localPlayerRig.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            // gameObject.transform.SetPositionAndRotation(spawnPosition,spawnRotation);

            

            foreach (var item in componentsToDisable)
            {
                if(item != null){
                    item.SetActive(false);
                }
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        //only update your spawned player if you own the object.
        if (IsOwner)
        {
            //sync root position and rotation
            root.SetPositionAndRotation(VRTrackingReferences.instance.root.position, VRTrackingReferences.instance.root.rotation);

            hmd.SetPositionAndRotation(VRTrackingReferences.instance.hmd.position, VRTrackingReferences.instance.hmd.rotation);

            leftHand.SetPositionAndRotation(VRTrackingReferences.instance.leftHand.position, VRTrackingReferences.instance.leftHand.rotation);
            
            rightHand.SetPositionAndRotation(VRTrackingReferences.instance.rightHand.position, VRTrackingReferences.instance.rightHand.rotation);
        }
    }

    public void SetVisibility(bool visible){
        if (!visible){
            foreach (var item in componentsToDisable)
            {
                if(item != null){
                    item.SetActive(false);
                }
            }
        }
        else{
            foreach (var item in componentsToDisable)
            {
                if(item != null){
                    item.SetActive(true);
                }
            }
        }
    }
}
