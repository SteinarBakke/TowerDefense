using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float startSpeed = 10f;
    public float startHealth = 10;
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public float health;

    [Header("Enemy Type")]
    public bool flying = false;
    public bool ground = true;
    public bool fighter = false;
    public bool boss = false;

    [Header("Enemy Animation Settings")]
    public bool dead = false;
    public string deadEnemyTag = "DeadEnemy";
    public Animator anim;


    //This Semi Works.. But not really, since it's not linked to bullet Hit
    [Header("Enemy Debuffs")]
    public bool slowed = false;
    public bool weakened = false;
    public bool burning = false;

    [HideInInspector]
    public float slowedTimer =0;
    [HideInInspector]
    public float slowedAmount = 0;
    [HideInInspector]
    public float weakenedTimer = 0;
    [HideInInspector]
    public float weakenedAmount = 0;
    [HideInInspector]
    public float burningTimer = 0;
    [HideInInspector]
    public float burningAmount = 0;


    [Header("Particle Effect")]
    public GameObject dieEffect;
    public GameObject slowedEffect;
    public GameObject weakenedEffect;
    public GameObject burningEffect;

    private Transform target;

    void Start()
    {
        speed = startSpeed + Wavespawner.addedDifficulty;
        this.transform.localScale += new Vector3((Wavespawner.addedDifficulty / 10), (Wavespawner.addedDifficulty / 10), (Wavespawner.addedDifficulty / 10));
        health = startHealth * Mathf.Pow(2, (int)Wavespawner.addedDifficulty); //(Wavespawner.addedDifficulty + 1));//doubling every round

        //10...  10*2 = 20;. 10*2*2. 10*2*2*2            10. 20. 40. 80. 
        dead = false;
        if (boss) //increasing health by a ton, and reducing speed for boss lvl
        {
            //also doubling size
            this.transform.localScale += this.transform.localScale;
            health += health * 5;
            speed -= speed / 2;
        }
    }

    void Update()
    {
        if (weakened)
        {
            if (weakenedTimer > 0)
            {
                weakenedTimer -= Time.deltaTime;
                //this would be weakened in enemy.armor and enemy.weapon?
            }
            else
                weakened = false;
        }
        if (burning)
        {
            if (burningTimer > 0)
            {
                burningTimer -= Time.deltaTime;
                TakeDamage(burningAmount * Time.deltaTime);
            }
            else
                burning = false;
        }
        if (slowed)
        {
            if (slowedTimer > 0)
            {
                slowedTimer -= Time.deltaTime;
            }
            else
                slowed = false;
        }

    }


    public void TakeDamage(float damageTaken)
    {
        if (dead)
            return;
        health -= damageTaken;
        anim.Play("Take Damage");
        if (health <= 0)
            Die();
    }

    //index 0 = no debuffs
    //index 1 = slowed
    //index 2 = weakened
    //index 3 = burning
    //And ParticleEffect
    //  - assinging particleEffect as Child of Enemy for the duration (To travel with object)
    public void TakeDebuff(int index, float duration, float amount)
    {
        switch (index)
        {
            case 1:
                slowed = true;
                slowedTimer = duration;
                slowedAmount = amount;
                GameObject s_effect = (GameObject)Instantiate(slowedEffect, transform.position, Quaternion.identity);
                s_effect.transform.parent = transform;
                Destroy(s_effect, duration);
                break;
            case 2:
                weakened = true;
                weakenedTimer = duration;
                weakenedAmount = amount;
                GameObject w_effect = (GameObject)Instantiate(weakenedEffect, transform.position, Quaternion.identity);
                w_effect.transform.parent = transform;
                Destroy(w_effect, duration);
                break;
            case 3:
                burning = true;
                burningTimer = duration;
                burningAmount = amount;
                GameObject b_effect = (GameObject)Instantiate(burningEffect, transform.position, Quaternion.identity);
                b_effect.transform.parent = transform; 
                Destroy(b_effect, duration);
                break;
            default:
                break;
        }
    }
    

    void Die()
    {
        dead = true;
        GameObject[] Goldmine = GameObject.FindGameObjectsWithTag("GoldMine");
        foreach (GameObject mine in Goldmine)
            mine.GetComponent<PassiveTurret>().GiveMoney();

        GameObject effect = (GameObject)Instantiate(dieEffect, transform.position, Quaternion.identity);
        Destroy(effect, 3f);

        //changing tag to live out Die animation. (because of turret search).
        transform.gameObject.tag = deadEnemyTag;
        anim.Play("Die");
        //add die animation here
        Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length + 1.0f);
    }

}
