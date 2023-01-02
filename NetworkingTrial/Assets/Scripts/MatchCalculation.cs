using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum CardType {ROCK, PAPER, SCIZORS, DRAW}
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
            _sumbitedCards[0].playerObj.MatchResult(false, CardType.DRAW);
            _sumbitedCards[1].playerObj.MatchResult(false, CardType.DRAW);
            ResetCards();
            return;
        }
        switch(player1){
            case CardType.ROCK:
                if(player2 == CardType.SCIZORS) _sumbitedCards[0].playerObj.MatchResult(true, player1);
                else if(player2 == CardType.PAPER) _sumbitedCards[1].playerObj.MatchResult(true, player2);
                break;
            case CardType.PAPER:
                if(player2 == CardType.ROCK) _sumbitedCards[0].playerObj.MatchResult(true, player1);
                else if(player2 == CardType.SCIZORS) _sumbitedCards[0].playerObj.MatchResult(true, player1);
                break;
            case CardType.SCIZORS:
                if(player2 == CardType.PAPER) _sumbitedCards[0].playerObj.MatchResult(true, player1);
                else if(player2 == CardType.ROCK) _sumbitedCards[0].playerObj.MatchResult(true, player1);
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
        }
    }
}
