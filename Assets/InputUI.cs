using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InputUI : MonoBehaviour
{
    public string ipAdressInput;
    public TMP_InputField inputField;
    public Button connectButton;
    // Start is called before the first frame update

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
       
        connectButton.onClick.AddListener(() => NetworkManager.instance.Connect(inputField.text));
    }

    // Update is called once per frame
    void Update()
    {


    }

    void LoadIntoGameScene()
    {
        SceneManager.LoadScene(1);
    }
}
