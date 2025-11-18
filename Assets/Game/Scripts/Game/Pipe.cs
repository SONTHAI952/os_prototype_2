using JetBrains.Annotations;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    [SerializeField]  private Transform body;
    [SerializeField]  private Transform head1;
    [SerializeField]  private Transform head2;
    private Cell _cell1;
    private Cell _cell2;

    public void Initialize(Cell cell1, Cell cell2)
    {
        _cell1  = cell1;
        _cell2  = cell2;
        
        Vector3 middle = (_cell1.transform.position + _cell2.transform.position) / 2f;
        transform.position = middle;
        transform.LookAt(_cell1.transform);
        var distance = Vector3.Distance(_cell1.transform.position, _cell2.transform.position);
        bool longPipe = distance > 2;
        var scale = body.localScale;
        scale.x = longPipe ? 9.5f : 2.5f;
        
        float gap = longPipe ? 1.25f : .35f;
        body.localScale = scale;
        head1.localPosition = new Vector3(-gap, 0, 0);
        head2.localPosition = new Vector3(gap, 0, 0);

    }


}
