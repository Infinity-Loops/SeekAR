using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CoinMapInstantiateEvent : MonoBehaviour, IPointerDownHandler
{
    private bool click;
    public GameObject eventToSpawn;
    public CoinDB coins;
    private GameCoin currentCoin;
    private Vector3 magnetRadius;
    private void Awake()
    {
        var rnd = Random.insideUnitCircle;
        magnetRadius = new Vector3(rnd.x, 0, rnd.y);
        HandleVisuals();
    }

    void HandleVisuals()
    {
        var coinList = coins.coins.FindAll(x => x.notSpawn == false);
        currentCoin = coinList[Random.Range(0, coinList.Count)];
        var coinModel = Instantiate(currentCoin.model, transform);
        coinModel.transform.localScale = Vector3.one * 2f;
        coinModel.SetLayerRecursively(6);
    }

    private void Update()
    {
        if (PowerupData.magnetActive && Vector3.Distance(transform.position, MapAvatar.Instance.transform.position) <= 75)
        {
            Vector3 targetLocation = MapAvatar.Instance.transform.position + (Vector3.up * 10) + magnetRadius * 30;

            if(Vector3.Distance(transform.position, targetLocation) >= 10)
            {
                Vector3 moveVector = (targetLocation - transform.position).normalized;
                transform.position += moveVector * Time.deltaTime * 50;
            }

        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        float radius = 35f;

        if (PowerupData.showMeTheMoneyActive)
        {
            radius = 70;
        }

        if (MapUtils.isCloseTo(transform.position, radius) || PowerupData.whaleActive)
        {
            if (!click)
            {
                click = true;
                transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
                {
                    HandleSpawnEvent();
                });

            }
        }
    }

    void HandleSpawnEvent()
    {
        var location = ARCamera.arCamera.transform.position + Vector3.Scale(ARCamera.arCamera.transform.forward, new Vector3(1, 0, 1)) * 3f;
        var rotation = Quaternion.Euler(0, ARCamera.arCamera.transform.eulerAngles.y - 180, 0);

        var coin = Instantiate(eventToSpawn, location, rotation);
        var coinSelector = coin.GetComponent<CollectableCoinSelector>();
        coinSelector.dontChooseCoin = true;
        coinSelector.coinToChoose = currentCoin.name;
        MapSystem.instance.EnableOrDisableMap();
        Destroy(gameObject);
    }
}

