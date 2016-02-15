using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public enum TimeType
    {
        Normal,
        Game
    }

    private static double[] timeScale = { 1.0, 1.0 };

    public static double GetTimeScale(TimeType type)
    {
        return timeScale[(int)type];
    }

    public static void SetTimeScale(TimeType type, double scale)
    {
        if (type == TimeType.Normal)
        {
            throw new UnityException("Cannot modify Normal time scale! Use other existing TimeTypes or create a new TimeType instead.");
        }

        timeScale[(int)type] = scale;
    }

    public static double GetDeltaTime(TimeType type)
    {
        return Time.deltaTime * timeScale[(int)type];
    }

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
