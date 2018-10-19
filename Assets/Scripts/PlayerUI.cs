using UnityEngine;

public class PlayerUI : MonoBehaviour {
    [SerializeField]
    private RectTransform thrusterFuelFill;

    private PlayerController playerController;
    private Player player;

    [SerializeField]
    private RectTransform healthFill;


    [SerializeField]
    GameObject pauseMenu;

    private void Start()
    {
        PauseMenu.isON = false;
        pauseMenu.SetActive(false);

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

    private void Update()
    {
        SetAmountFuel(playerController.GetThrusterAmount());
        SetAmountHealth(player.GetHealth());

        if(Input.GetKeyUp(KeyCode.Escape))
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
