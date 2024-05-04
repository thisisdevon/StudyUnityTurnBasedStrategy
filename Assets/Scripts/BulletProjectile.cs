using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Vector3 targetPosition;
    [SerializeField] private float moveSpeed = 200f;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform bulletHitVFXPrefab;
    bool isHit = false;

    public void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    void Update()
    {
        if (isHit) return;
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        Vector3 nextFramePosition = transform.position + moveDir * moveSpeed * Time.deltaTime;

        Vector3 nextFrameMoveDir = (targetPosition - nextFramePosition).normalized;

        if (Vector3.Dot(moveDir, nextFrameMoveDir) < 0)
        {
            transform.position = targetPosition;
            trailRenderer.transform.parent = null;
            Instantiate(bulletHitVFXPrefab, targetPosition, Quaternion.identity);
            isHit = true;
            Destroy(gameObject, 0.1f);
        }
        else
        {
            transform.position = nextFramePosition;
        }
    }
}
