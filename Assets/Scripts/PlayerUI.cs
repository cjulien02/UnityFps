using UnityEngine;

public class PlayerUI : MonoBehaviour {
    [SerializeField]
    private RectTransform thrusterFuelFill;

    private PlayerController playerController;

    public void SetPlayerController(PlayerController controller)
    {
        playerController = controller;
    }

    private void Update()
    {
        SetAmountFuel(playerController.GetThrusterAmount());
    }

    private void SetAmountFuel(float amount) {
        thrusterFuelFill.localScale = new Vector3(1f, amount, 1f);
    }
}
