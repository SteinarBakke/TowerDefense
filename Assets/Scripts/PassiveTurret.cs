using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveTurret : MonoBehaviour
{
    public GameObject moneyEffect;
    public GameObject healthEffect;
    public int moneyAmount = 1; //default
    public int healthAmount = 1; //default

    public void GiveMoney()
    {
        //run coin effect - on turret position + effect amount/count = amount
        GameObject effect = (GameObject)Instantiate(moneyEffect, this.transform.position, moneyEffect.transform.rotation);
        Destroy(effect, 3f);
        PlayerStats.Currency += moneyAmount;
    }

    public void GiveHealth()
    {
        //run heart effect - on turret position + effect amount/count = amount
        GameObject effect = (GameObject)Instantiate(healthEffect, this.transform.position, healthEffect.transform.rotation);
        Destroy(effect, 3f);
        PlayerStats.Health += healthAmount;
    }
}
