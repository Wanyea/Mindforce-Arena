using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;

/// <summary>
/// Manages data coming from EmotivBCI program.
/// </summary>
public class TrainingManager : MonoBehaviour
{
    [Header ("Training Interface")]
    public GameObject startTrainingMenu;
    public GameObject continueMenu;
    public TrainingSystem trainingSystem;
    public int rounds = 10;

    [Header("Training UI")]
    public TextMeshProUGUI mentalCommandText;
    public TextMeshProUGUI roundsLeftText;
    public TextMeshProUGUI trainingText;
    public TextMeshProUGUI secondsText;

    [Header("Training Elements")]
    public GameObject trainingPedestal;
    public GameObject trainingEEGFireElement;
    public GameObject trainingEEGWaterElement;
    public GameObject trainingEEGEarthElement;
    public GameObject trainingEEGShockElement;

    [Header ("Training Trigger")]
    public bool startTraining = false;

    [Header("Mental Command Training Log")]
    [TextArea]
    public string mentalCommandsTrainingLog;

    // Mental command to element conversion table
    Dictionary<string, string> conversionTable = new Dictionary<string, string>()
    {
        {"neutral", "neutral"},
        {"lift", "fire"},
        {"left", "earth"},
        {"right", "shock"},
        {"drop", "water"}
    };

    private void Start()
    {
        MentalCommands mentalCommands = GameObject.FindGameObjectWithTag("MentalCommands").GetComponent<MentalCommands>();
    }
    void Update()
    {
        if (startTraining && !trainingSystem.IsTrainingInProgress)
        {
            startTraining = false;
            StartCoroutine(StartTrainingLoop());
        }
    }

    public void SetStartTraining()
    {
        startTraining = true;
        startTrainingMenu.SetActive(false);
    }

    /// <summary>
    /// Handles training mental commands for all five mental commands, eight seconds each for ten rounds.
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartTrainingLoop()
    {
        List<string> mentalCommands = new List<string>() { "neutral", "drop", "lift", "left", "right" };
        trainingSystem.IsTrainingInProgress = true;

        foreach (var action in mentalCommands)
        {
            // Convert the action to its elemental representation using the conversion table
            string elementalAction = conversionTable[action];

            // Set the text to the elemental representation of the action
            mentalCommandText.text = elementalAction;


            int successfulAttempts = 0; // Track successful attempts
            while (successfulAttempts < rounds) // Ensure 10 successful attempts are made for each action
            {
                roundsLeftText.text = $"Rounds left: {rounds - successfulAttempts}";
                mentalCommandsTrainingLog = $"Training for action: {action}, Attempt: {successfulAttempts + 1}";
                trainingSystem.selectedAction = action;
                trainingSystem.mentalCommandResponse = "";
                
                
                yield return TrainingSequence(action);

                // React based on the training outcome
                if (!trainingSystem.mentalCommandResponse.Contains("Failed"))
                {
                    successfulAttempts++; // Increment only on success
                }
                else
                {
                    roundsLeftText.text = $"Training round unsucessful... trying again...";
                    Debug.Log($"Training attempt for {action} failed, retrying...");
                    // If "Failed", do not increment successfulAttempts, thereby retrying
                }

                // Adding a short delay to ensure proper handling of rapid consecutive attempts
                yield return new WaitForSeconds(0.5f);
            }
        }

        trainingText.text = "Training Complete!";
        Debug.Log("Training for all actions completed.");
        trainingSystem.IsTrainingInProgress = false;

        secondsText.transform.root.gameObject.SetActive(false);
        roundsLeftText.transform.root.gameObject.SetActive(false);
        mentalCommandText.transform.root.gameObject.SetActive(false);

        continueMenu.SetActive(true);
        startTrainingMenu.SetActive(false);
    }

    /// <summary>
    /// Starts training coroutines.
    /// </summary>
    /// <returns></returns>
    private IEnumerator TrainingSequence(string mentalCommand)
    {
        Debug.Log("Starting Training Round");

        // Execute each step of the training sequence
        yield return StartCoroutine(CreateSession());
        yield return StartCoroutine(LoadProfile());
        yield return StartCoroutine(Subscribe());
        yield return StartCoroutine(StartMCTraining(mentalCommand));

        // It's crucial to wait for a response after starting training
        // Assuming the system updates 'mentalCommandResponse' with the outcome
        yield return new WaitUntil(() => !string.IsNullOrEmpty(trainingSystem.mentalCommandResponse));

        yield return StartCoroutine(AcceptMCTraining());
        yield return StartCoroutine(SaveProfile());

        Debug.Log("Training Round Completed");
    }

    private IEnumerator CreateSession()
    {
        bool completed = false;
        trainingSystem.CreateSession(() => completed = true);
        yield return new WaitUntil(() => completed);
    }

    private IEnumerator LoadProfile()
    {
        bool completed = false;
        trainingSystem.LoadProfile(() => completed = true);
        yield return new WaitUntil(() => completed);
    }

    private IEnumerator Subscribe()
    {
        bool completed = false;
        trainingSystem.Subscribe(() => completed = true);
        yield return new WaitUntil(() => completed);
    }

    private IEnumerator StartMCTraining(string mentalCommand)
    {
        StartCoroutine(EnableObjectTemporary(mentalCommand));

        bool completed = false;
        trainingSystem.StartMCTraining(() => completed = true);
        yield return new WaitUntil(() => completed);
    }

    private IEnumerator AcceptMCTraining()
    {
        bool completed = false;
        trainingSystem.AcceptMCTraining(() => completed = true);
        yield return new WaitUntil(() => completed);
    }

    private IEnumerator SaveProfile()
    {
        bool completed = false;
        trainingSystem.SaveProfile(() => completed = true);
        yield return new WaitUntil(() => completed);
    }

    private IEnumerator EnableObjectTemporary(string mentalCommand)
    {
        switch (mentalCommand)
        {
            case "lift":
                trainingEEGFireElement.SetActive(true);
                MentalGameplayManager.Instance.TriggerFireVibration(); // Start fire vibration
                yield return new WaitForSeconds(8);
                trainingEEGFireElement.SetActive(false);
                break;
            case "drop":
                trainingEEGWaterElement.SetActive(true);
                MentalGameplayManager.Instance.TriggerWaterVibration(); // Start water vibration
                yield return new WaitForSeconds(8);
                trainingEEGWaterElement.SetActive(false);
                break;
            case "left":
                trainingEEGEarthElement.SetActive(true);
                MentalGameplayManager.Instance.TriggerEarthVibration(); // Start earth vibration
                yield return new WaitForSeconds(8);
                trainingEEGEarthElement.SetActive(false);
                break;
            case "right":
                trainingEEGShockElement.SetActive(true);
                MentalGameplayManager.Instance.TriggerShockVibration(); // Start shock vibration
                yield return new WaitForSeconds(8);
                trainingEEGShockElement.SetActive(false);
                break;
        }
    }
}


