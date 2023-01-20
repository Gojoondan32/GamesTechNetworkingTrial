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
        MatchData matchData = new MatchData(_player1, player2, player1Card, player2Card);
        //StartCoroutine(WaitForMatchToStart(matchData));
        Evaluation(matchData);
    }

    private IEnumerator WaitForMatchToStart(MatchData matchData){
        while(matchData.Player2.CardAnimationFinished == false)
            yield return null;
        
        Debug.Log("Finished waiting");
        Evaluation(matchData);
    }

    private void Evaluation(MatchData matchData){
        if(matchData.Player1Card == matchData.Player2Card){
            Debug.Log("draw");
            ResetCards(matchData);
            return;
        }
        switch(matchData.Player1Card){
            case CardType.ROCK:
                if(matchData.Player2Card == CardType.SCIZORS) Debug.Log("player1 win rock");
                else if(matchData.Player2Card == CardType.PAPER) Debug.Log("player2 win paper");
                break;
            case CardType.PAPER:
                if(matchData.Player2Card == CardType.ROCK) Debug.Log("player1 win paper");
                else if(matchData.Player2Card == CardType.SCIZORS) Debug.Log("player2 win scizors");
                break;
            case CardType.SCIZORS:
                if(matchData.Player2Card == CardType.PAPER) Debug.Log("player1 win scizors");
                else if(matchData.Player2Card == CardType.ROCK) Debug.Log("player2 win rock");
                break;
            default:
                Debug.Log("Invalid card type");
                break;

        }
        
        ResetCards(matchData);
    }

    
}
