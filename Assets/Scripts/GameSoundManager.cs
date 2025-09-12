using System.Collections;
using UnityEngine;

public class GameSoundManager : MonoBehaviour
{

    #region Fields
    //---------------------------------------

    [Header("Audio Source OnBoarding")]
    public AudioSource onBoarding;
    public AudioClip clip;

    [Header("Actual Combination")]
    public int[] targetCombination = new int[5];


    [Header("All buttons")]
    public ButtonSound[] objects = new ButtonSound[5];

    [Header("Begining Timer")]
    public float startGameTimer;

    [Header("Game Input")]
    public bool newGame = false;

    public bool replayCombination = false;

    #endregion

    #region Unity Methods
    //---------------------------------------

    void Start()
    {
        startGameTimer = 2f + clip.length;
        //---------------------------------------

        onBoarding.clip = clip;
        onBoarding.Play();
        //---------------------------------------

        StartCoroutine(StartGame());
    }

    void Update()
    {
        if (newGame == true)
        {
            GenerateCombination();
            newGame = false;
        }
        else return;


        if (replayCombination == true)
        {
            PlayTargetCombination();
            replayCombination = false;
        }
        else return;
    }

    #endregion

    #region Custom Methods
    //---------------------------------------

    void GenerateCombination()
    {
        for (int i = 0; i < targetCombination.Length; i++)
        {
            targetCombination[i] = Random.Range(0, objects[i].audioClips.Length);
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
        yield return new WaitForSeconds(startGameTimer);

        GenerateCombination();
        PlayTargetCombination();
    }

    #endregion

}
