using UnityEngine;
using UnityEngine.UI;

public class SkyboxChanger : MonoBehaviour
{

    public static Material skybox;
    public Material[] Skyboxes;

    /*
    private Dropdown _dropdown;
    public void Awake()
    { 
        _dropdown = GetComponent<Dropdown>();
    }
    */

    void Start()
    {
        skybox = Skyboxes[0];
    }

    public void SkyboxTo1()
    {
        RenderSettings.skybox = Skyboxes[0];
        skybox = Skyboxes[0];
    }

    public void SkyboxTo2()
    {
        RenderSettings.skybox = Skyboxes[1];
        skybox = Skyboxes[1];
    }

    public void SkyboxTo3()
    {
        RenderSettings.skybox = Skyboxes[2];
        skybox = Skyboxes[2];
    }

    public void SkyboxTo4()
    {
        RenderSettings.skybox = Skyboxes[3];
        skybox = Skyboxes[3];
    }

    public void SkyboxTo5()
    {
        RenderSettings.skybox = Skyboxes[4];
        skybox = Skyboxes[4];
    }

}