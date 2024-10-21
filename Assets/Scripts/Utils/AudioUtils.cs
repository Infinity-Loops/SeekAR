using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioUtils : MonoBehaviour
{
    public static void PlayClipAtPoint(AudioClip clip, Vector3 position, float volume = 1f, float spatialBlend = 1f, float pitch = 1f)
    {
        GameObject gameObject = new GameObject("One shot audio");
        gameObject.transform.position = position;
        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.clip = clip;
        audioSource.spatialBlend = spatialBlend;
        audioSource.pitch = pitch;
        audioSource.volume = volume;
        audioSource.Play();
        Object.Destroy(gameObject, clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }
}
