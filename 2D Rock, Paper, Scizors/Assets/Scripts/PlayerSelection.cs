using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSelection : NetworkBehaviour {
    [SerializeField] private bool _canSelectCard;
    public bool CanSelectCard {private get{return _canSelectCard;} set {_canSelectCard = value;}}
    
    private void Awake() {
        CanSelectCard = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if(!IsOwner || !CanSelectCard) return;


        if (Input.GetKeyDown(KeyCode.S))
        {
            MatchCalculation.Instance.SumbitCard(this, CardType.ROCK);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MatchCalculation.Instance.SumbitCard(this, CardType.PAPER);
        }
        else if (Input.GetKeyDown((KeyCode.F)))
        {
            MatchCalculation.Instance.SumbitCard(this, CardType.SCIZORS);
        }
    }

    public void MatchResult(bool win, CardType cardType){
        Debug.Log($"{this.name} {win} {cardType}");
        
    }
}

