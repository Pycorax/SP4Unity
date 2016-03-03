using UnityEngine;
using System.Collections;

public class GachaponCapsuleAnimationEventHandler : MonoBehaviour
{
    public GachaponScreen GachaponScreenController;

    public void ResetCapsule()
    {
        gameObject.GetComponent<Animator>().enabled = false;
        gameObject.GetComponent<Animator>().enabled = true;
    }

    public void NotifyCapsuleDisappeared()
    {
        GachaponScreenController.NotifyFinishedOpeningCapsule();
    }
}
