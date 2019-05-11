using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{

    public void StartScene()
    {
        SceneManager.LoadScene("AudioVisualizer");
    }

    public void GoMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Start3DScene()
    {
        SceneManager.LoadScene("3D_AudioVisualizer");
    }

    public void Go3DMenu()
    {
        SceneManager.LoadScene("3D_MainMenu");
    }
}
