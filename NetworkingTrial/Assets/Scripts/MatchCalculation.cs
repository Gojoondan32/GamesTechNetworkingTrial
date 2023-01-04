using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum CardType {ROCK, PAPER, SCIZORS}
public enum MatchResult {WIN, LOSE, DRAW}
public class MatchCalculation : MonoBehaviour
{
    public static MatchCalculation Instance;
    private PlayerSelection player1;
    private CardType player1Card;
    private void Awake() {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);

    }
    public void SumbitCard(PlayerSelection obj, CardType cardType){
        obj.CanSelectCard = false;
        if(player1 == null){
            player1 = obj;
            player1Card = cardType;
        }
        else
            CreateMatch(obj, cardType);
        Debug.Log("Added Card");
    }

    private void CreateMatch(PlayerSelection player2, CardType player2Card){
        MatchData matchData = new MatchData(player1, player2, player1Card, player2Card);
        StartCoroutine(WaitForMatchToStart(matchData));
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

    private void ResetCards(MatchData matchData){
        player1 = null;
        
        matchData.Player1.ResetCardPosition();
        matchData.Player2.ResetCardPosition();
    }
}
