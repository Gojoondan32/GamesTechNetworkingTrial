using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Card : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private CardType _cardType;
    public CardType CardType{get{return _cardType;}}

    [Header("Animation Variables")]
    [SerializeField] private Transform[] _movePoints;
    private Vector3[] _movePointPositions;
    private Vector3 _startPos;
    private Quaternion _startRotation;
    private void Awake() {
        _startPos = transform.position;
        _startRotation = transform.rotation;
        _movePointPositions = new Vector3[_movePoints.Length + 1]; //Need to convert transfroms to a vector3 array as leantween only takes vector3

        for(int i = 0; i < _movePoints.Length; i++){
            _movePointPositions[i] = _movePoints[i].position;
        }
        _movePointPositions[_movePointPositions.Length - 1] = _movePointPositions[_movePointPositions.Length - 2];
    }
    public void PlaceCardAnimation(PlayerSelection playerCallback){
        playerCallback.CardAnimationFinished = false;
        LeanTween.rotateX(gameObject, 0, 0.25f);
        LeanTween.moveSpline(gameObject, _movePointPositions, 0.25f);
        StartCoroutine(AnimationFinished(playerCallback));
    }
    public void UnselectedCardAnimation(){

    }
    private IEnumerator AnimationFinished(PlayerSelection playerCallback){
        yield return new WaitForSeconds(0.25f);
        playerCallback.CardAnimationFinished = true;
    }
    public void ResetCardPosition(){
        transform.position = _startPos;
        transform.rotation = _startRotation;
        Debug.Log("Reseting");
    }
}
