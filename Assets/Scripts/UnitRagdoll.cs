using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] private Transform ragdollRootBone;

    public void Setup(Transform originaRootBone)
    {
        MatchAllChildTransforms(originaRootBone, ragdollRootBone);
        ApplyExplosionForce(ragdollRootBone, 1000f, transform.position, 10f);
    }

    private void MatchAllChildTransforms(Transform root, Transform clone)
    {
        foreach(Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);
            if (cloneChild != null)
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;
                MatchAllChildTransforms(child, cloneChild);
            }
        }
    }

    private void ApplyExplosionForce(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        Rigidbody[] rigibodies = root.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigibody in rigibodies)
        {
            rigibody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
        }
    }
}
