using UnityEngine;
using System.Collections;

public class ScreenData : MonoBehaviour
{
	public static Vector2 GetScreenSpaceAspectRatio()
	{
		float f = (float)Screen.width / (float)Screen.height;
		int i = 0;
		while (true)
		{
			i++;
			if (System.Math.Round(f * i, 2) == Mathf.RoundToInt(f * i))
				break;
		}
		return new Vector2((float)System.Math.Round(f * i, 2), i);
	}

	public static Vector2 GetScreenSize() // If object is 1x1 unit in size
	{
		/*Bounds b = new Bounds();
		Vector2 aspect = GetAspectRatio();
		float orthoSize = Camera.main.orthographicSize;

		b.center = Vector3.zero;
		b.max = new Vector3(orthoSize / aspect.y * aspect.x, orthoSize);
		b.min = -b.max;
		return b;*/

		Vector2 aspect = GetScreenSpaceAspectRatio();
		float orthoSize = Camera.main.orthographicSize;
		return new Vector2(
			(orthoSize / aspect.y * aspect.x * 2),
			(orthoSize * 2)
			);
	}

	public static Vector2 GetScreenSizeByObject(Vector2 origSize, float pixelPerUnit) // If object is NOT 1x1 unit in size
	{
		/*Bounds b = new Bounds();

		b.center = Vector3.zero;
		b.max = new Vector3(, orthoSize);
		b.min = -b.max;*/

		Vector2 aspect = GetScreenSpaceAspectRatio();
		float orthoSize = Camera.main.orthographicSize;
		return new Vector2(
			(orthoSize / aspect.y * aspect.x * 2) / (origSize.x / pixelPerUnit),
			(orthoSize * 2) / (origSize.y / pixelPerUnit)
			);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
