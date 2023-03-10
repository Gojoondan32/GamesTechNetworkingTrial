using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum CardType {ROCK, PAPER, SCIZORS}
public enum MatchResult {WIN, LOSE, DRAW}
public class MatchCalculation : MatchManager
{
    //public static MatchCalculation Instance;
    //[SerializeField] private Player player1;
    private CardType player1Card;
    

    protected override void StartMatch() => Evaluation();

    #region OldStuff
    /*
    public void SumbitCard(Player obj, CardType cardType){
        //obj.CanSelectCard = false;
        Debug.Log($"Match Calculation: {obj.OwnerClientId}");
        if(player1 == null){
            player1 = obj;
            player1Card = cardType;
        }
        else
            CreateMatch(obj, cardType);
        
    }
    */
    protected override void SubmitCard(Player player2, CardType player2Card){
        CreateMatch(player2, player2Card);
    }   

    private void CreateMatch(Player player2, CardType player2Card){
        //MatchData matchData = new MatchData(_player1, player2, player1Card, player2Card);
        //StartCoroutine(WaitForMatchToStart(matchData));
        //Evaluation(matchData);
    }

    private IEnumerator WaitForMatchToStart(MatchData matchData){
        while(matchData.Player2.CardAnimationFinished == false)
            yield return null;
        
        Debug.Log("Finished waiting");
        //Evaluation(matchData);
    }
    #endregion

    private void Evaluation(){
        if(_matchData.Player1Card == _matchData.Player2Card){
            Debug.Log("draw");
            ResetCards();
            return;
        }
        //Compare player 2's card to player 1's card
        switch(_matchData.Player1Card){
            case CardType.ROCK:
                if(_matchData.Player2Card == CardType.SCIZORS) RecordMatchResults(true); //Player 1 win rock
                else if(_matchData.Player2Card == CardType.PAPER) RecordMatchResults(false); //Player 2 win paper
                break;
            case CardType.PAPER:
                if(_matchData.Player2Card == CardType.ROCK) RecordMatchResults(true); //Player 1 win paper
                else if(_matchData.Player2Card == CardType.SCIZORS) RecordMatchResults(false); //Player 2 win scizors
                break;
            case CardType.SCIZORS:
                if(_matchData.Player2Card == CardType.PAPER) RecordMatchResults(true); //Player 1 win scizors
                else if(_matchData.Player2Card == CardType.ROCK) RecordMatchResults(false); //Player 2 win rock
                break;
            default:
                Debug.Log("Invalid card type");
                break;

        }
        
        ResetCards();
    }

    private void RecordMatchResults(bool player1Win){
        if(player1Win){
            RecordMatchResultWin(_matchData.Player1);
            RecordMatchResultLoss(_matchData.Player2);
        }
        else{
            RecordMatchResultWin(_matchData.Player2);
            RecordMatchResultLoss(_matchData.Player1);
        }
    }
}
