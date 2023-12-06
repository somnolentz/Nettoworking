using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InputUI : MonoBehaviour
{
    public string ipAdressInput;
    public TMP_InputField inputField, nameInputField;
    public Button connectButton;
    private void Awake()
    {
        NetworkManager.instance.ConnectedToServerEvent += LoadIntoGameScene;
    }

    private void OnDestroy()
    {
        NetworkManager.instance.ConnectedToServerEvent -= LoadIntoGameScene;
    }

    void Start()
    {
        nameInputField.text = "Username";
        inputField.text = "127.0.0.1";
        connectButton.onClick.AddListener(() => NetworkManager.instance.Connect(inputField.text, nameInputField.text));
    }

    void Update()
    {
    }

    void LoadIntoGameScene()
    {
        SceneManager.LoadScene(1);
    }
}
