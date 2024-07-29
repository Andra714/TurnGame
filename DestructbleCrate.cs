using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructbleCrate : MonoBehaviour
{
    public static event EventHandler OnAnyDestoryed;

    [SerializeField] private Transform crateDestroyedPrefab;
    private GridPosition gridPosition;

    private void Start() 
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);    
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
    public void Damage()
    {
        Transform crateDestroyedTransform = Instantiate(crateDestroyedPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
        ApplyExplosionToChildren(crateDestroyedTransform, 150f, transform.position, 10f);

        OnAnyDestoryed?.Invoke(this, EventArgs.Empty);
    }

     private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach(Transform child in root)
        {
            if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }
            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
