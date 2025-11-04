using TMPro;
using UnityEngine;

public class PlayerUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;
    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }
}
