using UnityEngine;

public static class MapUtils
{
    public static bool isCloseTo(Vector3 position, float closeDistance)
    {
        var avatar = MapAvatar.Instance.transform;

        return Vector3.Distance(avatar.position, position) < closeDistance;
    }
}
