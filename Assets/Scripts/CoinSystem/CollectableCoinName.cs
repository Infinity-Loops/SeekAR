using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CollectableCoinName : MonoBehaviour, IPointerDownHandler
{
    public GameObject infoPanel;
    private Transform camera;
    private TMP_Text text;
    private bool shownInformationalPanel;
    private GameCoin currentCoin;
    private BoxCollider collider;

    public void OnPointerDown(PointerEventData eventData)
    {
        ShowInformationalPanel();
    }

    void ShowInformationalPanel()
    {
        if (!shownInformationalPanel && currentCoin != null)
        {
            shownInformationalPanel = true;
            Vector3 location = transform.parent.position + transform.parent.right * 1.25f;
            Quaternion rotation = Quaternion.Euler(0, ARCamera.arCamera.transform.eulerAngles.y, 0);
            var panel = Instantiate(infoPanel, location, rotation);
            panel.transform.localScale = Vector3.one * 0.003f;
            var coinInfoPanel = panel.GetComponent<CoinInfoPanel>();
            coinInfoPanel.SetCaller(transform);
            coinInfoPanel.SetDetails(currentCoin);
        }
    }

    public void ResetInformationalPanelState()
    {
        shownInformationalPanel = false;
    }

    public void OnSelectedCoin(GameCoin coin)
    {
        currentCoin = coin;

        if(text == null)
        {
            text = GetComponent<TMP_Text>();
        }

        if (collider == null)
        {
            collider = gameObject.AddComponent<BoxCollider>();
        }
        text.text = currentCoin.name;
        text.ForceMeshUpdate();
        var sharedMesh = text.mesh;

        collider.size = sharedMesh.bounds.size;
    }

    private void Awake()
    {
        camera = ARCamera.arCamera.transform;
    }

    void HandleMovementUpdate()
    {
        transform.forward = camera.forward;
    }

    private void LateUpdate()
    {
        HandleMovementUpdate();
    }
}
