using UnityEngine;

public class GameSoundManager : MonoBehaviour
{

    #region Fields
    //---------------------------------------

    [Header("Button A")]
    public AudioClip[] locationSounds = new AudioClip[5];

    [Header("ID")]
    [Range(0, 6)]
    public int locationSoundID = 0;

    public int locationCount;

    public AudioSource audioSourceA;

    //---------------------------------------
    [Header("Button B")]
    public AudioClip[] animalSounds = new AudioClip[5];
    [Range(0, 6)]
    public int animalSoundID = 0;

    public int animalCount;

    public AudioSource audioSourceB;

    //---------------------------------------
    [Header("Button C")]
    public AudioClip[] voiceSounds = new AudioClip[5];
    [Range(0, 6)]
    public int voiceSoundID = 0;

    public int voiceCount;

    public AudioSource audioSourceC;

    //---------------------------------------
    [Header("Button D")]
    public AudioClip[] movementSounds = new AudioClip[5];
    [Range(0, 6)]
    public int movementSoundID = 0;

    public int movementCount;

    public AudioSource audioSourceD;

    //---------------------------------------
    [Header("Button E")]
    public AudioClip[] birdSounds = new AudioClip[5];
    [Range(0, 6)]
    public int birdSoundID = 0;

    public int birdCount;

    public AudioSource audioSourceE;

    [Header("Each buttons")]
    public AudioClip[][] soundList;
    

    #endregion

    #region Unity Methods
    //---------------------------------------

    void Awake()
    {
        soundList = new AudioClip[][]
        {
        locationSounds,
        animalSounds,
        voiceSounds,
        movementSounds,
        birdSounds
        };
    }

    void Start()
    {
        PlayRandomLocationSound();
        PlayRandomAnimalSound();
        PlayRandomVoiceSound();
        PlayRandomMovementSound();
        PlayRandomBirdSound();
    }

    #endregion

    #region Custom Methods
    //---------------------------------------

    #region Button A Methods
    //---------------------------------------

    public void PlayRandomLocationSound()
    {
        if (locationSounds.Length == 0 || audioSourceA == null) return;

        int index;

        if (locationSoundID >= 1 && locationSoundID <= 6)
        {
            index = locationSoundID - 1;
        }
        else
        {
            index = Random.Range(0, locationSounds.Length);
            locationCount = index;
        }

        audioSourceA.clip = locationSounds[index];
        audioSourceA.Play();
    }

    #endregion

    #region Button B Methods
    //---------------------------------------

    public void PlayRandomAnimalSound()
    {
        if (animalSounds.Length == 0 || audioSourceB == null) return;

        int index;

        if (animalSoundID >= 1 && animalSoundID <= 6)
        {
            index = animalSoundID - 1;
        }
        else
        {
            index = Random.Range(0, animalSounds.Length);
            animalCount = index;
        }

        audioSourceB.clip = animalSounds[index];
        audioSourceB.Play();
    }

    #endregion

    #region Button C Methods
    //---------------------------------------

    public void PlayRandomVoiceSound()
    {
        if (voiceSounds.Length == 0 || audioSourceC == null) return;

        int index;

        if (voiceSoundID >= 1 && voiceSoundID <= 6)
        {
            index = voiceSoundID - 1;
        }
        else
        {
            index = Random.Range(0, voiceSounds.Length);
            voiceCount = index;
        }

        audioSourceC.clip = voiceSounds[index];
        audioSourceC.Play();
    }

    #endregion

    #region Button D Methods
    //---------------------------------------

    public void PlayRandomMovementSound()
    {
        if (movementSounds.Length == 0 || audioSourceD == null) return;

        int index;

        if (movementSoundID >= 1 && movementSoundID <= 6)
        {
            index = movementSoundID - 1;
        }
        else
        {
            index = Random.Range(0, movementSounds.Length);
            movementCount = index;
        }

        audioSourceD.clip = movementSounds[index];
        audioSourceD.Play();
    }

    #endregion

    #region Button E Methods
    //---------------------------------------

    public void PlayRandomBirdSound()
    {
        if (birdSounds.Length == 0 || audioSourceE == null) return;

        int index;

        if (birdSoundID >= 1 && birdSoundID <= 6)
        {
            index = birdSoundID - 1;
        }
        else
        {
            index = Random.Range(0, birdSounds.Length);
            birdCount = index;
        }

        audioSourceE.clip = birdSounds[index];
        audioSourceE.Play();
    }

    #endregion

    public void ResetSound()
    {
        locationSoundID = 0;
        animalSoundID = 0;
        voiceSoundID = 0;
        movementSoundID = 0;
        birdSoundID = 0;

        PlayRandomLocationSound();
        PlayRandomAnimalSound();
        PlayRandomVoiceSound();
        PlayRandomMovementSound();
        PlayRandomBirdSound();
    }

    #endregion

}
