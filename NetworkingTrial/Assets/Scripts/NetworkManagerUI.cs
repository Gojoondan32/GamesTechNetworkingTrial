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

    private void Awake() {
        _serverBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartServer();
            HideLobby();
        });
        _hostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            HideLobby();
        });
        _clientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            HideLobby();
        });
    }

    private void HideLobby(){
        gameObject.SetActive(false);
    }

}
