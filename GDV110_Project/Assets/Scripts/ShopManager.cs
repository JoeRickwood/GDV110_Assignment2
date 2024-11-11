using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Transform toppingCardTransform;
    public Transform waffleCardTransform;

    public int toppingCardsSpawned = 8;
    public int waffleCardsSpawned = 2;
    public int restockPrice;

    bool canRestock = true;

    public GameObject cardPrefab;

    public AudioSource source;
    public Text restockButtonText;

    public ActivationIndicator activationIndicator;

    public ScreenTransition transition;

    bool roundStarting = false;

    private void Start()
    {
        StartCoroutine(RestockShop(false));
    }

    public void StartRestockShop(bool costs = false)
    {
        StartCoroutine(RestockShop(costs));
    }


    public IEnumerator RestockShop(bool costs = false) //We Dont Charge The Player For Restocking The Shop Directly From The Method, Instead Do It On Button Call
    {
        if ((RunManager.Instance.money < restockPrice || !canRestock) && costs == true)
        {
            yield break;
        }

        StopCoroutine(RestockShop());
        canRestock = false;

        if (costs == true)
        {
            RunManager.Instance.money -= restockPrice;
        }

        if (costs == true)
        {
            restockPrice++;
        }

        restockButtonText.text = $"Restock (${restockPrice})";

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
            if(UnityEngine.Random.Range(0, 1000) <= 1)
            {
                GameObject cur = Instantiate(cardPrefab, toppingCardTransform);
                cur.GetComponent<ShopCard>().shop = this;
                cur.GetComponent<ShopCard>().UpdateCardData(RunManager.Instance.GetRandomCard(CardTypeReturn.Developer));
            }
            else
            {
                GameObject cur = Instantiate(cardPrefab, toppingCardTransform);
                cur.GetComponent<ShopCard>().shop = this;
                cur.GetComponent<ShopCard>().UpdateCardData(RunManager.Instance.GetRandomCard(CardTypeReturn.Topping, CardTypeReturn.Cantrip));
            }

            source.pitch = Random.Range(0.8f, 1.2f);
            source.Play();
            yield return new WaitForSeconds(0.1f);
        }

        //Now The Waffles
        for (int i = 0; i < waffleCardsSpawned; i++)
        {
            GameObject cur = Instantiate(cardPrefab, waffleCardTransform);
            cur.GetComponent<ShopCard>().shop = this;
            cur.GetComponent<ShopCard>().UpdateCardData(RunManager.Instance.GetRandomCard(CardTypeReturn.Waffle));
            source.pitch = Random.Range(0.8f, 1.2f);
            source.Play();
            yield return new WaitForSeconds(0.1f);
        }
        canRestock = true;
    }

    public void StartNextRound()
    {
        if (roundStarting)
        {
            return;
        }

        StartCoroutine(StartNextRoundCoroutine());
    }

    public IEnumerator StartNextRoundCoroutine()
    {
        roundStarting = true;
        float t = 1f / transition.speed;
        StartCoroutine(transition.StartScreenTransition(false));

        while (t > 0) 
        {
            t -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }


        AsyncOperation operation = SceneManager.LoadSceneAsync(3);
        while (!operation.isDone) 
        { 
           yield return null;
        }
    }
}
