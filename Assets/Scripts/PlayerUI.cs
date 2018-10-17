using UnityEngine;

public class PlayerUI : MonoBehaviour {
    [SerializeField]
    private RectTransform thrusterFuelFill;

    private PlayerController playerController;
    private Player player;

    [SerializeField]
    private RectTransform healthFill;

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
    }

    private void SetAmountHealth(float amount)
    {
        Debug.Log("health : " + amount);
        healthFill.localScale = new Vector3(amount, 1f, 1f);
    }

    private void SetAmountFuel(float amount) {
        thrusterFuelFill.localScale = new Vector3(1f, amount, 1f);
    }
}
