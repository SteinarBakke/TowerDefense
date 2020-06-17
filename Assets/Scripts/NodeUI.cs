using UnityEngine;

public class NodeUI : MonoBehaviour
{

    public GameObject ui;
    public GameObject upgUI;
    private Node target;


    public void SetTarget(Node selectedNode)
    {
        target = selectedNode;
        transform.position = target.GetBuildPosition();

        upgUI.SetActive(false);
        ui.SetActive(true);
    }


    public void Hide()
    {
        target = null;
        ui.SetActive(false);
    }

    public void SellMe()
    {
        target.SellTurret();
        Hide();
    }

    public void UpgradeButton()
    {
        upgUI.SetActive(true);
    }

    public void UpgradeSlowEffect()
    {
        target.UpgradeSlowEffect();
        Hide();
    }
    public void UpgradeBurnEffect()
    {
        target.UpgradeBurningEffect();
        Hide();
    }
    public void UpgradeWeakEffect()
    {
        target.UpgradeWeakenEffect();
        Hide();
    }
    public void UpgradeDmgEffect()
    {
        target.UpgradeDamageEffect();
        Hide();
    }

}
