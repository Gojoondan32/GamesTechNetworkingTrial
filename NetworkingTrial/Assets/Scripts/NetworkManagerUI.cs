using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class NetworkManagerUI : MonoBehaviour
{
    public static NetworkManagerUI Instance;
    #region GameMenu
    [Header("GameMenu")]
    [SerializeField] private Button _serverBtn;
    [SerializeField] private Button _hostBtn;
    [SerializeField] private Button _clientBtn;
    [SerializeField] private GameObject _gameMenu;
    #endregion

    #region GameUI
    [Header("GameUI")]
    [SerializeField] private GameObject _gameUI;
    [SerializeField] private TextMeshProUGUI _winText;
    [SerializeField] private TextMeshProUGUI _lossText;
    #endregion

    private void Awake() {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);

        ShowLobby(true);
        ShowGameUI(false);
        _serverBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartServer();
            ShowLobby(false);
            ShowGameUI(true);
        });
        _hostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            ShowLobby(false);
            ShowGameUI(true);
        });
        _clientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            ShowLobby(false);
            ShowGameUI(true);
        });
    }

    private void ShowLobby(bool value) => _gameMenu.SetActive(value);
    private void ShowGameUI(bool value) => _gameUI.SetActive(value);

    public void UpdateWins(int wins) => _winText.text = wins.ToString();
    public void UpdateLosses(int losses) => _lossText.text = losses.ToString(); 

}
