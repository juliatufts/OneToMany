using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SessionManager : MonoBehaviour {

    private enum SessionStatus
    {
        Standby, // Just ended, or waiting to start
        Tutorial,
        InGame
    }

    public static SessionManager Instance;

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

    void Start ()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one SessionManager in scene");
        }
        Instance = this;

        sessionDurationInSeconds = sessionDurationInMinutes * 60f;
        //TODO: enter standby mode
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
        //TODO: Set up for tutorial

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
}
