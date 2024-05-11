using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{
    [SerializeField] private UnitRagdoll ragdollPrefab;
    [SerializeField] private Transform ragdollRootBone;

    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();

        healthSystem.OnDeath += HealthSystem_OnDeath;
    }

    private void HealthSystem_OnDeath(object sender, EventArgs e)
    {
        UnitRagdoll ragdoll = Instantiate(ragdollPrefab, transform.position, transform.rotation) as UnitRagdoll;
        ragdoll.Setup(ragdollRootBone);
    }
}
