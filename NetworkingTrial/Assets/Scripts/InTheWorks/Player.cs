using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(AIPath))]
public class Player : MonoBehaviour
{
    private AIPath _aiPath;
    [SerializeField] private Camera _camera;
    private void Awake() {
        _aiPath = GetComponent<AIPath>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        CheckForInputs();

        RotatePlayerToFacePosition();
    }

    private void CheckForInputs(){
        if(Input.GetMouseButtonDown(0)) SetDestination(GetMousePosition());
        else if(Input.GetKeyDown(KeyCode.F)) Interact();
    }

    private void SetDestination(Vector3 targetPosition){
        _aiPath.destination = targetPosition;
        _aiPath.SearchPath();

        //Animation stuff goes here
    }

    private void Interact(){
        
    }

    private void RotatePlayerToFacePosition(){
        Vector3 directionToTarget = (_aiPath.steeringTarget - transform.position).normalized;
        float rotationSpeed = 20f;
        transform.forward = Vector3.Lerp(transform.forward, directionToTarget, rotationSpeed * Time.deltaTime);
    }

    private Vector3 GetMousePosition(){
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, LayerMask.GetMask("Ground"));
        return hit.point;
    }
}
