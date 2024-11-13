using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{

    [TextAreaAttribute]
    public string[] panelTitles;

    [TextAreaAttribute]
    public string[] panelDescriptions;

    public int currentPanel;

    public Text titleText;
    public Text descriptionText;

    public BattleManager battleManager;

    private void Update()
    {
        if(currentPanel >= panelDescriptions.Length)
        {
            Destroy(gameObject);
            return;
        }

        titleText.text = panelTitles[currentPanel];
        descriptionText.text = panelDescriptions[currentPanel];
    }


    public void Next()
    {
        currentPanel++;
    }
}
