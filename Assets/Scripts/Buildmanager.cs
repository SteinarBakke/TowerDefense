using UnityEngine;

public class Buildmanager : MonoBehaviour
{

    public static Buildmanager instance; //buildmanager inside the buildmanage
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple BuildManagers");
            return;
        }
        instance = this;//Setting buildmanager to this instance
    }

    public TurretBlueprint turretToBuild;
    private Node selectedNode;

    public NodeUI nodeUI;

    public GameObject BuildEffect;


    public bool CanBuild { get { return turretToBuild != null; } }
    public bool HasMoney { get { return PlayerStats.Currency >= turretToBuild.cost; } }


    //simple setter
    public void SelectTurretToBuild (TurretBlueprint turret)
    {
        turretToBuild = turret;
        DeselectNode();
    }

    //simple setter
    public void SelectNodeToBuild(Node node)
    {
        if (selectedNode == node)
        {
            DeselectNode();
            return;
        }
        selectedNode = node;
        turretToBuild = null;

        nodeUI.SetTarget(node);
    }

    public void DeselectNode()
    {
        selectedNode = null;
        nodeUI.Hide();
    }
}
