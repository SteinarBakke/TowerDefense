using UnityEngine;

public class Shop : MonoBehaviour
{

    public TurretBlueprint standardTurret;
    public TurretBlueprint groundAOETurret;
    public TurretBlueprint AirTurret;
    public TurretBlueprint LaserTurret;
    public TurretBlueprint goldMine;

    Buildmanager buildManager;

    void Start()
    {
        buildManager = Buildmanager.instance;
    }

    public void SelectStandardTurret()
    {
        buildManager.SelectTurretToBuild(standardTurret);
    }
    public void SelectGroundTurret()
    {
        buildManager.SelectTurretToBuild(groundAOETurret);
    }
    public void SelectLaserTurret()
    {
        buildManager.SelectTurretToBuild(LaserTurret);
    }
    public void SelectMissileTurret()
    {
        buildManager.SelectTurretToBuild(AirTurret);
    }
    public void SelectGoldMine()
    {
        buildManager.SelectTurretToBuild(goldMine);
    }

}
