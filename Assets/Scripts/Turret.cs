using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;
    private Enemy targetEnemy;

    [Header("General Turret Settings")]
    public float range = 15f;
    public Transform firePoint;

    [Header("Use Bullets (default)")]
    public float fireRate = 1f;
    public float fireCountdown = 0f;
    public GameObject bulletPrefab;

    [Header("Special Bullets")]
    public bool useLaser = false;
    public bool usePiercing = false;
    public LineRenderer lineRenderer;
    public float LaserDmg = 20f;
    //public ParticleSystem laserImpactEffect;

    
    [Header("Special Effects")]
    public bool slowTower;
    public float slowAmount = 1f; // 1 = no slow (speed * slowAmount)
    public float slowDuration = 0;
    public bool burningTower;
    public float burningDuration = 0;
    public bool weakenedTower;
    public float weakenedAmount = 0; // Weakened not implemented yet
    public float weakenedDuration = 0;
    public bool extraDamageTower; // double dmg THIS MIGHT BE OP COMBINED WITH BURNING FOR ONLY 400? Make it a Expensive Upgrade?
    public float extraDamageAmount = 0;
    public bool IsUpgraded;
    private bool tempUpgraded = false;
    public GameObject turretReference;



    [Header("Turret Type (ground default)")]
    public bool AirTurret = false;
    public bool GroundTurret = true;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";

    [Header("Rotation Settings")]
    public Transform partToRotate;
    public float rotationSpeed = 10f;


    void Start()
    {
        slowTower = false;
        burningTower = false;
        weakenedTower = false;
        extraDamageTower = false;
        IsUpgraded = false;
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }
    void UpdateTarget()
    {
        if (GameManager.gameEnded)
            return;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        //checking all gameobjects with tag Enemy
        //checking if unit ground/air match turret
        //checking if target is alive (For death animation)
        foreach (GameObject enemy in enemies)
        {
            if ((enemy.GetComponent<Enemy>().ground == true && GroundTurret == true) || (enemy.GetComponent<Enemy>().flying == true && AirTurret == true))
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
                else
                    target = null;
            }
        }
        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<Enemy>();
        }
        else
        {
            target = null;
            targetEnemy = null;
        }
    }


    void Update()
    {
        if (GameManager.gameEnded) //don't play when game is over
            return;


        if (target == null)
        {
            if (useLaser)
            {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                    //laserImpactEffect.Stop();
                }
            }
            return;
        }

        LockOnTarget();

        if (useLaser)
        {
            //Debuffs
            if (slowTower)
                targetEnemy.TakeDebuff(1, slowDuration, slowAmount);
            if (weakenedTower)
                targetEnemy.TakeDebuff(2, weakenedDuration, weakenedAmount);
            if (burningTower)
                targetEnemy.TakeDebuff(3, burningDuration, LaserDmg);
            if (extraDamageTower)
                LaserDmg = LaserDmg * extraDamageAmount;

            LaserShoot();
        }
        else if (usePiercing)
        {
            PierceShoot(); //not implemented
        }
        else //Bullets
        {
            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }
            fireCountdown -= Time.deltaTime;
        }

    }

    void Shoot()
    {
        GameObject bulletGameObject = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGameObject.GetComponent<Bullet>();

        if (bullet != null)
        {
            //Debuffs
            if (slowTower)
                bullet.ApplyDebuff(1, slowDuration, slowAmount);
            if (weakenedTower)
                bullet.ApplyDebuff(2, weakenedDuration, weakenedAmount);
            if (burningTower)
                bullet.ApplyDebuff(3, burningDuration, bullet.damage);
            if (extraDamageTower)
                bullet.damage = bullet.damage * (int)extraDamageAmount;

            bullet.Seek(target);
        }
    }

    public void upgradeTurretEffectColor()
    {
        if (slowTower)
        {
            turretReference.GetComponent<Renderer>().materials[1].color = Color.cyan;
            if (useLaser)
                lineRenderer.SetColors(Color.cyan, Color.white);
        }
        if (burningTower)
        {
            turretReference.GetComponent<Renderer>().materials[1].color = Color.red;
            if (useLaser)
                lineRenderer.SetColors(Color.red, Color.white);
        }
        if (weakenedTower)
        {
            turretReference.GetComponent<Renderer>().materials[1].color = Color.yellow;
            if (useLaser)
                lineRenderer.SetColors(Color.yellow, Color.white);
        }
        if (extraDamageTower)
        {
            turretReference.GetComponent<Renderer>().materials[1].color = Color.black;
            if (useLaser)
                lineRenderer.SetColors(Color.black, Color.white);
        }

    }

    void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void LaserShoot()
    {
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            //laserImpactEffect.Play();
        }

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        targetEnemy.TakeDamage(LaserDmg * Time.deltaTime);
    }

    void PierceShoot()
    {
        Debug.Log("Not Implemented Yet");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,range);
    }
}
