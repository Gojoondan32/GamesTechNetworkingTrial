using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignPositions : MonoBehaviour
{
    public static AssignPositions Instance;
    [SerializeField] private Transform _location1, _location2;
    private bool _loaction1InUse;
    private void Awake() {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
        
        _loaction1InUse = false;
    }

    public Vector3 GetStartingPosition(){
        if(!_loaction1InUse){
            _loaction1InUse = true;
            return _location1.position;
        }
        else
            return _location2.position;
    }
 }
