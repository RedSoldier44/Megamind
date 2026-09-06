using UnityEngine;

public class ButtonSound : MonoBehaviour
{

    #region Fields
    //---------------------------------------

    public AudioSource audioSource;

    [Header("Button clips")]
    public AudioClip[] audioClips = new AudioClip[6];
    public AudioClip[] shortClips = new AudioClip[6];

    [Header("ID")]
    [Range(0, 5)]
    public int SoundID = 0;
    [SerializeField, Range(0, 4)]
    private int buttonId = 0;

    [HideInInspector]
    public bool isReady = false;

    #endregion

    #region Custom Methods
    //---------------------------------------

    public void SetButtonValue(int btnId, int newValue)
    {
        if (btnId != buttonId) return;
        SetSoundID(newValue - 1);
    }

    public void SetSoundID(int newID)
    {
        if (newID >= 0 && newID < audioClips.Length)
        {
            SoundID = newID;
            PlayCurrentSound();
        }
        else
        {
            //Debug.LogWarning("SoundID invalide pour " + gameObject.name);
        }
    }

    public void PlayIndex(int index)
    {
        if (!isReady) return;
        if (audioSource != null) audioSource.Stop();
        if (index >= 0 && index < audioClips.Length && audioSource != null && audioClips[index] != null)
        {
            audioSource.clip = audioClips[index];
            audioSource.Play();
        }
        else
        {
            //Debug.LogWarning("Index invalide ou son manquant pour " + gameObject.name);
        }
    }

    void PlayCurrentSound()
    {
        if (!isReady) return;
        if (audioSource != null) audioSource.Stop();
        if (audioSource != null && shortClips[SoundID] != null)
        {
            audioSource.clip = shortClips[SoundID];
            audioSource.Play();
        }
    }

    #endregion

}
