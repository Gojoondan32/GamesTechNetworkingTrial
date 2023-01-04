using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSelection : NetworkBehaviour {
    [SerializeField] private Camera _camera;
    private Card _activeCard;
    private bool _canSelectCard;
    private bool _cardAnimationFinished;
    

    public bool CanSelectCard {private get{return _canSelectCard;} set {_canSelectCard = value;}}
    public bool CardAnimationFinished {get{return _cardAnimationFinished;} set{_cardAnimationFinished = value;}}
    
    private void Awake() {
        CanSelectCard = true;
    }

    // Update is called once per frame
    private void Update()
    {
        //if(!IsOwner || !CanSelectCard) return;
        //if(!CanSelectCard) return;
        if(Display.activeEditorGameViewTarget != _camera.targetDisplay) return;

        if(Input.GetMouseButtonDown(0)){
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, LayerMask.GetMask("Card"))){
                Card card = hit.collider.gameObject.GetComponent<Card>();
                _activeCard = card;
                card.PlayCardAnimation(this);
                MatchCalculation.Instance.SumbitCard(this, card.CardType);
            }
                
        }
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
}

