using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrangment : MonoBehaviour
{
    [SerializeField] private GameObject _testCube;
    private Vector3 _startPoint;
    private void Awake() {
        _startPoint = new Vector3();
    }
    

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            CalculatePerimeter();
        }
    }

    private void CalculatePerimeter(){
        float numberOfPlayers = 2;
        float degrees = (360 / 100) * (100 / numberOfPlayers);
        float radius = 10;
        float currentDegrees = 0;
        Vector3 position = new Vector3();

        for (int i = 0; i < numberOfPlayers; i++){
            position.x = _startPoint.x + radius * Mathf.Cos(degrees + currentDegrees * Mathf.Deg2Rad);
            position.y = 6;
            position.z = _startPoint.z + radius * Mathf.Sin(degrees + currentDegrees * Mathf.Deg2Rad);
            currentDegrees += degrees;
            Instantiate(_testCube, position, Quaternion.identity);
        }
        
    }
}
