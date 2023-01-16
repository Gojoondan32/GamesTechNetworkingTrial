using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSelection : PlayerNetwork {

    private Color[] colours = new Color[4] {Color.blue, Color.green, Color.magenta, Color.yellow};
    private int count = 0;
    private float timer = 0f;

    private void Awake() {
        transform.position = AssignPositions.Instance.GetStartingPosition();
        Vector3 centre = new Vector3(0, transform.position.y, 0);
        transform.LookAt(centre);
        CanSelectCard = true;
        _testCube = transform.GetChild(0).gameObject;
        _testCubeRend = _testCube.GetComponent<Renderer>();
    }
    private void Start() {
        if (IsOwner)
        {
            Debug.Log("Object owned");
            _camera.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if(!IsOwner || !CanSelectCard) return;
        //if(!CanSelectCard) return;
        //if(Display.activeEditorGameViewTarget != _camera.targetDisplay) return;

        if(Input.GetMouseButtonDown(0)){
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, LayerMask.GetMask("Card"))){
                //!This is not working on the client side
                Card card = hit.collider.gameObject.GetComponent<Card>();
                _activeCard = card;
                CanSelectCard = false;
                PlayAnim();
                SumbitCardToServerRpc(_activeCard.CardType);
                
            }   
        }
        
        if(Input.GetKeyDown(KeyCode.G)){
            TestClientRpc();
            //new ClientRpcParams{Send = new ClientRpcSendParams{TargetClientIds = new List<ulong> {1}}}
        }
        
        if(timer >= 1f){
            ColourChange();
            timer = 0f;
        }
        else
            timer += Time.deltaTime;
        


        if(_activeCard == null){
            //_testCubeRend.material.color = Color.red;
        }
        //else 
            //_testCubeRend.material.color = Color.green;
    }

    private int SearchForCard(Card card){
        for(int i = 0; i < _cards.Count; i++){
            if(_cards[i] == card){
                Debug.Log(i);
                return i;
            }
        }
        throw new System.Exception("The card index was not found");
    }

    public void PlayAnim(){
        //_activeCard.UnselectedCardAnimation();
        _activeCard.PlaceCardAnimation(this);
    }

    public void MatchResult(bool win, CardType cardType){
        Debug.Log($"{this.name} {win} {cardType}");
        
    }

    protected override void ResetCardPosition(){
        
        Debug.Log("Reset Card has been called");
        //if(_activeCard == null) return;
        CanSelectCard = true;
        _activeCard.ResetCardPosition();
        _activeCard = null;
        //_rockCard.ResetCardPosition();
    }


    protected override void ChangeColour()
    {
        _testCubeRend.material.color = Color.blue;
    }

    private void ColourChange(){
        if(count >= 4) count = 0;
        _testCubeRend.material.color = colours[count];
        count++;
    }

    [ClientRpc]
    private void CardAnimationClientRpc(){
        _activeCard.PlaceCardAnimation(this);
    }

}

