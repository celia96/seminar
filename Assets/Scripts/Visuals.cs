using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visuals : MonoBehaviour
{
    public static Visuals instance;
    public float timer = 2f;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        StartCoroutine(WaitAndDestroy());
    }

    IEnumerator WaitAndDestroy()
    {
        while (true)
        {
            yield return new WaitForSeconds(timer);
            print("name " + gameObject.name);
            VisualSpawn.instance.BackToOriginal(gameObject.name);
            Destroy(gameObject);
        }
    }
}
