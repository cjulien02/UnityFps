using UnityEngine;
using System.Collections.Generic;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    private RectTransform thrusterFuelFill;

    private PlayerController playerController;
    private Player player; 

    [SerializeField]
    private RectTransform healthFill;

    List<GameObject> scoreLineList = new List<GameObject>();

    [SerializeField]
    private GameObject scoreLinePrefab;

    [SerializeField]
    private Transform scoreLineListParent;

    [SerializeField]
    GameObject pauseMenu;

    [SerializeField]
    GameObject scoreMenu;

    private void Start()
    {
        PauseMenu.isON = false;
        pauseMenu.SetActive(false);

        scoreMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SetPlayerController(PlayerController controller)
    {
        playerController = controller;
    }

    public void SetPlayer(Player _player)
    {
        player = _player;
    }

    private void UpdateScores()
    {
        foreach(string playerName in GameManager.GetPlayerScores().Keys)
        {
            GameObject scoreLineGO = Instantiate(scoreLinePrefab);
            scoreLineGO.transform.SetParent(scoreLineListParent);
            ScoreLine sl = (ScoreLine)scoreLineGO.GetComponent<ScoreLine>();

            if(sl != null)
            {
                PlayerScore p = GameManager.GetPlayerScore(playerName);
                sl.Setup(playerName, p.GetFrags(), p.GetDeaths());
            }

            scoreLineList.Add(scoreLineGO);
        }
    }

    private void ClearScore()
    {
        for(int i = 0; i < scoreLineList.Count; i++)
        {
            Destroy(scoreLineList[i]);
        }
        scoreLineList.Clear();
    }

    private void Update()
    {
        

        SetAmountFuel(playerController.GetThrusterAmount());
        SetAmountHealth(player.GetHealth());

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ClearScore();
            UpdateScores();
            scoreMenu.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            scoreMenu.SetActive(false);
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    private void ToggleMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);

        PauseMenu.isON = pauseMenu.activeSelf;

        if (PauseMenu.isON)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;           
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void SetAmountHealth(float amount)
    {
        
        healthFill.localScale = new Vector3(amount, 1f, 1f);
    }

    private void SetAmountFuel(float amount) {
        thrusterFuelFill.localScale = new Vector3(1f, amount, 1f);
    }
}
