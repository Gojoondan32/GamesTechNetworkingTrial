using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum CardType {ROCK, PAPER, SCIZORS}
public enum MatchResult {WIN, LOSE, DRAW}
public class MatchCalculation : MonoBehaviour
{
    public static MatchCalculation Instance;
    [SerializeField] private List<(PlayerSelection playerObj, CardType cardType)> _sumbitedCards;
    //[SerializeField] private List<Tuple<PlayerSelection, CardType>> _sumbitedCards;
    //[SerializeField] private List<CardType> _sumbitedCards;
    private void Awake() {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);

        _sumbitedCards = new List<(PlayerSelection playerObj, CardType cardType)>();
    }
    public void SumbitCard(PlayerSelection obj, CardType cardType){
        _sumbitedCards.Add((obj, cardType));
        //_sumbitedCards.Add(new Tuple<PlayerSelection, CardType>(obj, cardType));
        obj.CanSelectCard = false;
        Debug.Log("Added Card");
        if(_sumbitedCards.Count == 2) Evaluation(_sumbitedCards[0].cardType, _sumbitedCards[1].cardType);
    }

    private void Evaluation(CardType player1, CardType player2){
        if(player1 == player2){
            Debug.Log("draw");
            ResetCards();
            return;
        }
        switch(player1){
            case CardType.ROCK:
                if(player2 == CardType.SCIZORS) Debug.Log("player1 win rock");
                else if(player2 == CardType.PAPER) Debug.Log("player2 win paper");
                break;
            case CardType.PAPER:
                if(player2 == CardType.ROCK) Debug.Log("player1 win paper");
                else if(player2 == CardType.SCIZORS) Debug.Log("player2 win scizors");
                break;
            case CardType.SCIZORS:
                if(player2 == CardType.PAPER) Debug.Log("player1 win scizors");
                else if(player2 == CardType.ROCK) Debug.Log("player2 win rock");
                break;
            default:
                Debug.Log("Invalid card type");
                break;

        }

        ResetCards();
    }

    private void ResetCards(){
        _sumbitedCards.Clear();

        foreach((PlayerSelection playerObj, CardType cardType) player in _sumbitedCards){
            player.playerObj.CanSelectCard = true;
            player.playerObj.ResetCardPosition();
        }
    }
}
