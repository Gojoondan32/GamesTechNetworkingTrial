using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    #region Variable References
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _testCube;
    #endregion

    #region Accessible Variables

    protected bool _canSelectCard;
    protected bool _cardAnimationFinished;
    public bool CanSelectCard { protected get { return _canSelectCard; } set { _canSelectCard = value; } }
    public bool CardAnimationFinished { get { return _cardAnimationFinished; } set { _cardAnimationFinished = value; } }
    #endregion

    #region Private Variables
    [SerializeField] private Card _activeCard;
    #endregion
    private void Awake() {
        transform.position = AssignPositions.Instance.GetStartingPosition();
        Vector3 centre = new Vector3(0, transform.position.y, 0);
        transform.LookAt(centre);
        CanSelectCard = true;
        _activeCard = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (IsOwner)
        {
            Debug.Log("Object owned");
            _camera.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner || !CanSelectCard) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, LayerMask.GetMask("Card")))
            {
                Card card = hit.collider.gameObject.GetComponent<Card>();
                _activeCard = card;
                CanSelectCard = false;
                PlayAnim();
                StartCoroutine(WaitForAnimationToFinish());
            }
        }

        if(Input.GetKeyDown(KeyCode.L)){
            TestNewClientRpc();
        }
    }

    private void PlayAnim()
    {
        //_activeCard.UnselectedCardAnimation();
        _activeCard.PlaceCardAnimation(this);
    }

    private IEnumerator WaitForAnimationToFinish(){
        yield return new WaitUntil(() => CardAnimationFinished == true);
        SumbitCardToServerRpc(_activeCard.CardType);
    }

    #region Reset Section
    public void ResetCardsFromServer(){
        ResetCardClientRpc();
    }

    private void ResetCardPosition()
    {
        Debug.Log("Reset Card has been called");
        if(_activeCard == null) _testCube.GetComponent<Renderer>().material.color = Color.red;
        else _testCube.GetComponent<Renderer>().material.color = Color.blue;

        CanSelectCard = true;
        _activeCard.ResetCardPosition();
        _activeCard = null;
        //_rockCard.ResetCardPosition();
    }
    #endregion

    #region ServerRPCs
    [ServerRpc]
    private void SumbitCardToServerRpc(CardType cardType){
        MatchCalculation.Instance.SumbitCard(this, cardType);
    }
    #endregion

    #region ClientRPCs
    [ClientRpc]
    private void TestNewClientRpc(){
        if(!IsLocalPlayer) return;
        _testCube.GetComponent<Renderer>().material.color = Color.blue;
    }

    [ClientRpc]
    private void ResetCardClientRpc(){
        if(!IsLocalPlayer) return;
        ResetCardPosition();
    }
    #endregion

}
