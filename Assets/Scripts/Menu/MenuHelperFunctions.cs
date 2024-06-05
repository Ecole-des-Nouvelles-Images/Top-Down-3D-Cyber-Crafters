using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuHelperFunctions : MonoBehaviour
{
    public static bool[] playersReady = new bool[4]{ false, false, false, false };
    public static bool[] playersJoined = new bool[4] { false, false, false, false };

    public List<GameObject> playerModels = new List<GameObject>();
    public int modelId;
    public bool isModelCanvas;

    public UnityEvent OnOpenMenu;


    //lobby variables
    public int playerIndex = 1;
    public GameObject playerPanel;
    public bool p1;
    public bool p2;
    public bool p3;
    public bool p4;

    private void Start()
    {
        OnOpenMenu.Invoke();
    }

    private void Update()
    {
        if (isModelCanvas)
        {
            ChangeModel();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void ChangeModel()
    {
        if (GetComponent<PlayerInput>().actions["Left"].WasPressedThisFrame())
        {
            modelId -= 1;
        }
        if (GetComponent<PlayerInput>().actions["Right"].WasPressedThisFrame())
        {
            modelId += 1;
        }
        if (modelId < 0)
        {
            modelId = 3;
        }
        if (modelId > 3)
        {
            modelId = 0;
        }
        for (int i = 0; i < playerModels.Count; i++)
        {
            if (i == modelId)
            {
                playerModels[i].SetActive(true);
            }
            else
            {
                playerModels[i].SetActive(false);
            }
        }

        if (p1)
        {
            PlayerModelIndexer.Instance.player1ModelId = modelId;
        }
        else if (p2)
        {
            PlayerModelIndexer.Instance.player2ModelId = modelId;
        }
        else if (p3)
        {
            PlayerModelIndexer.Instance.player3ModelId = modelId;
        }
        else if (p4)
        {
            PlayerModelIndexer.Instance.player4ModelId = modelId;
        }
    }

    public void PlayerJoined()
    {
        GameObject.Find("CountdownTimer").GetComponent<Text>().text = "";
        foreach (MenuHelperFunctions menuHelper in GameObject.FindObjectsOfType<MenuHelperFunctions>())
        {
            menuHelper.StopCoroutine("LobbyCountdown");
        }
        playersJoined[playerIndex - 1] = true;
    }

    public void PlayerReady()
    {
        playersReady[playerIndex - 1] = true;

        bool allPlayersReady = true;
        int readyCount = 0;
        for(int i = 0; i < playersJoined.Length; i++)
        {
            if(playersJoined[i] == true)
            {
                if (playersReady[i] == false)
                {
                    allPlayersReady = false;
                }
                else
                {
                    readyCount++;
                }
            }
        }

        if(allPlayersReady == true && readyCount > 1)
        {
            StartCoroutine("LobbyCountdown");
        }
    }


    IEnumerator LobbyCountdown()
    {
        int countdownDuration = 5;

        int remainingTime = countdownDuration;
        for(int i = 0; i < countdownDuration; i++)
        {
            GameObject.Find("CountdownTimer").GetComponent<Text>().text = "Start\n" + remainingTime.ToString();
            yield return new WaitForSeconds(1);
            remainingTime--;
        }

        LoadScene("GameScene");
    }

    public void OnCancel()
    {
        if (playersReady[playerIndex - 1] == true)
        {
            playerPanel.transform.Find("Get Ready Panel").transform.Find("ReadyButton").gameObject.SetActive(true);
            playerPanel.GetComponent<Animator>().Play("RotatePlayerModel");
            playersReady[playerIndex - 1] = false;
            playerPanel.transform.Find("EventSystemPlayer").GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(null);
            playerPanel.transform.Find("EventSystemPlayer").GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(playerPanel.transform.Find("Get Ready Panel").transform.Find("ReadyButton").gameObject);

            GameObject.Find("CountdownTimer").GetComponent<Text>().text = "";
            foreach (MenuHelperFunctions menuHelper in GameObject.FindObjectsOfType<MenuHelperFunctions>())
            {
                menuHelper.StopCoroutine("LobbyCountdown");
            }
            Debug.Log("Cancel Ready");
        }
        else if (playersJoined[playerIndex -1] == true)
        {
            playerPanel.transform.Find("Get Ready Panel").gameObject.SetActive(false);
            playerPanel.transform.Find("Join Panel").gameObject.SetActive(true);
            playerPanel.transform.Find("EventSystemPlayer").GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(null);

            playerPanel.transform.Find("EventSystemPlayer").GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(playerPanel.transform.Find("Join Panel").transform.Find("JoinButton").gameObject);

            playerPanel.GetComponent<Animator>().Play("RotatePlayerModel");
            playersJoined[playerIndex - 1] = false;
            Debug.Log("Cancel Join" + playerIndex);
        }
        else if(playerIndex == 1)
        {
            LoadScene("Lobby");
        }
    }
}
