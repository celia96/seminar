using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject defaultFloor;
    // Use this for initialization
    void Awake()
    {
        // Set the skybox that was set from the menu
        if (SkyboxChanger.skybox)
        {
            RenderSettings.skybox = SkyboxChanger.skybox;
        }

        if (FloorChanger.floor)
        {
            GameObject Floor = FloorChanger.floor;
            // Instantiate the floor that was set from the menu
            Instantiate(Floor);
        } else
        {
            Instantiate(defaultFloor);
        }
    }


}
