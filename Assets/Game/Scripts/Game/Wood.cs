using System;
using System.Collections;
using CS;
using UnityEngine;

public class Wood : MonoBehaviour
{
    [SerializeField] private Rigidbody body;

    private void Awake()
    {
        body.isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(Timeline());

        IEnumerator Timeline()
        {
            yield return Yielder.Wait(.3f);
            Drop();
        }
    }

    void Drop()
    {
        body.isKinematic = false;
         
        Ray ray = new Ray(transform.position, Vector3.up);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            if (hit.collider != null)
            {
                var player = hit.collider.GetComponent<Player>();
                if (player != null)
                {
                    player.Fall();
                }
            }
        }
    }
}
