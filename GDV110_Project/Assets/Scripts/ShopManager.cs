using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Transform toppingCardTransform;
    public Transform waffleCardTransform;

    public int toppingCardsSpawned = 8;
    public int waffleCardsSpawned = 2;
    public int restockPrice;

    public GameObject cardPrefab;

    public AudioSource source;
    public Text restockButtonText;

    public ActivationIndicator activationIndicator;
    private void Start()
    {
        RestockShop(false);
    }


    public async void RestockShop(bool costs = false) //We Dont Charge The Player For Restocking The Shop Directly From The Method, Instead Do It On Button Call
    {
        if (RunManager.Instance.money < restockPrice)
        {
            Debug.Log("Not Enough Money");   
            return;
        }

        if (costs == true)
        {
            RunManager.Instance.money -= restockPrice;
        }

        //Destroy The Cards Already In The Shop To Roll Them

        for (int i = 0; i < toppingCardTransform.childCount; i++) 
        {
            Destroy(toppingCardTransform.GetChild(i).gameObject);
        }

        for (int i = 0; i < waffleCardTransform.childCount; i++) 
        {
            Destroy(waffleCardTransform.GetChild(i).gameObject);
        }

        //Now We Create New Cards
        //Topping Cards
        for(int i = 0; i < toppingCardsSpawned; i++)
        {
            GameObject cur = Instantiate(cardPrefab, toppingCardTransform);
            cur.GetComponent<ShopCard>().shop = this;
            cur.GetComponent<ShopCard>().data = RunManager.Instance.GetRandomCard();
            source.pitch = Random.Range(0.8f, 1.2f);
            source.Play();
            await Task.Delay(100);
        }

        //Now The Waffles
        for (int i = 0; i < waffleCardsSpawned; i++)
        {
            GameObject cur = Instantiate(cardPrefab, waffleCardTransform);
            cur.GetComponent<ShopCard>().shop = this;
            cur.GetComponent<ShopCard>().data = RunManager.Instance.GetRandomCard();
            source.pitch = Random.Range(0.8f, 1.2f);
            source.Play();
            await Task.Delay(100);
        }

        if (costs == true)
        {
            restockPrice++;
        }

        restockButtonText.text = $"Restock (${restockPrice})";
    }
}
