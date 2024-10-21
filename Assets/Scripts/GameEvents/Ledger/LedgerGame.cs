using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class LedgerGame : MonoBehaviour, IPointerDownHandler
{
    public Transform meshRoot;
    public Transform interactionCanvas;
    public CoinDB gameCoins;
    public GameObject coinCollectable;
    public AudioClip collectAudio;
    public ParticleSystem collectParticle;
    private bool started;
    private bool isCloseEnough;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!started)
        {
            started = true;
            transform.DOPunchScale(Vector3.one * 1.1f, 0.25f, 2, 1);
            HandleStartLedger();
        }
    }

    void HandleStartLedger()
    {
        interactionCanvas.gameObject.SetActive(true);
        interactionCanvas.localScale = Vector3.zero;
        interactionCanvas.DOScale(Vector3.one * 0.003f, 0.25f);
    }

    public void HandleCollect()
    {
        transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
        {
            float[] positions = new float[] { -1f, 0f, 1f };
            for (int i = 0; i < 3; i++)
            {
                var coin = gameCoins.coins[Random.Range(0, gameCoins.coins.Count)];
                Vector3 location = transform.position + new Vector3(positions[i], 0, 0);
                var coinPrefab = Instantiate(coinCollectable, location, Quaternion.identity);
                var selector = coinPrefab.GetComponent<CollectableCoinSelector>();
                selector.dontChooseCoin = true;
                selector.coinToChoose = coin.name;
            }
            AudioUtils.PlayClipAtPoint(collectAudio, transform.position, 1, 0, 1);
            Instantiate(collectParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        });
    }

    void HandleRotation()
    {
        float distance = Vector3.Distance(ARCamera.arCamera.transform.position, transform.position);
        if (distance <= 5)
        {
            if (!isCloseEnough)
            {
                isCloseEnough = true;
                transform.DORotate(new Vector3(0, ARCamera.arCamera.transform.eulerAngles.y, 0), 0.25f);
            }
        }
        else
        {
            isCloseEnough = false;
        }

        if (!started && !isCloseEnough)
        {
            meshRoot.Rotate(Vector3.up * 100 * Time.deltaTime);
        }
        else
        {
            meshRoot.localRotation = Quaternion.Slerp(meshRoot.localRotation, Quaternion.identity, 5 * Time.deltaTime);
        }

    }

    void Update()
    {
        HandleRotation();
    }
}
