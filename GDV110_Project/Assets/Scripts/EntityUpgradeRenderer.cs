using System.Collections.Generic;
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
            Destroy(upgradeGrid.GetChild(i).gameObject);
        }

        List<Sprite> sprites = new List<Sprite>();
        List<int> counts = new List<int>();

        for (int i = 0; i < GetComponent<EntityClass>().entityUpgrades.Count; i++)
        {
            bool ret = true;
            for(int j = 0; j < sprites.Count; j++)
            {
                if (GetComponent<EntityClass>().entityUpgrades[i].entityApplySprite == sprites[j])
                {
                    counts[j]++;
                    ret = false;
                    break;
                }
            }

            if(ret == true)
            {
                sprites.Add(GetComponent<EntityClass>().entityUpgrades[i].entityApplySprite);
                counts.Add(1);
            }
        }

        for (int i = 0; i < sprites.Count; i++)
        {
            GameObject cur = Instantiate(upgradeVisualPrefab, upgradeGrid);
            cur.GetComponent<Image>().sprite = sprites[i];
            cur.transform.GetChild(0).GetComponent<Text>().text = $"X{counts[i]}";
        }
    }
}
