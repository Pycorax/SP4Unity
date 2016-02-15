using UnityEngine;
using System.Collections;

public class SpriteData : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static Vector2 GetNativeSize(Sprite sprite)
    {
        return sprite.textureRect.max;
    }
}