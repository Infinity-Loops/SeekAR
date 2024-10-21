using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class HodlPowerup : MonoBehaviour , IPointerDownHandler
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
        if (PowerupData.hodlActive)
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
                        transform.DOScale(Vector3.zero, 0.25f).SetDelay(3.7f).OnComplete(() =>
                        {
                            PowerupData.hodlActive = true;
                            PowerupNotificationHandler.instance.HandleNotificationSequence("HODL", "You're a true HODLER! the value of your collected tokens are increased.", 5f);
                            Destroy(gameObject);
                        });
                    });
                });


            }
        }
    }
}
