using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{

    public Color hoverColor;
    public Color occupiedColor;
    public Color selectedColor;
    private Color startColor;
    public Color noMoneyColor;
    public Vector3 positionOffset;

    [HideInInspector]
    public GameObject turret;
    [HideInInspector]
    public TurretBlueprint turretBlueprint;
    [HideInInspector]
    public int totalCost;

    private Renderer rend;

    Buildmanager buildManager;

    void Start()
    {
        totalCost = 0;
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;

        buildManager = Buildmanager.instance;
    }

    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }

    //Also include phone hover here
    void OnMouseEnter()
    {
        if (turret != null)
        {
            if (!buildManager.CanBuild)
                rend.material.color = occupiedColor;
            else if (buildManager.HasMoney)
                rend.material.color = noMoneyColor;
            return;
        }
        if (!buildManager.CanBuild || !buildManager.HasMoney)
            return;
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        Hover(true);
    }
    void OnMouseExit() { Hover(false); }

    void OnMouseDown()
    {
        if (turret != null)
        {
            buildManager.SelectNodeToBuild(this);
            return;
        }
        else if (turret == null && buildManager.CanBuild)
            BuildTurret(buildManager.turretToBuild);
    }

    void BuildTurret(TurretBlueprint blueprint)
    {
        if (PlayerStats.Currency < blueprint.cost)
        {
            Debug.Log("YOU BROKE BRUW");
            buildManager.turretToBuild = null;
            return;
        }

        PlayerStats.Currency -= blueprint.cost;
        totalCost += blueprint.cost;

        GameObject _turret = (GameObject)Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret;
        GameObject effect = (GameObject)Instantiate(buildManager.BuildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 3f);
        //resetting turret here
        buildManager.turretToBuild = null;
    }

    public void UpgradeTurret()
    {

    }

    public void UpgradeSlowEffect()
    {
        if (!turret.GetComponent<Turret>().IsUpgraded && (PlayerStats.Currency >= turretBlueprint.effectUpgradeCost))
        {
            PlayerStats.Currency -= turretBlueprint.effectUpgradeCost;
            totalCost += turretBlueprint.effectUpgradeCost;
            turret.GetComponent<Turret>().slowTower = true;
            turret.GetComponent<Turret>().IsUpgraded = true;
            turret.GetComponent<Turret>().upgradeTurretEffectColor();
        }
    }
    public void UpgradeWeakenEffect()
    {
        if (!turret.GetComponent<Turret>().IsUpgraded && (PlayerStats.Currency >= turretBlueprint.effectUpgradeCost))
        {
            PlayerStats.Currency -= turretBlueprint.effectUpgradeCost;
            totalCost += turretBlueprint.effectUpgradeCost;
            turret.GetComponent<Turret>().weakenedTower = true;
            turret.GetComponent<Turret>().IsUpgraded = true;
            turret.GetComponent<Turret>().upgradeTurretEffectColor();
        }
    }
    public void UpgradeBurningEffect()
    {
        if (!turret.GetComponent<Turret>().IsUpgraded && (PlayerStats.Currency >= turretBlueprint.effectUpgradeCost))
        {
            PlayerStats.Currency -= turretBlueprint.effectUpgradeCost;
            totalCost += turretBlueprint.effectUpgradeCost;
            turret.GetComponent<Turret>().burningTower = true;
            turret.GetComponent<Turret>().IsUpgraded = true;
            turret.GetComponent<Turret>().upgradeTurretEffectColor();
        }
    }
    public void UpgradeDamageEffect()
    {
        if (!turret.GetComponent<Turret>().IsUpgraded && (PlayerStats.Currency >= turretBlueprint.effectUpgradeCost))
        {
            PlayerStats.Currency -= turretBlueprint.effectUpgradeCost;
            totalCost += turretBlueprint.effectUpgradeCost;
            turret.GetComponent<Turret>().extraDamageTower = true;
            turret.GetComponent<Turret>().IsUpgraded = true;
            turret.GetComponent<Turret>().upgradeTurretEffectColor();
        }
    }



    public void SellTurret()
    {
        PlayerStats.Currency += totalCost / 2;
        totalCost = 0;
        Destroy(turret);
    }


    void Hover(bool hovered)
    {
        if (hovered)
        {
            if (buildManager.CanBuild)
            {
                if (buildManager.HasMoney)
                    rend.material.color = hoverColor;
                else
                    rend.material.color = noMoneyColor;
            }
            else
                rend.material.color = startColor;
        }
        else
            rend.material.color = startColor;
    }
}
