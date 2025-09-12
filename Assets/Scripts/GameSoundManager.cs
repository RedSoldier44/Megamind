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

    private bool skipTuto = false;

    private bool hasStart = false;
    private bool starting = false;
    private bool won = false;
    public bool Won => won;

    #endregion

    #region Unity Methods
    //---------------------------------------
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            skipTuto = true;
        }

        if (starting) return;
        if (newGame)
        {
            NewGame();
            newGame = false;
        }

        if (replayCombination)
        {
            ReplayCombination();
            replayCombination = false;
        }

        if (validateCombination)
        {
            ValidateCombination();
            validateCombination = false;
        }
    }

    #endregion

    #region Custom Methods
    //---------------------------------------

    private void PlayGame()
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
        if (!won)
        {
            NewGame();
        }
        else if (hasStart)
        {
            ValidateCombination();
        }
    }

    public void NewGame()
    {
        if (starting) return;
        SetReady(true);
        StopAllCoroutines();
        PlayGame();
    }

    public void ReplayCombination()
    {
        if (!hasStart) return;
        if (starting) return;
        PlayTargetCombination();
    }

    public void ValidateCombination()
    {
        if (!hasStart) return;
        if (won || starting) return;
        if (CheckPlayerCombination())
        {
            audioSource.clip = victoryClip;
            audioSource.Play();
            SetReady(false);
            won = true;
        }
        else
        {
            audioSource.clip = falseClip;
            audioSource.Play();
            SetReady(false);
            won = false;
        }
    }

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

    private void SetReady(bool state)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].isReady = state;
        }
    }

    public IEnumerator StartGame()
    {
        starting = true;
        skipTuto = false;
        hasStart = false;

        float timer = 0.0f;
        WaitForSeconds wfs = new WaitForSeconds(0.1f);
        while (timer < startGameTimer) {
            yield return wfs;
            timer += 0.1f;
            if (skipTuto) timer = startGameTimer;
        }
        
        starting = false;
        skipTuto = false;
        hasStart = true;

        GenerateCombination();
        PlayTargetCombination();
    }

    #endregion

}
