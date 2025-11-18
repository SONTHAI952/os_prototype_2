using UnityEngine;
using CS;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] private HideType hideType;
    [SerializeField] private float timeLife;
    [SerializeField] private bool activeOnEnable;

    private Countdowner countdowner;

    private void OnEnable()
    {
        if (activeOnEnable)
        {
            countdowner.StartCd(timeLife);
        }
    }

    private void OnDisable()
    {
        countdowner.StartCd(timeLife);
    }

    public void StartAutoDestroy(float timeLife, HideType hideType)
    {
        this.hideType = hideType;
        this.timeLife = timeLife;
        countdowner.StartCd(timeLife);
    }

    private void Update()
    {
        if (countdowner.IsCd())
        {
            countdowner.Cd();
            if (countdowner.IsOut())
            {
                Destroy();
            }
        }
    }

    private void Destroy()
    {
        if (hideType == HideType.Destroy)
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying)
                DestroyImmediate(gameObject);

#else
            if (Application.isPlaying)
            Destroy(gameObject);
#endif
        }
        else if (hideType == HideType.Disable)
        {
            gameObject.SetActive(false);
        }
        else if (hideType == HideType.Pool)
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying)
                gameObject.Recycle();

#else
            if (Application.isPlaying)
                gameObject.Recycle();
#endif
        }
    }

    public enum HideType
    {
        Destroy, Disable, Pool
    }
}
