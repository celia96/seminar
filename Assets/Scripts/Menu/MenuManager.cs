using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [Header("Options Panel")]
    public GameObject StartPanel;
    public GameObject BackgroundsPanel;
    public GameObject FloorsPanel;
    public Material skybox;
    public GameObject floor;

    void Awake()
    {
        // set the default skybox
        RenderSettings.skybox = skybox;
        if (floor) Instantiate(floor);
    }

    public void OpenBackgrounds()
    {
        // enable respective panel
        BackgroundsPanel.SetActive(true);
        StartPanel.SetActive(false);
    }

    public void OpenFloors()
    {
        // enable respective panel
        FloorsPanel.SetActive(true);
        BackgroundsPanel.SetActive(false);
    }

    public void GoBack()
    {
        // enable respective panel
        BackgroundsPanel.SetActive(true);
        FloorsPanel.SetActive(false);
    }
}
