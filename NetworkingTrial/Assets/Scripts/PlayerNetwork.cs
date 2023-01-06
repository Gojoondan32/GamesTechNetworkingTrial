using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] protected Camera _camera;
    protected Card _activeCard;
    protected bool _canSelectCard;
    protected bool _cardAnimationFinished;
    

    public bool CanSelectCard {protected get{return _canSelectCard;} set {_canSelectCard = value;}}
    public bool CardAnimationFinished {get{return _cardAnimationFinished;} set{_cardAnimationFinished = value;}}

    [ServerRpc]
    protected void SumbitCardToServerRpc(CardType cardType){
        if(NetworkManager.ConnectedClients.ContainsKey(OwnerClientId)){
            
            //PlayCardAnimClientRpc();
            MatchCalculation.Instance.SumbitCard(this, cardType);
        }
        
    }   

    public void ResetCardsFromServer(){
        if(!IsServer) return;

        ResetCardClientRpc();
    }

    [ClientRpc] 
    private void ResetCardClientRpc(){
        CanSelectCard = true;
        _activeCard.ResetCardPosition();
        _activeCard = null;
    }
}
