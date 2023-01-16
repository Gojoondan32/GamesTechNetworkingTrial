using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public abstract class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] protected Camera _camera;
    [SerializeField] protected GameObject _testCube;
    protected Renderer _testCubeRend;

    #region Variables
    [SerializeField] protected List<Card> _cards;
    [SerializeField] protected Card _activeCard;
    protected bool _canSelectCard;
    protected bool _cardAnimationFinished;


    public bool CanSelectCard { protected get { return _canSelectCard; } set { _canSelectCard = value; } }
    public bool CardAnimationFinished { get { return _cardAnimationFinished; } set { _cardAnimationFinished = value; } }
    #endregion

    #region Abstract Methods
    protected abstract void ResetCardPosition();
    protected abstract void ChangeColour();
    #endregion

    public override void OnNetworkSpawn(){
        Debug.Log("Testing");
    }
    

    #region RPC's
    [ServerRpc]
    protected void SumbitCardToServerRpc(CardType cardType){
        if(NetworkManager.ConnectedClients.ContainsKey(OwnerClientId)){
            
            //PlayCardAnimClientRpc();
            //_activeCard = _cards[index];
            MatchCalculation.Instance.SumbitCard(this, cardType);
        }
        
    }   

    public void ResetCardsFromServer(){
        if(!IsHost) return;

        ResetCardClientRpc();
    }

    [ClientRpc] 
    private void ResetCardClientRpc(){
        _testCubeRend.material.color = Color.green;
        //ResetCardPosition();
    }

    [ClientRpc]
    protected void TestClientRpc(){
        
        if(!IsLocalPlayer) return;
        ChangeColour();
        Debug.Log("Client RPC Ran" + OwnerClientId);
    }
    #endregion
    
}
