using System.Collections;
using UnityEngine;
using static HP;

public class GeneARecBehaviors : MonoBehaviour, IAttackBehavior
{
    public float AttackRange => fireBallRange - 5f;

    [Header("DO NOT modify!!!")]
    public GameObject fireBallPrefabForEnemies; // Declare a public GameObject for the fire prefab
    public GameObject fireBallPrefabForDefenders; // Declare a public GameObject for the fire prefab

    [Header("Damage Settings")]
    public float instantDamage = 30f;    // Instant damage applied upon touch.
    public float dotDamage = 5f;         // Damage over time applied while burning.
    public float burnDuration = 3f;      // Duration of the burn effect.
    public float burnTickInterval = 1f;  // Time interval between damage ticks while burning.

    [Header("Attack Settings")]
    public float fireBallInterval = 1f;
    public float fireBallRange = 20f;
    public float explosionRange = 5f;
    private GameObject firePoint;
    private GameObject fireBallPrefab;
    
    private float nextFireTime = 1.0f;

    private HP selfHP;
    [HideInInspector]public GameObject target;
    private EnemyController enemyController;
    private DefenderController defenderController;
    private bool isAttacking;

    private void Awake()
    {
        selfHP = GetComponent<HP>();

        if (selfHP.objectType == ObjectType.Enemy)
        {
            enemyController = GetComponent<EnemyController>();
        }
        else if (selfHP.objectType == ObjectType.Defender)
        {
            defenderController = GetComponent<DefenderController>();
        }
        firePoint = transform.GetChild(0).gameObject;

        //get stats
        /*instantDamage = geneTypeAInfoSO.recStats.instantDamage;
        dotDamage = geneTypeAInfoSO.recStats.dotDamage;
        burnDuration = geneTypeAInfoSO.recStats.burnDuration;
        burnTickInterval = geneTypeAInfoSO.recStats.burnTickInterval;
        fireBallInterval = geneTypeAInfoSO.recStats.fireBallInterval;
        fireBallRange = geneTypeAInfoSO.recStats.fireBallRange;
        explosionRange = geneTypeAInfoSO.recStats.explosionRange;*/
    }

    private void Update()
    {
        if (selfHP.objectType == ObjectType.Enemy)
        {
            isAttacking = enemyController.isAttacking;
        }
        else if (selfHP.objectType == ObjectType.Defender)
        {
            isAttacking = defenderController.isAttacking;
        }
        if (Time.time > nextFireTime && isAttacking)
        {
            StartCoroutine(LaunchFireBall());
            nextFireTime = Time.time + fireBallInterval;
        }
    }

    IEnumerator LaunchFireBall()
    {
        
        if (selfHP.objectType == ObjectType.Enemy)
        {
            //FindClosestDefender();
            target = enemyController.targetDefender;
        }
        else if (selfHP.objectType == ObjectType.Defender)
        {
            //FindClosestEnemy();
            target = defenderController.targetEnemy;
        }
        if(target != null) 
        {
            if (Vector3.Distance(transform.position, target.transform.position) < fireBallRange)
            {
                //Vector3 spawnPosition = transform.position + transform.forward; // Adjust the offset if necessary
                if (selfHP.objectType == ObjectType.Enemy)
                {
                    fireBallPrefab = fireBallPrefabForEnemies;
                }
                else if (selfHP.objectType == ObjectType.Defender)
                {
                    fireBallPrefab = fireBallPrefabForDefenders;
                }
                GameObject fireInstance = Instantiate(fireBallPrefab, firePoint.transform.position, transform.rotation);
                FireBall fireBall = fireInstance.AddComponent<FireBall>();
                fireBall.objectType = selfHP.objectType;
                fireBall.fireBallTarget = target;
                fireBall.burnDuration = burnDuration;
                fireBall.burnTickInterval = burnTickInterval;
                fireBall.instantDamage = instantDamage;
                fireBall.dotDamage = dotDamage;
                fireBall.explosionRange = explosionRange;
            }
        }
        yield return null;
    }

    private void FindClosestDefender()
    {
        float closestDistance = fireBallRange;
        GameObject closestDefender = null;
        foreach (GameObject defender in DefenderManager.Instance.defenders)
        {
            float currentDistance = Vector3.Distance(transform.position, defender.transform.position);
            if (currentDistance <= closestDistance)
            {
                closestDistance = currentDistance;
                closestDefender = defender;
            }
        }
        target = closestDefender;
    }

    private void FindClosestEnemy()
    {
        float closestDistance = fireBallRange;
        GameObject closestEnemy = null;
        foreach (GameObject enemy in EnemyManager.Instance.Enemies)
        {
            float currentDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if (currentDistance <= closestDistance)
            {
                closestDistance = currentDistance;
                closestEnemy = enemy;
            }
        }
        target = closestEnemy;
    }
}
