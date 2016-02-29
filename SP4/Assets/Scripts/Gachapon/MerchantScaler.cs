using System;
using UnityEngine;

public class MerchantScaler : MonoBehaviour
{
    [Tooltip("The offset for the image in 16:9 resolution.")]
    public Vector2 Offset169;

    // Components
    private RectTransform rectTransform;

	// Use this for initialization
    void Start()
    {
        // Initialize the Component
        rectTransform = GetComponent<RectTransform>();

        if (Math.Abs(Camera.main.aspect - 16.0f / 9.0f) < 0.1f)
        {
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, Offset169.y, rectTransform.rect.height);
        }
    }
}
