using DG.Tweening;
using GDTools.ObjectPooling;
using UnityEngine;

public class Ball : MonoBehaviour
{
    
    [SerializeField] MeshRenderer meshRenderer;
    Tube _owner;

    private int _colorID;

    public PoolObject poolObject;
    
    public int ColorID => _colorID;
    
    public void Initialize(int colorID)
    {
        _colorID = colorID;
        var colorRow = SOColorTable.GetRowById(_colorID);
        meshRenderer.material = colorRow.material;
    }
    
    public void SetOwner(Tube owner)
    {
        _owner = owner;
    }
    
    public void AutoDestroy()
    {
        transform.DOScaleX(0, .2f).SetEase(Ease.Linear);
        transform.DOScaleZ(0, .22f).SetEase(Ease.Linear).OnComplete(() =>
        {
            ManagerGame.Instance.PoolController.DeInstantiateBall(this);
            transform.localScale = Vector3.one;
        });
    }
}
