using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSelection : PlayerNetwork {

    
    private void Awake() {
        transform.position = AssignPositions.Instance.GetStartingPosition();
        Vector3 centre = new Vector3(0, transform.position.y, 0);
        transform.LookAt(centre);
        CanSelectCard = true;
    }

    public override void OnNetworkSpawn()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        //if(!IsOwner || !CanSelectCard) return;
        //if(!CanSelectCard) return;
        //if(Display.activeEditorGameViewTarget != _camera.targetDisplay) return;

        if(Input.GetMouseButtonDown(0)){
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, LayerMask.GetMask("Card"))){
                //!This is not working on the client side
                Card card = hit.collider.gameObject.GetComponent<Card>();
                _activeCard = card;
                PlayAnim();
                //SumbitCardToServerRpc(card.CardType);
                
            }
                
                
        }
    }

    public void PlayAnim(){
        //_activeCard.UnselectedCardAnimation();
        _activeCard.PlaceCardAnimation(this);
    }

    public void MatchResult(bool win, CardType cardType){
        Debug.Log($"{this.name} {win} {cardType}");
        
    }

    public void ResetCardPosition(){
        Debug.Log("Reset Card");
        CanSelectCard = true;
        _activeCard.ResetCardPosition();
        _activeCard = null;
        //_rockCard.ResetCardPosition();
    }

    [ServerRpc]
    private void TestServerRpc(CardType cardType){
        if (NetworkManager.ConnectedClients.ContainsKey(OwnerClientId)){
            CardAnimationClientRpc();
            MatchCalculation.Instance.SumbitCard(this, cardType);
            
            //hit.gameObject.GetComponent<Renderer>().material.color = Color.green;
            
            //Ray ray = _camera.ScreenPointToRay(position);
            
            /*
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, LayerMask.GetMask("Card")))
            {
                //_testCube.GetComponent<Renderer>().material.color = Color.green;
                //Card card = hit.collider.gameObject.GetComponent<Card>();
                //_activeCard = card;
                //CardAnimationClientRpc();
                Debug.Log($"Client: {OwnerClientId}");
                //MatchCalculation.Instance.SumbitCard(this, card.CardType);
            }
            */
        }
    }
    [ClientRpc]
    private void CardAnimationClientRpc(){
        _activeCard.PlaceCardAnimation(this);
    }
    [ClientRpc]
    public void ResetCardClientRpc(){
        Debug.Log("Reset Card");
        CanSelectCard = true;
        _activeCard.ResetCardPosition();
        _activeCard = null;
    }

    [ClientRpc]
    private void TestClientRpc(){
        
        
    }
}

