using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class WhalePowerup : MonoBehaviour, IPointerDownHandler
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
        if (PowerupData.whaleActive)
        {
            PowerupManager.PlayCantSelect();
            return;
        }

        float radius = 35f;

        if (PowerupData.showMeTheMoneyActive)
        {
            radius = 70;
        }
        if (MapUtils.isCloseTo(transform.position, radius))
        {
            if (!alreadyTap)
            {
                alreadyTap = true;
                rotationHandler.enabled = false;
                
                anim.Play("Tap");

                var particle = Instantiate(onTap, transform.position, Quaternion.identity);
                particle.transform.localScale = Vector3.one * 40;
                particle.SetLayerRecursively(6); //Map Layer
                Vector3 targetRot = MapCamera.mapCamera.transform.position - transform.position;
                Vector3 rot = Quaternion.LookRotation(targetRot).eulerAngles;
                Vector3 targetLoc = MapCamera.mapCamera.transform.position + (MapCamera.mapCamera.transform.forward * 35);
                transform.DOMove(targetLoc, 0.25f).OnComplete(() =>
                {
                    transform.DORotate(rot, 0.25f).OnComplete(() =>
                    {
                        transform.DOScale(Vector3.zero, 0.25f).SetDelay(3.8f).OnComplete(() =>
                        {
                            PowerupData.whaleActive = true;
                            PowerupNotificationHandler.instance.HandleNotificationSequence("Whale", "You've become a whale and you're controlling the market! You're able to grab one item on the map outside your radius.", 5f);
                            Destroy(gameObject);
                        });
                    });
                });


            }
        }
    }
}
