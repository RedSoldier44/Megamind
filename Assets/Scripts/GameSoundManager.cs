using System.Collections;
using UnityEngine;

public class GameSoundManager : MonoBehaviour
{

    #region Fields
    //---------------------------------------

    [Header("Clips")]
    public AudioSource audioSource;
    public AudioClip onBoardingClip;
    public AudioClip victoryClip;
    public AudioClip falseClip;

    [Header("Actual Combination")]
    public int[] targetCombination = new int[5];


    [Header("All buttons")]
    public ButtonSound[] objects = new ButtonSound[5];

    [Header("Begining Timer")]
    public float startGameTimer;

    [Header("Game Input")]
    public bool newGame = false;

    public bool replayCombination = false;

    public bool validateCombination = false;

    private bool starting = false;
    private bool won = false;
    public bool Won => won;

    #endregion

    #region Unity Methods
    //---------------------------------------

    void Start()
    {
        won = false;
        startGameTimer = 2f + onBoardingClip.length;
        //---------------------------------------

        audioSource.Stop();
        audioSource.clip = onBoardingClip;
        audioSource.Play();
        //---------------------------------------

        StartCoroutine(StartGame());
    }

    public void BigButtonPressed()
    {
        if (starting) return;
        if (!won) {
            NewGame();
        } else {
            ValidateCombination();
        }
    }

    public void NewGame()
    {
        if (starting) return;
        StopAllCoroutines();
        Start();
    }

    public void ReplayCombination()
    {
        if (starting) return;
        PlayTargetCombination();
    }

    public void ValidateCombination()
    {
        if (won || starting) return;
        if (CheckPlayerCombination()) {
            audioSource.clip = victoryClip;
            audioSource.Play();
            won = true;
        } else {
            audioSource.clip = falseClip;
            audioSource.Play();
            won = false;
        }
    }

    void Update()
    {
        if (starting) return;
        if (newGame) {
             NewGame();
             newGame = false;
        }

        if (replayCombination) {
            ReplayCombination();
            replayCombination = false;
        }

        if (validateCombination) {
            ValidateCombination();
            validateCombination = false;
        }
    }

    #endregion

    #region Custom Methods
    //---------------------------------------

    void GenerateCombination()
    {
        for (int i = 0; i < targetCombination.Length; i++)
        {
            targetCombination[i] = UnityEngine.Random.Range(0, objects[i].audioClips.Length);
        }
    }

    public bool CheckPlayerCombination()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].SoundID != targetCombination[i])
                return false;
        }
        return true;
    }

    public void PlayTargetCombination()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].PlayIndex(targetCombination[i]);
        }
    }

    public IEnumerator StartGame()
    {
        starting = true;
        yield return new WaitForSeconds(startGameTimer);
        starting = false;

        GenerateCombination();
        PlayTargetCombination();
    }

    #endregion

}
