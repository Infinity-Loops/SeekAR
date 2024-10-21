using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapInstantiateEvent : MonoBehaviour, IPointerDownHandler
{
    private bool click;
    public GameObject eventToSpawn;

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

                if (PowerupData.whaleActive)
                {
                    PowerupData.whaleActive = false;
                }

                transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
                {
                    HandleSpawnEvent();
                });

            }
        }
    }

    void HandleSpawnEvent()
    {
        var location = ARCamera.arCamera.transform.position + Vector3.Scale(ARCamera.arCamera.transform.forward, new Vector3(1,0,1)) * 3f;
        var rotation = Quaternion.Euler(0, ARCamera.arCamera.transform.eulerAngles.y - 180, 0);
        Instantiate(eventToSpawn, location, rotation);
        MapSystem.instance.EnableOrDisableMap();
        Destroy(gameObject);
    }
}
