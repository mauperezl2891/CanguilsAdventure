
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.Tilemaps;

public class EventController : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private CharacterBehaviour characterController;

    private bool isPlayingWarningSound = false;
    private bool isPlayingBackgroundSound = false;
    private bool isTimerActive = false;

    [SerializeField] private AudioClip warningSound;
    [SerializeField] private AudioClip backgroundSound;
    [SerializeField] private AudioSource BGM;
    [SerializeField] private AudioSource BGM2;
    [SerializeField] private AudioClip BGM_GameOver;
    [SerializeField] private GameObject tooltip;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject freedom;
    [SerializeField] private LayerMask shadowMask;
    [SerializeField] private LayerMask lightMask;
    [SerializeField] public Tilemap layerLight;
    [SerializeField] public Tilemap layerShadow;
    private bool isGameOver = false;
    public bool isCharacterInsideShadowLayer = true;
    private bool isInverted = false;
    


    private float currentTime = 10.0f;
    private float startingTime = 10f;


    private void Start()
    {

        characterController = FindObjectOfType<CharacterBehaviour>();
        currentTime = startingTime;

        layerLight.gameObject.SetActive(true);
        layerShadow.gameObject.SetActive(true);
    }

    private void Update()
    {

        isInverted = characterController.isInverted;

        if(characterController.gameObject.transform.position.x >= 153f)
        {
            Freedom();
        }

       if (isCharacterInsideShadowLayer == true && isInverted == true)
        {
            StartTimer();
        }
        else if (isCharacterInsideShadowLayer == false && isInverted == false)
        {
            StartTimer();
        }
        else if ((isCharacterInsideShadowLayer == true && isInverted == false ) || (isCharacterInsideShadowLayer == false && isInverted == true ))
        {
            StopTimer();
        }
    }

    public void Freedom()
    {
        Time.timeScale = 0f;
        freedom.gameObject.SetActive(true);
    }

    public void CharacterEnteredCollider(Collider2D characterCollider)
    {
 


        if (characterCollider.gameObject.layer == LayerMask.NameToLayer("Light"))
        {
            Debug.Log("Light   "+ isCharacterInsideShadowLayer);
            isInverted = characterController.isInverted;
            isCharacterInsideShadowLayer = false;
            if (isInverted == false)
            {
                StartTimer();
            }
        }
        else if (characterCollider.gameObject.layer == LayerMask.NameToLayer("Shadow"))
        {
            Debug.Log("Shadow    "+ isCharacterInsideShadowLayer);
            isInverted = characterController.isInverted;
            isCharacterInsideShadowLayer = true;
            if (isInverted == true)
            {
                StartTimer();
            }
        }
        else if (characterCollider.gameObject.layer == LayerMask.NameToLayer("Info"))
        {
            tooltip.SetActive(true);
        }

    }

    public void HideInfo()
    {
        tooltip.SetActive(false);
    }


    public void CallTimer()
    {
        currentTime -= 1 * Time.deltaTime;
        if (currentTime <= 0 )
        {
            currentTime = 0;
            GameOver();
        }
        UpdateTimerText();
    }

    private void GameOver()
    {
        if (!isGameOver)
        {
            Time.timeScale = 0f;
            gameOver.gameObject.SetActive(true);
            BGM.Pause();
            BGM2.Pause();
            BGM.clip = BGM_GameOver;
            BGM.Play();
        }
    }

    private void StartTimer()
    {
        if (isCharacterInsideShadowLayer == true && isInverted == true)
        {
            if (!isPlayingWarningSound)
            {
                ChangeSound(warningSound,1);
                isPlayingWarningSound = true;
                isPlayingBackgroundSound = false;
            }
            isTimerActive = true;
            timerText.gameObject.SetActive(true);
            CallTimer();
        }
        else if (isCharacterInsideShadowLayer == false && isInverted == false)
        {
            if (!isPlayingWarningSound)
            {
                ChangeSound(warningSound,1);
                isPlayingWarningSound = true;
                isPlayingBackgroundSound = false;
            }
            isTimerActive = true;
            timerText.gameObject.SetActive(true);
            CallTimer();
        }
        else
        {
            StopTimer();
        }
    }


    private void UpdateTimerText()
    {
        if (timerText != null && isTimerActive)
        {
            timerText.text = "Time : " + currentTime.ToString("0");
        }
        else
        {
            StopTimer();
        }
    }


    private void StopTimer()
    {
        if (!isPlayingBackgroundSound)
        {
            ChangeSound(backgroundSound,2);
            isPlayingBackgroundSound = true;
            isPlayingWarningSound = false;
        }
        timerText.text = "";
        currentTime = startingTime;
        isTimerActive = false;
        timerText.gameObject.SetActive(false);
    }

    public void ChangeSound(AudioClip track, int who)
    {
        if (who == 1 && BGM.isPlaying)
        {
            BGM.Pause();
            BGM2.Play();
        }else if(who == 2 && BGM2.isPlaying)
        {
            BGM2.Pause();
            BGM.Play();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Retry()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // string currentSceneName = SceneManager.GetActiveScene().name;

        // Vuelve a cargar la escena actual
        SceneManager.LoadScene(currentSceneIndex);
    }
}