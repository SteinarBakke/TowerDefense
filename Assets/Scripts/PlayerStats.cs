using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    //Can add RPG elements to the player here!!
    public static int Currency;
    public int startCurrency = 15;
    public Text currency;

    public static int Health;
    public int startHealth = 20;
    public Text health;


    void Start()
    {
        Currency = startCurrency;
        Health = startHealth;
    }

    void Update()
    {
        health.text = "[" + Health.ToString() + "] HEALTH";
        currency.text = "[" + Currency.ToString() + "] GOLD";
    }

}
