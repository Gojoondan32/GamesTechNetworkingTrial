using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public abstract class MatchManager : MonoBehaviour
{
    public static MatchManager Instance;
    protected MatchData _matchData;


    private void Awake() {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);

        _matchData = new MatchData();
    }

    public void WaitingRoom(Player playerObj, CardType cardType){
        _matchData.AddMatchData(playerObj, cardType);
        Debug.Log("Match data has been added in waiting room");

        if(_matchData.IsReady == false) return;

        //Play the animations
        _matchData.Player1.PlayAnimationClientRpc();
        _matchData.Player2.PlayAnimationClientRpc();
    }

    public void WaitingForSubmit(){
        _matchData.Counter++; //Used to check that both players have finished their animations
        int numberOfPlayers = 2;
        if(_matchData.Counter == numberOfPlayers){
            Debug.Log("Both players finished");
            StartMatch();
        }
    }

    protected abstract void SubmitCard(Player player2, CardType player2Card);
    protected abstract void StartMatch();


    protected void RecordMatchResultWin(Player player){
        player.RecordMatchResultClientRpc(true, new ClientRpcParams {Send = new ClientRpcSendParams {TargetClientIds = new List<ulong> {player.PlayerId}}});
    }
    protected void RecordMatchResultLoss(Player player)
    {
        player.RecordMatchResultClientRpc(false, new ClientRpcParams {Send = new ClientRpcSendParams {TargetClientIds = new List<ulong> {player.PlayerId}}});
    }

    protected void ResetCards(){
        _matchData.Player1.ResetCardClientRpc();
        _matchData.Player2.ResetCardClientRpc();

        //!Reset match data 
        _matchData.Reset();
    }
}
