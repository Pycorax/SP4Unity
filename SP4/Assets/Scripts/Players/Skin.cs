using System;
using UnityEngine;
using System.Linq;

public class Skin : MonoBehaviour
{
    [Tooltip("The URL to the Skin Sprite.")]
    public string SkinSpriteUrl = "Sprites/Player/Skins";
    [Tooltip("The chance to obtain this Skin.")]
    public int Chance = 10;

    // Stores the Sprite's Sub Sprites that will be loaded on Start()
    private Sprite[] skinSubSprites;

    // Getters
    public Sprite PreviewSprite
    {
        get
        {
            if (skinSubSprites != null)
            {
                // Find the idle sprite and give it
                return (from s in skinSubSprites where s.name == "Idle" select s).First();

            }

            return null;
        }
    }

    public void Save(int id)
    {
        PlayerPrefs.SetString(SaveClass.GetKey(SaveClass.Save_Keys.Key_Skin_URL) + "_" + id, SkinSpriteUrl);
    }

    public void Load(int id)
    {
        SkinSpriteUrl = SaveClass.GetPlayerPrefString(SaveClass.Save_Keys.Key_Skin_URL, "_" + id);
        Load();
    }

    /// <summary>
    /// Use this function to load the Skin Sprites before SwapSkin(). This only needs to be called once for
    /// the whole lifetime of Skin
    /// </summary>
    public void Load()
    {
        // Load all the skin sub sprites
        skinSubSprites = Resources.LoadAll<Sprite>(SkinSpriteUrl);
    }

    /// <summary>
    /// Call this function in the LateUpdate() of the GameObject you wish to skin
    /// </summary>
    /// <param name="spriteRenderer">A reference to the SpriteRenderer of the Player to skin.</param>
    public void SwapSkin(SpriteRenderer spriteRenderer)
    {
        skinSubSprites = Resources.LoadAll<Sprite>(SkinSpriteUrl);

        // If a skin wasn't loaded properly, we don't do anything about it
        if (skinSubSprites == null)
        {
            return;
        }

        // Get name of the sprite
        string currentSpriteName = spriteRenderer.sprite.name;

        // Find the sprite of the same name in the skin
        var skinnedSprite = Array.Find(skinSubSprites, subsprite => subsprite.name == currentSpriteName);
        //var skinnedSprite = skinSubSprites.Find(subsprite => subsprite.name == currentSpriteName);

        // If we found the sprite, sub it in
        if (skinnedSprite != null)
        {
            spriteRenderer.sprite = skinnedSprite;
        }

        //var subSprites = Resources.LoadAll<Sprite>("Characters/Player_Grey");
        //spriteRenderer.sprite = Resources.Load<Sprite>("Characters/Player_Grey");
    }
}
