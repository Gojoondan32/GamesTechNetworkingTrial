using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TempPlayer : NetworkBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject cube;
    private void Awake() {
        transform.position = AssignPositions.Instance.GetStartingPosition();
        Vector3 centre = new Vector3(0, transform.position.y, 0);
        transform.LookAt(centre);
    }
    private void Start() {
        if (IsOwner)
        {
            Debug.Log("Object owned");
            _camera.gameObject.SetActive(true);
        }
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.T)){
            TestingClientRpc();
        }
    }

    [ClientRpc]
    private void TestingClientRpc(){
        cube.GetComponent<Renderer>().material.color = Color.blue;
    }
}
