using ReadyPlayerMe.Core;
using ReadyPlayerMe.WebView;
using UnityEngine;

public class ReadyPlayerMeHandler : MonoBehaviour
{
    public WebViewPanel readyPlayerMePanel;
    public RuntimeAnimatorController animatorInstance;
    public MapAvatar mapAvatarInstance;
    private GameObject avatarInstance;

    private void Awake()
    {
        HandleEvents();
        GetAvatar();
    }

    void HandleEvents()
    {
        readyPlayerMePanel.OnAvatarCreated.AddListener(HandleOnAvatarCreated);
        readyPlayerMePanel.OnUserAuthorized.AddListener(HandleOnUserAuthTokenUpdated);
        readyPlayerMePanel.OnUserUpdate.AddListener(HandleOnUserAuthTokenUpdated);
        readyPlayerMePanel.OnUserSet.AddListener(HandleOnUserAuthTokenUpdated);
        readyPlayerMePanel.OnUserLogout.AddListener(() => HandleOnUserAuthTokenUpdated(""));
    }

    void HandleOnUserAuthTokenUpdated(string token)
    {
        DataSystem.gameData.playerData.readyPlayerMeAuthToken = token;
        DataSystem.SaveData();
    }

    void HandleOnAvatarCreated(string url)
    {
        DataSystem.gameData.playerData.avatarUrl = url;
        DataSystem.SaveData();
        GetAvatar();
    }

    void HandleOnAvatarInstanceLoaded()
    {
        avatarInstance.SetLayerRecursively(6); //Map Layer;
        avatarInstance.transform.parent = mapAvatarInstance.avatarRoot;
        avatarInstance.transform.localPosition = Vector3.zero;
        avatarInstance.transform.localRotation = Quaternion.identity;
        avatarInstance.transform.localScale = Vector3.one;
        var animator = avatarInstance.GetComponent<Animator>();
        animator.runtimeAnimatorController = animatorInstance;
        mapAvatarInstance.avatarAnimator = animator;
    }

    void GetAvatar()
    {
        string url = "";


        if (!string.IsNullOrEmpty(DataSystem.gameData.playerData.avatarUrl))
        {
            url = DataSystem.gameData.playerData.avatarUrl;
        }
        else
        {
            url = "https://models.readyplayer.me/638df693d72bffc6fa17943c.glb";
        }

        var avatarLoader = new AvatarObjectLoader();
        avatarLoader.OnCompleted += (_, args) =>
        {
            if (avatarInstance != null)
            {
                Destroy(avatarInstance);
            }

            avatarInstance = args.Avatar;

            HandleOnAvatarInstanceLoaded();
        };
        avatarLoader.LoadAvatar(url);

    }
}
