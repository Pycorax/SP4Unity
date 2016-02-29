using UnityEngine;
using UnityEngine.UI;

public class GachaCapsuleButton : MonoBehaviour
{
    [Tooltip("Reference to the Text that shows the current coins that the player has.")]
    public Text CoinText;

    [Tooltip("Reference to a Gachapon to get the cost.")]
    public Gachapon GachaponReference;

    [Tooltip("Reference to the Player to get the player's number of coins.")]
    public PlayerSettings PlayerSetting;

    // Stores the existing string
    private string prevString;

    void Update()
    {
        if (GetComponent<RectTransform>().rect.Contains(Input.mousePosition))
        {
            Debug.Log("ADAS");
        }
    }

    void OnMouseEnter()
    {
        // Save the string to restore later
        prevString = CoinText.text;

        // Set the text
        CoinText.text = "Coins: " + (PlayerSetting.Coins - GachaponReference.Cost);

        // Set the text colour
        CoinText.color = Color.red;
    }

    void OnMouseExit()
    {
        // Restore the string
        CoinText.text = prevString;

        // Set the text
        CoinText.text = "Coins: " + PlayerSetting.Coins;

        // Set the text colour
        CoinText.color = Color.white;
    }
}
