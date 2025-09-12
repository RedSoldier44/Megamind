using UnityEngine;

public class ButtonSound : MonoBehaviour
{

    #region Fields
    //---------------------------------------

    public AudioSource audioSource;

    [Header("Button clips")]
    public AudioClip[] audioClips = new AudioClip[6];

    [Header("ID")]
    [Range(0, 5)]
    public int SoundID = 0;

    #endregion

    #region Unity Methods
    //---------------------------------------

    void Update()
    {

    }

    #endregion

    #region Custom Methods
    //---------------------------------------

    public void SetSoundID(int newID)
    {
        if (newID >= 0 && newID < audioClips.Length)
        {
            SoundID = newID;
            PlayCurrentSound();
        }
        else
        {
            Debug.LogWarning("SoundID invalide pour " + gameObject.name);
        }
    }

    public void PlayIndex(int index)
    {
        if (index >= 0 && index < audioClips.Length && audioSource != null && audioClips[index] != null)
        {
            audioSource.clip = audioClips[index];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Index invalide ou son manquant pour " + gameObject.name);
        }
    }

    void PlayCurrentSound()
    {
        if (audioSource != null && audioClips[SoundID] != null)
        {
            audioSource.clip = audioClips[SoundID];
            audioSource.Play();
        }
    }

    #endregion

}
