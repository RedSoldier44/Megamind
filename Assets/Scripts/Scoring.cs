using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scoring : MonoBehaviour
{

    #region Fields
    //-------------------------------------------------

    [SerializeField] private float score;

    [SerializeField, HideInInspector] private float timer;

    [SerializeField] private float animalButtonScore;

    [SerializeField] private float locationButtonScore;

    [SerializeField] private float voicesButtonScore;

    [SerializeField] private float movementButtonScore;

    [SerializeField] private float birdsButtonScore;

    [SerializeField] private float resetButtonScore;


    [SerializeField] private TMP_Text m_scoreTxt;

    #endregion

    #region Unity Methods
    //-------------------------------------------------


    void Start()
    {
        
    }


    void Update()
    {
        timer = Time.time;

        int time = (int)timer;

        m_scoreTxt.text = $"{time}";

    }

    #endregion

    #region Custom Methods
    //-------------------------------------------------

    #endregion
}
