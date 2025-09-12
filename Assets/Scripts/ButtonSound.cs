using UnityEngine;

public class ButtonSound : MonoBehaviour
{

    #region Fields
    //---------------------------------------

    public AudioSource audioSource;

    [Header("Button clips")]
    public AudioClip[] audioClips = new AudioClip[6];

    [Header("ID")]
    [Range(0, 6)]
    public int SoundID = 0;
    
    private int _currentValue = -1; // valeur interne

    public int CurrentValue
    {
        get => _currentValue;
        set
        {
            if (_currentValue != value) // seulement si ça change
            {
                _currentValue = value;
                PlaySound(_currentValue);
            }
        }
    }

    #endregion

    #region Unity Methods
    //---------------------------------------

    void Update()
    {
        SoundID = CurrentValue;
    }

    #endregion

    #region Custom Methods
    //---------------------------------------

    void PlaySound(int index)
    {
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource manquant !");
            return;
        }

        if (index >= 0 && index < audioClips.Length && audioClips[index] != null)
        {
            audioSource.clip = audioClips[index];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"Pas de son défini pour l'index {index}");
        }
    }

    #endregion

}
