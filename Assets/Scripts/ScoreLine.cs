using UnityEngine;
using UnityEngine.UI;

public class ScoreLine : MonoBehaviour
{
    [SerializeField]
    private Text playerNameText;

    [SerializeField]
    private Text fragsText;

    [SerializeField]
    private Text deadsText;

    public void Setup(string playerName, int frags, int deads)
    {
        playerNameText.text = playerName;
        fragsText.text = frags.ToString();
        deadsText.text = deads.ToString();
    }
}
