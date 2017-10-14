using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

public class SessionManager : MonoBehaviour {

    private enum SessionStatus
    {
        Standby, // Just ended, or waiting to start
        Tutorial,
        InGame
    }

    public static SessionManager Instance;

    public PostProcessingProfile mainProfile;
    public float sessionDurationInMinutes = 1f;
    public Text sessionNumberText;
    public Text sessionStatusText;
    public Text progressText;
    public Button NewSessionButton;
    public Button cancelButton;

    //[HideInInspector]
    public bool tutorialComplete;

    private int sessionNumber;
    private float sessionDurationInSeconds;
    private float currentSessionProgressInSeconds;
    private SessionStatus sessionStatus;
    private Vector4 originalSkyboxColorPhase;

    void Start ()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one SessionManager in scene");
        }
        Instance = this;

        sessionDurationInSeconds = sessionDurationInMinutes * 60f;
        originalSkyboxColorPhase = RenderSettings.skybox.GetVector("_ColorPhase");

        //TODO: enter standby mode
        var newSettings = mainProfile.vignette.settings;
        newSettings.intensity = 0f;
        mainProfile.vignette.settings = newSettings;
    }

    void OnDestroy()
    {
        var newSettings = mainProfile.vignette.settings;
        newSettings.intensity = 0f;
        mainProfile.vignette.settings = newSettings;
    }
	
	void Update ()
    {
        switch (sessionStatus)
        {
            case SessionStatus.Standby:
                break;
            case SessionStatus.Tutorial:
                if (tutorialComplete)
                {
                    sessionStatus = SessionStatus.InGame;
                    //TODO: Set up for game
                }
                break;
            case SessionStatus.InGame:
                currentSessionProgressInSeconds += Time.deltaTime;
                if (currentSessionProgressInSeconds >= sessionDurationInSeconds)
                {
                    // Session has ended
                    sessionStatus = SessionStatus.Standby;
                    //TODO: Shut down some things
                }
                break;
            default:
                break;
        }

        // Update displays
        sessionNumberText.text = "Session " + sessionNumber;
        sessionStatusText.text = sessionStatus.ToString();
        progressText.text = (currentSessionProgressInSeconds / 60f).ToString("F2") + " / " + sessionDurationInMinutes.ToString("F2");

        NewSessionButton.interactable = (sessionStatus == SessionStatus.Standby);
        cancelButton.interactable = (sessionStatus != SessionStatus.Standby);
    }

    public void StartNewSession()
    {
        StartCoroutine(BlinkTransition(0.1f, 0f));

        sessionStatus = SessionStatus.Tutorial;
        currentSessionProgressInSeconds = 0f;
        sessionNumber++;
    }

    public void Cancel()
    {
        if (sessionStatus != SessionStatus.Standby)
        {
            sessionStatus = SessionStatus.Standby;
            //TODO: Shut down some things
        }
    }

    IEnumerator BlinkTransition(float delta, float waitTime)
    {
        var progress = 0f;
        VignetteModel.Settings newSettings;

        while (progress < 1f)
        {
            newSettings = mainProfile.vignette.settings;
            newSettings.intensity = Mathf.Lerp(0f, 1f, progress);
            mainProfile.vignette.settings = newSettings;

            progress += (delta * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        newSettings = mainProfile.vignette.settings;
        newSettings.intensity = 1f;
        mainProfile.vignette.settings = newSettings;
        yield return new WaitForSeconds(waitTime);

        // DO STUFF HERE
        // maybe wait a minute

        progress = 0f;
        while (progress < 1f)
        {
            newSettings = mainProfile.vignette.settings;
            newSettings.intensity = Mathf.Lerp(1f, 0f, progress);
            mainProfile.vignette.settings = newSettings;

            progress += (delta * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        newSettings = mainProfile.vignette.settings;
        newSettings.intensity = 0f;
        mainProfile.vignette.settings = newSettings;
    }
}
