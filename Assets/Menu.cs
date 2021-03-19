using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class Menu : MonoBehaviour
{
    public GameObject MenuButtons;
    public GameObject ShowMenuButton;
    public List<GameObject> models;
    public Interaction interaction;
    public ARPlaneManager aRPlaneManager;

    public void ShowMenu() 
    {
        MenuButtons.SetActive(true);
        ShowMenuButton.SetActive(false);
    }

    public void HideMenu()
    {
        MenuButtons.SetActive(false);
        ShowMenuButton.SetActive(true);
    }

    public void TogglePlanes(Toggle t)
    {
        foreach (var plane in aRPlaneManager.trackables)
            plane.gameObject.SetActive(t.isOn);
    }

    public void SelectGeometry(Dropdown d)
    {
        interaction.model = models[d.value];
    }
}
