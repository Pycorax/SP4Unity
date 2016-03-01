using UnityEngine;
using System.Collections;

public class GachaponCapsuleAnimationEventHandler : MonoBehaviour
{
    public GachaponScreen GachaponScreenController;

    public void NotifyCapsuleDisappeared()
    {
        GachaponScreenController.NotifyFinishedOpeningCapsule();
    }
}
