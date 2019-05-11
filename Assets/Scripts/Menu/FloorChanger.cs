using UnityEngine;
using UnityEngine.UI;

public class FloorChanger : MonoBehaviour
{
    public static GameObject floor;
    private GameObject ActiveFloor;
    public GameObject[] Floors;

    /*
    private Dropdown _dropdown;
    public void Awake()
    {
        _dropdown = GetComponent<Dropdown>();
    }
    */

    void Start()
    {
        floor = Floors[0];
        ActiveFloor = floor;
    }

    public void FloorTo1()
    {
        DestroyAndCreate(0);
        floor = Floors[0];
    }

    public void FloorTo2()
    {
        DestroyAndCreate(1);
        floor = Floors[1];
    }

    public void FloorTo3()
    {
        DestroyAndCreate(2);
        floor = Floors[2];
    }

    public void FloorTo4()
    {
        DestroyAndCreate(3);
        floor = Floors[3];
    }

    public void FloorTo5()
    {
        DestroyAndCreate(4);
        floor = Floors[4];
    }

    // Destroy the pre-existing floor and instantiate a new one
    public void DestroyAndCreate(int index)
    {
        ActiveFloor = GameObject.FindWithTag("floor");
        if (ActiveFloor)
        {
            Instantiate(Floors[index]);
            Destroy(ActiveFloor);
        }
    }

}
