using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MatchManager : MonoBehaviour
{
    public static MatchManager Instance;
    protected Player _player1;
    protected CardType _player1CardType;
    private void Awake() {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);

    }

    public void WaitingRoom(Player playerObj){
       if(_player1 == null){
           _player1 = playerObj;
           return;
       }
       else{
           //Run the animations 
           _player1.PlayAnimationClientRpc();
           playerObj.PlayAnimationClientRpc();
           //SubmitCard(playerObj, playerCardType);
       }
    }

    public void WaitingForSubmit(Player playerObj, CardType playerCardType){
        if(_player1 == null){
            _player1 = playerObj;
            _player1CardType = playerCardType;
            return;
        }
        else SubmitCard(playerObj, playerCardType);
    }

    protected abstract void SubmitCard(Player player2, CardType player2Card);

    protected void ResetCards(MatchData matchData){
        _player1 = null;
        
        matchData.Player1.ResetCardsFromServer();
        matchData.Player2.ResetCardsFromServer();
    }
}
