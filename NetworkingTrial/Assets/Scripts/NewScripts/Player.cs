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

    private bool _canSelectCard;
    public bool CanSelectCard { protected get { return _canSelectCard; } set { _canSelectCard = value; } }
    
    private bool _cardAnimationFinished;
    public bool CardAnimationFinished { get { return _cardAnimationFinished; } set { _cardAnimationFinished = value; } }
    
    private ulong _playerId;
    public ulong PlayerId {get { return _playerId;}}
    #endregion

    #region Private Variables
    [SerializeField] private Card _activeCard;
    [SerializeField] private int _wins;
    [SerializeField] private int _losses;
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
        if (IsOwner) _camera.gameObject.SetActive(true);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _playerId = OwnerClientId;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner || !CanSelectCard) return;
        //if(Display.activeEditorGameViewTarget != _camera.targetDisplay) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, LayerMask.GetMask("Card")))
            {
                Card card = hit.collider.gameObject.GetComponent<Card>();
                _activeCard = card;
                CanSelectCard = false;
                //Send a message to the sever saying that this script is ready to send its data
                CallWaitingRoomOnServerRPC(_activeCard.CardType);
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

        //This looks quite weird but it just gives player time to see what their opponent picked before it resets
        float time = 0f;
        while(time < 2f){
            time += Time.deltaTime;
            yield return null;
        }
        SumbitCardToServerRpc();
    }

    #region Reset Section
    public void ResetCardsFromServer(){
        ResetCardClientRpc();
    }

    private void ResetCardPosition()
    {
        Debug.Log("Reset Card has been called");
        //if(_activeCard == null) _testCube.GetComponent<Renderer>().material.color = Color.red;
        //else _testCube.GetComponent<Renderer>().material.color = Color.blue;

        CanSelectCard = true;
        _activeCard.ResetCardPosition();
        _activeCard = null;
    }
    #endregion

    #region ServerRPCs

    //!cardType must be used as a parameter here because this function is called on the server
    //!as such, the server does not know what the active card of this client is
    [ServerRpc]
    private void CallWaitingRoomOnServerRPC(CardType cardType) => MatchManager.Instance.WaitingRoom(this, cardType); 

    [ServerRpc]
    private void SumbitCardToServerRpc(){
        //MatchCalculation.Instance.SumbitCard(this, cardType);
        MatchManager.Instance.WaitingForSubmit();
    }
    #endregion

    #region ClientRPCs
    [ClientRpc]
    public void PlayAnimationClientRpc(){
        if(!IsLocalPlayer) return;
        PlayAnim();
        StartCoroutine(WaitForAnimationToFinish());
    }

    [ClientRpc]
    private void TestNewClientRpc(){
        if(!IsLocalPlayer) return;
        //_testCube.GetComponent<Renderer>().material.color = Color.blue;
    }

    [ClientRpc]
    public void ResetCardClientRpc(){
        if(!IsLocalPlayer) return;
        ResetCardPosition();
    }


    [ClientRpc]
    public void RecordMatchResultClientRpc(bool result, ClientRpcParams clientRpcParams){
        Debug.Log($"Player {_playerId} {result}");
        if(result){
            _wins++;
            NetworkManagerUI.Instance.UpdateWins(_wins);
        }
        else{
            _losses++;
            NetworkManagerUI.Instance.UpdateLosses(_losses);
        }
    }
    #endregion

}
