using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmotivUnityPlugin;
using System;
using TMPro;

/// <summary>
/// Handles the connection to EmotivBCI program.
/// </summary>
public class TrainingSystem : MonoBehaviour
{
    [SerializeField] private EmotivManager emotivManager;
    public TextMeshProUGUI secondsText;
    public string mentalCommandResponse { get; set; }
    public bool IsTrainingInProgress { get; set; }

    public bool EEGToggle, MOTToggle, PMToggle, CQToggle, POWToggle, EQToggle, COMToggle, FEToggle, SYSToggle = true;
    public bool createSession, startRecord, subscribe, unsubscribe, loadProfile, unloadProfile, saveProfile, startMCTraining, acceptMCTraining, rejectMCTraining, eraseMCTraining, stopRecord, injectMarker;

    [TextArea]
    public string messageLog;

    private EmotivUnityItf _eItf = EmotivUnityItf.Instance;
    private float _timerDataUpdate = 0;
    const float TIME_UPDATE_DATA = 1f;

    private string profileName, headsetId;
    [HideInInspector] public string selectedAction;
    [HideInInspector] public bool trainingActionCompleted = false;

    private bool countdownActive = false;
    private float countdownDuration = 8;

    void Start()
    {
        Debug.Log($"{emotivManager.clientId}, {emotivManager.clientSecret}, {emotivManager.appName}, {emotivManager.appVersion}");

        _eItf.Init(emotivManager.clientId, emotivManager.clientSecret, emotivManager.appName, emotivManager.appVersion, false);
        _eItf.Start();
    }

    void Update()
    {
        _timerDataUpdate += Time.deltaTime;
        if (_timerDataUpdate >= TIME_UPDATE_DATA)
        {
            _timerDataUpdate -= TIME_UPDATE_DATA;
            UpdateLogs();
            PerformActionsBasedOnToggles();

            if (_eItf.MentalCommandTrainingResponse.Contains("Succeeded") || _eItf.MentalCommandTrainingResponse.Contains("Failed"))
            {
                mentalCommandResponse = _eItf.MentalCommandTrainingResponse;
            }
        }

        profileName = emotivManager.profileName;
        headsetId = emotivManager.headsetId;


        if (countdownActive)
        {
            secondsText.text = "Seconds left: " + Mathf.Floor(countdownDuration).ToString();
            countdownDuration -= Time.deltaTime;

            if (countdownDuration <= 0f)
            {
                countdownActive = false;
                countdownDuration = 8;
                secondsText.text = "Seconds left: 8";
            }
        }


    }

    /// <summary>
    /// Triggers methods that handle connection to Emotiv headset, loading profile, subscribing to the data stream and training mental commands.
    /// </summary>
    void PerformActionsBasedOnToggles()
    {

        if (createSession && !_eItf.IsSessionCreated)
        {
            _eItf.CreateSessionWithHeadset(emotivManager.headsetId);
            createSession = false;
            trainingActionCompleted = true;
        }

        if (loadProfile && !_eItf.IsProfileLoaded)
        {
            _eItf.LoadProfile(emotivManager.profileName);
            loadProfile = false;
            trainingActionCompleted = true;
        }

        if (unloadProfile && _eItf.IsProfileLoaded)
        {
            _eItf.UnLoadProfile(emotivManager.profileName);
            unloadProfile = false;
            trainingActionCompleted = true;
        }

        if (saveProfile && _eItf.IsProfileLoaded)
        {
            _eItf.SaveProfile(emotivManager.profileName);
            saveProfile = false;
            trainingActionCompleted = true;
        }

        if (subscribe && _eItf.IsSessionCreated)
        {
            _eItf.SubscribeData(GetStreamsList());
            subscribe = false;
            trainingActionCompleted = true;
        }

        if (unsubscribe && _eItf.IsSessionCreated)
        {
            _eItf.UnSubscribeData(GetStreamsList());
            unsubscribe = false;
            trainingActionCompleted = true;
        }

        if (startMCTraining && _eItf.IsProfileLoaded)
        {
            _eItf.StartMCTraining(selectedAction);
            startMCTraining = false;
            trainingActionCompleted = true;
        }

        if (acceptMCTraining && _eItf.IsProfileLoaded)
        {
            _eItf.AcceptMCTraining();
            acceptMCTraining = false;
            trainingActionCompleted = true;
        }

        if (rejectMCTraining && _eItf.IsProfileLoaded)
        {
            _eItf.RejectMCTraining();
            rejectMCTraining = false;
            trainingActionCompleted = true;
        }

        if (eraseMCTraining && _eItf.IsProfileLoaded)
        {
            _eItf.EraseMCTraining(selectedAction);
            eraseMCTraining = false;
            trainingActionCompleted = true;
        }

        if (stopRecord && _eItf.IsRecording)
        {
            _eItf.StopRecord();
            stopRecord = false;
            trainingActionCompleted = true;
        }
    }

    /// <summary>
    /// Creates session with chosen EEG headset.
    /// </summary>
    /// <param name="onComplete"></param>
    public void CreateSession(Action onComplete)
    {
        string headsetId = emotivManager.headsetId;
        _eItf.CreateSessionWithHeadset(headsetId);
        onComplete?.Invoke();
    }

    /// <summary>
    /// Loads user training profile.
    /// </summary>
    /// <param name="onComplete"></param>
    public void LoadProfile(Action onComplete)
    {
        _eItf.LoadProfile(emotivManager.profileName);
        onComplete?.Invoke();
    }

    /// <summary>
    /// Subscribes to EmotivBCI data stream.
    /// </summary>
    /// <param name="onComplete"></param>
    public void Subscribe(Action onComplete)
    {
        _eItf.SubscribeData(GetStreamsList());
        onComplete?.Invoke();
    }

    /// <summary>
    /// Starts mental command training.
    /// </summary>
    /// <param name="onComplete"></param>
    public void StartMCTraining(Action onComplete)
    {
        countdownActive = true;
        _eItf.StartMCTraining(selectedAction);
        onComplete?.Invoke();
    }

    /// <summary>
    /// Accepts mental command training.
    /// </summary>
    /// <param name="onComplete"></param>
    public void AcceptMCTraining(Action onComplete)
    {
        _eItf.AcceptMCTraining();
        onComplete?.Invoke();
    }

    /// <summary>
    /// Rejects mental command training.
    /// </summary>
    /// <param name="onComplete"></param>
    public void RejectMCTraining(Action onComplete)
    {
        _eItf.RejectMCTraining();
        onComplete?.Invoke();
    }

    /// <summary>
    /// Erases mental command training.
    /// </summary>
    /// <param name="onComplete"></param>
    public void EraseMCTraining(Action onComplete)
    {
        _eItf.EraseMCTraining(selectedAction);
        onComplete?.Invoke();
    }

    /// <summary>
    /// Saves user training profile.
    /// </summary>
    /// <param name="onComplete"></param>
    public void SaveProfile(Action onComplete)
    {
        _eItf.SaveProfile(emotivManager.profileName);
        onComplete?.Invoke();
    }

    /// <summary>
    /// Handles what information is pulled into Unity from EmotivBCI.
    /// </summary>
    /// <returns></returns>
    private List<string> GetStreamsList()
    {
        List<string> streams = new List<string>();
        if (EEGToggle) streams.Add("eeg");
        if (MOTToggle) streams.Add("mot");
        if (PMToggle) streams.Add("met");
        if (CQToggle) streams.Add("dev");
        if (POWToggle) streams.Add("pow");
        if (EQToggle) streams.Add("eq");
        if (COMToggle) streams.Add("com");
        if (FEToggle) streams.Add("fac");
        if (SYSToggle) streams.Add("sys");
        return streams;
    }

    /// <summary>
    /// Logs Emotiv state changes.
    /// </summary>
    void UpdateLogs()
    {
        if (_eItf.MessageLog.Contains("Get Error:"))
        {
            messageLog = "Error: " + _eItf.MessageLog;
        }
        else
        {
            messageLog = _eItf.MessageLog;
        }
    }

    /// <summary>
    /// Stops EEG stream on when application quits.
    /// </summary>
    void OnApplicationQuit()
    {
        _eItf.Stop();
    }
}
