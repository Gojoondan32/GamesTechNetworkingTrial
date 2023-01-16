using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AssignStatus : NetworkBehaviour
{
    public static AssignStatus Instance;
    private bool _hostExists;

    private void Awake() {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);

        _hostExists = false;
    }

    public void ConnectToGame(){
        if(!_hostExists){
            
        }
    }
}
