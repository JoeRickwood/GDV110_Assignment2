using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(EntityClass))]
public class EntityUpgradeRenderer : MonoBehaviour
{
    public Transform upgradeGrid;
    public GameObject upgradeVisualPrefab;

    public void Start()
    {
        GetComponent<EntityClass>().onItemAdded += OnItemAdded;
    }

    public void OnItemAdded(Upgrade upgrade)
    {
        for (int i = 0; i < upgradeGrid.childCount; i++)
        {
            Destroy(upgradeGrid.GetChild(0).gameObject);
        }

        for (int i = 0; i < GetComponent<EntityClass>().entityUpgrades.Count; i++)
        {
            GameObject cur = Instantiate(upgradeVisualPrefab, upgradeGrid);
            cur.GetComponent<Image>().sprite = GameManager.Instance.GetStatIcon(GetComponent<EntityClass>().entityUpgrades[i].ID);
        }
    }
}
