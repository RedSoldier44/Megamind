using UnityEngine;

public class GameSoundManager : MonoBehaviour
{

    #region Fields
    //---------------------------------------

    [Header("Actual Combination")]
    public int[] targetCombination = new int[5];


    [Header("All buttons")]
    public ButtonSound[] objects = new ButtonSound[5];

    public bool newGame = false;

    public bool replayCombination = false;

    #endregion

    #region Unity Methods
    //---------------------------------------

    void Start()
    {
        GenerateCombination();
        //---------------------------------------

        PlayTargetCombination();
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

    #endregion

}
