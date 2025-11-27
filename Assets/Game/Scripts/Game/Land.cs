using UnityEngine;

public class Land : MonoBehaviour
{
    public GameObject snow;
    public GameObject grass;

    public void Init()
    {
        var snowy = ((ManagerData.CURRENT_LEVEL_ID-1)/3) % 2 == 0;
        snow.SetActive(snowy);
        grass.SetActive(!snow.activeSelf);
    }
}
