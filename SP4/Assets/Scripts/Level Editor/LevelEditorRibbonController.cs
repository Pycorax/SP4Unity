using UnityEngine;
using System;
using System.Collections;

public class LevelEditorRibbonController : MonoBehaviour
{
    public enum RibbonViews
    {
        Objective,
        View
    }

    [Tooltip("The default panel to show.")]
    public RibbonViews DefaultView = RibbonViews.Objective;
    [Tooltip("Stores the references to the Ribbon panels")]
    public GameObject[] RibbonPanels = new GameObject[Enum.GetNames(typeof(RibbonViews)).Length];


    void Start()
    {
        switchTo(DefaultView);
    }

    public void SwitchToObjectives()
    {
        switchTo(RibbonViews.Objective);
    }

    public void SwitchToViews()
    {
        switchTo(RibbonViews.View);
    }

    private void switchTo(RibbonViews view)
    {
        disableAllPanels();

        // Activate the panel
        RibbonPanels[(int)view].SetActive(true);
    }

    private void disableAllPanels()
    {
        foreach (var panel in RibbonPanels)
        {
            panel.SetActive(false);
        }
    }
}
