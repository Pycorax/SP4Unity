using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkinMenu : MonoBehaviour
{
    [Tooltip("Button reference to the first player on the menu.")]
    public GameObject FirstPlayer;
    [Tooltip("Button reference to the second player on the menu.")]
    public GameObject SecondPlayer;
    [Tooltip("The colour of a unselected player.")]
    public Color UnselectedColor;
    [Tooltip("Reference to the PlayerSettings to make changes.")]
    public PlayerSettings PlayerSetting;

    // Current Player to set Skins on
    private GameObject currentPlayer;               // Stores the current player button selected

    // Original Button Color
    private Color originalButtonColor;

    // Original Player Sprites
    private Sprite originalFirstPlayerSprite;
    private Sprite originalSecondPlayerSprite;

    // Componenets
    private Image firstPlayerButtonImage;
    private Image secondPlayerButtonImage;

    // Use this for initialization
    void Start ()
    {
        // Initialize the components
        firstPlayerButtonImage = FirstPlayer.GetComponent<Image>();
        secondPlayerButtonImage = SecondPlayer.GetComponent<Image>();

        // Save the original color
        originalButtonColor = firstPlayerButtonImage.color;

        // Save the original sprites
        originalFirstPlayerSprite = getChildImage(FirstPlayer).sprite;
        originalSecondPlayerSprite = getChildImage(SecondPlayer).sprite;

        // Select the first player by default
        SelectFirstPlayer();
    }
	

    public void SelectFirstPlayer()
    {
        // Set the Current
        currentPlayer = FirstPlayer;

        // Update the indicator
        firstPlayerButtonImage.color = originalButtonColor;
        secondPlayerButtonImage.color = UnselectedColor;
    }

    public void SelectSecondPlayer()
    {
        // Set the Current
        currentPlayer = SecondPlayer;

        // Update the indicator
        firstPlayerButtonImage.color = UnselectedColor;
        secondPlayerButtonImage.color = originalButtonColor;
    }

    public void ResetCurrentPlayerSkin()
    {
        // Get a reference to the child Image
        var currentPlayerImage = getChildImage(currentPlayer);

        // Set the skin of the currently selected player
        if (currentPlayer == FirstPlayer)
        {
            PlayerSetting.CurrentFirstSkin = null;

            if (currentPlayerImage != null)
            {
                currentPlayerImage.sprite = originalFirstPlayerSprite;
            }
        }
        else if (currentPlayer == SecondPlayer)
        {
            PlayerSetting.CurrentSecondSkin = null;

            if (currentPlayerImage != null)
            {
                currentPlayerImage.sprite = originalSecondPlayerSprite;
            }
        }
    }

    /// <summary>
    /// Skin Buttons should call this function to set the skin
    /// </summary>
    /// <param name="skin">The skin to change to.</param>
    public void SetCurrentPlayerSkin(Skin skin)
    {
        // Set the skin of the currently selected player
        if (currentPlayer == FirstPlayer)
        {
            PlayerSetting.CurrentFirstSkin = skin;
        }
        else if (currentPlayer == SecondPlayer)
        {
            PlayerSetting.CurrentSecondSkin = skin;
        }

        // Update the image
        var currentPlayerImage = getChildImage(currentPlayer);
        if (currentPlayerImage != null)
        {
            currentPlayerImage.sprite = skin.PreviewSprite;
        }
    }

    private Image getChildImage(GameObject go)
    {
        var childComponenets = go.GetComponentsInChildren<Image>();

        return childComponenets.FirstOrDefault(child => child.gameObject != go);

    }
}
