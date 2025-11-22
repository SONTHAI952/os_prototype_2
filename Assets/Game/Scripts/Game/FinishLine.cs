using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private MeshRenderer renderer;
    [SerializeField] private Material materialWhite;
    [SerializeField] private Material materialBlack;

    public void Init(int x, int y)
    {
        bool isWhite = (x + y) % 2 == 0;
        SetupVisual(isWhite);
    }
    public void SetupVisual(bool white)
    {
        renderer.material = white ? materialWhite : materialBlack;
    }
}
