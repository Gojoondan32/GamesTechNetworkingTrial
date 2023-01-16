using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button _serverBtn;
    [SerializeField] private Button _hostBtn;
    [SerializeField] private Button _clientBtn;
    [SerializeField] private Button _playBtn; 

    private void Awake() {
        _serverBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartServer();
        });
        _hostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });
        _clientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
        _playBtn.onClick.AddListener(() => {
            CheckStatus();
        });
    }

    private IEnumerator Test(){
        yield return new WaitForSeconds(5f);
        Debug.Log(NetworkManager.Singleton.ConnectedHostname);
    }
    private void CheckStatus(){
        if(NetworkManager.Singleton.ConnectedClientsList.Count <= 0){
            NetworkManager.Singleton.StartHost();
        }
        else NetworkManager.Singleton.StartClient();
    }
}
