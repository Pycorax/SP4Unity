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

    // Original Text Color
    private Color originalTextCol;

    // Components
    private RectTransform rectTransform;

    void Start()
    {
        // Initialize Component References
        rectTransform = GetComponent<RectTransform>();

        // Store the colour for restoring later
        originalTextCol = CoinText.color;
    }

    void Update()
    {
        if ((Input.mousePosition - rectTransform.position).sqrMagnitude < rectTransform.rect.width * rectTransform.rect.width * 0.25f)
        {
            OnMouseEnter();
        }
        else
        {
            OnMouseExit();
        }

        //GetComponent<RectTransform>().position = Input.mousePosition;
    }

    void OnMouseEnter()
    {
        // Set the text
        CoinText.text = "Coins: " + (PlayerSetting.Coins - GachaponReference.Cost);

        // Set the text colour
        CoinText.color = Color.yellow;
    }

    void OnMouseExit()
    {
        // Set the text
        CoinText.text = "Coins: " + PlayerSetting.Coins;

        // Reset the text colour
        CoinText.color = originalTextCol;
    }
}
