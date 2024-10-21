using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class MagnetPowerup : MonoBehaviour, IPointerDownHandler
{
    public GameObject onTap;
    public Animator anim;
    private bool alreadyTap;
    private RotatorHandler rotationHandler;
    private void Awake()
    {
        rotationHandler = GetComponent<RotatorHandler>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (PowerupData.magnetActive)
        {
            PowerupManager.PlayCantSelect();
            return;
        }

        float radius = 35f;

        if (PowerupData.showMeTheMoneyActive)
        {
            radius = 70;
        }
        if (MapUtils.isCloseTo(transform.position, radius) || PowerupData.whaleActive)
        {
            if (!alreadyTap)
            {
                alreadyTap = true;
                rotationHandler.enabled = false;

                anim.Play("Tap");

                var particle = Instantiate(onTap, transform.position, Quaternion.identity);
                particle.transform.localScale = Vector3.one * 80;
                particle.SetLayerRecursively(6); //Map Layer
                Vector3 targetRot = MapCamera.mapCamera.transform.position - transform.position;
                Vector3 rot = Quaternion.LookRotation(targetRot.normalized).eulerAngles;
                Vector3 targetLoc = MapCamera.mapCamera.transform.position + (MapCamera.mapCamera.transform.forward * 35);
                transform.DOMove(targetLoc, 0.25f).OnComplete(() =>
                {
                    transform.DORotate(rot, 0.25f).OnComplete(() =>
                    {
                        transform.DOScale(Vector3.zero, 0.25f).SetDelay(4.0f).OnComplete(() =>
                        {
                            PowerupData.magnetActive = true;
                            PowerupNotificationHandler.instance.HandleNotificationSequence("Coin Magnet!", "Temporarily attracts nearby crypto coins within a certain radius, pulling them toward the player automatically for 2 minutes.", 5f);
                            Destroy(gameObject);
                        });
                    });
                });


            }
        }
    }
}
