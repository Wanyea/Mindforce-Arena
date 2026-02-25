using System.Collections;
using UnityEngine;

/// <summary>
/// Controls EEG gameplay
/// </summary>
public class MentalGameplayManager : MonoBehaviour
{
    public static MentalGameplayManager Instance;

    [Header("Player")]
    public bool player1;
    public bool player2;

    [Header("Gameplay Handlers")]
    public bool startGameplay = false;
    public bool isGameplayActive = false;

    [Header("EEG Components")]
    [SerializeField] private GameObject mentalCommandsManager;
    private MentalCommands mentalCommands;
    public PowerTracker powerTracker;
    public GameObject p1ElementPedestal;
    [SerializeField] public GameObject p1Aura;
    public GameObject p2ElementPedestal;
    [SerializeField] public GameObject p2Aura;
    [HideInInspector] public GameObject elementPedestal;
    public Spawner currentSpawner;

    [Header("P1 EEG Elements")]
    public GameObject p1FireEEGElement;
    public GameObject p1WaterEEGElement;
    public GameObject p1EarthEEGElement;
    public GameObject p1ShockEEGElement;

    [Header("P2 EEG Elements")]
    public GameObject p2FireEEGElement;
    public GameObject p2WaterEEGElement;
    public GameObject p2EarthEEGElement;
    public GameObject p2ShockEEGElement;

    [Header("Audio Feedback")]
    public AudioClip correctSound;
    public AudioClip wrongSound;
    [SerializeField] private GameObject audioObject;
    private AudioSource audioSource;

    [Header("Haptic Feedback")]
    public bool fireVibration = false;
    public bool waterVibration = false;
    public bool earthVibration = false;
    public bool shockVibration = false;

    [Space(15)]

    public float fireIntensity = 0.8f;
    public float waterIntensity = 0.7f;
    public float earthIntensity = 0.9f;
    public float shockIntensity = 0.95f;

    [Header("Mental Commands")]
    [SerializeField] private string currentMentalCommand;
    [SerializeField] private string expectedMentalCommand;

    private GameObject fireEEGElement;
    private GameObject waterEEGElement;
    private GameObject earthEEGElement;
    private GameObject shockEEGElement;
    private GameObject playerAura;

    void Awake()
    {
        Instantiate(mentalCommandsManager);
        mentalCommandsManager.tag = "MentalCommands";
        mentalCommandsManager = GameObject.FindGameObjectWithTag("MentalCommands");
    }

    void Start()
    { 
        if (audioSource != null)
            audioSource = audioObject.GetComponent<AudioSource>();

        if (mentalCommandsManager != null)
            mentalCommands = mentalCommandsManager.GetComponent<MentalCommands>();
        
        if(powerTracker == null)
            Debug.LogWarning("PoweTracker reference not set!");

        if (player1) 
        {
            elementPedestal = p1ElementPedestal;

            fireEEGElement = p1FireEEGElement;
            waterEEGElement = p1WaterEEGElement;
            shockEEGElement = p1ShockEEGElement;
            earthEEGElement = p1EarthEEGElement;
            playerAura = p1Aura;

        } else
        {
            elementPedestal = p2ElementPedestal;

            fireEEGElement = p2FireEEGElement;
            waterEEGElement = p2WaterEEGElement;
            shockEEGElement = p2ShockEEGElement;
            earthEEGElement = p2EarthEEGElement;
            playerAura = p2Aura;
        }

    }

    void Update()
    {
        currentMentalCommand = mentalCommands.GetMentalCommand();

        if (powerTracker.typeSelected != -1)
            startGameplay = true;

        if (startGameplay)
        {
            startGameplay = false;
            isGameplayActive = true;

            SetExpectedMentalCommand();

            while (currentMentalCommand != "neutral" && isGameplayActive)
                ProcessCommand(currentMentalCommand);

        }

        if (fireVibration) 
        {
            fireVibration = false;
            TriggerFireVibration();
        }

        if (waterVibration)
        {
            waterVibration = false;
            TriggerWaterVibration();
        }

        if (earthVibration)
        {
            earthVibration = false;
            TriggerEarthVibration();
        }

        if (shockVibration)
        {
            shockVibration = false;
            TriggerShockVibration();
        }

    }

    /// <summary>
    /// Gameplay loop that handles EEG gameplay and sets power level.
    /// </summary>
    private void ProcessCommand(string command)
    {
        isGameplayActive = false;
        DisableAllObjects(); 

        // Enable the corresponding object based on the command
        switch (command)
        {
            case "drop":
                StartCoroutine(EnableObjectTemporary(waterEEGElement, 2f));
                break;
            case "lift":
                StartCoroutine(EnableObjectTemporary(fireEEGElement, 2f));
                break;
            case "left":
                StartCoroutine(EnableObjectTemporary(earthEEGElement, 2f));
                break;
            case "right":
                StartCoroutine(EnableObjectTemporary(shockEEGElement, 2f)); 
                break;
            default:
                Debug.LogWarning("Unknown command received.");
                break;
        }

        if (command == GetOppositeElement(expectedMentalCommand)) 
        {
            DoForOpposite();
        }
        else if (command == expectedMentalCommand)
        {
            DoForMatch();
        } else
        {
            DoForOthers();
        }

        currentSpawner.SpawnObject();
    }

    private IEnumerator EnableObjectTemporary(GameObject obj, float duration)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(duration);
        obj.SetActive(false);
        ToggleAura(false);
    }

    private void DisableAllObjects()
    {
        waterEEGElement.SetActive(false);
        fireEEGElement.SetActive(false);
        earthEEGElement.SetActive(false);
        shockEEGElement.SetActive(false);
    }

    private void DoForMatch()
    {
        if (powerTracker != null)
        {
            powerTracker.comboPower = 3;
        }
    }

    private void DoForOpposite()
    {
        if (powerTracker != null)
        {
            powerTracker.comboPower = 1;
        }
    }

    private void DoForOthers()
    {
        if (powerTracker != null)
        {
            powerTracker.comboPower = 2;
        }
    }

    private void SetExpectedMentalCommand() 
    {
        switch (powerTracker.typeSelected)
        {
            case 0:
                expectedMentalCommand = "lift";
                break;
            case 1:
                expectedMentalCommand = "drop";
                break;
            case 2:
                expectedMentalCommand = "right";
                break;
            case 3:
                expectedMentalCommand = "left";
                break;
            default:
                Debug.LogError("Invalid selection. Please choose a value between 0 and 3.");
                break;
        }
    }

    private string GetOppositeElement(string command)
    {   
        if (command == "lift")
        {
            return "drop";
        } else if (command == "drop")
        {
            return "lift";
        } else if (command == "left")
        {
            return "right";
        } else if (command == "right")
        {
            return "left";
        } else
        {
            return command;
        }

    }

    private string MapToElement(string mentalCommand)
    {
        switch (mentalCommand)
        {
            case "lift":
                return "fire";
            case "drop":
                return "water";
            case "left":
                return "earth";
            case "right":
                return "shock";
            default:
                return "fire";
        }
    }

    // Trigger fire vibration (continuous low vibration)
    public void TriggerFireVibration()
    {
        StopAllCoroutines(); // Ensure that no other vibrations are running
        StartCoroutine(FireVibration(0.5f, 0.8f, 0.05f, 0.5f)); 
    }

    // Trigger water vibration (pulsing)
    public void TriggerWaterVibration()
    {
        StopAllCoroutines(); // Ensure that no other vibrations are running
        StartCoroutine(PulseVibration(0.5f, 0.8f, 0.3f, 0.7f)); 
    }


    // Trigger earth vibration (hard start and stop)
    public void TriggerEarthVibration()
    {
        StopAllCoroutines(); // Ensure no other vibration patterns are running
        StartCoroutine(ContinuousEarthquakeVibration(1.0f, 0.4f, 1.5f)); // Example parameters, adjust as needed
    }

    // Trigger shock vibration (between two controllers)
    public void TriggerShockVibration()
    {
        StopAllCoroutines(); // Ensure that no other vibrations are running
        StartCoroutine(ShockVibration(0.6f, 1.0f, 0.3f)); 
    }


    // Improved Fire Vibration: Simulate fire growing and diminishing
    IEnumerator FireVibration(float startIntensity, float maxIntensity, float step, float duration)
    {
        float intensity = startIntensity;
        while (true)
        {
            // Increase intensity to simulate the fire growing
            while (intensity < maxIntensity)
            {
                OVRInput.SetControllerVibration(intensity, intensity, OVRInput.Controller.All);
                intensity += step;
                yield return new WaitForSeconds(duration);
            }
            // Decrease intensity to simulate the fire diminishing
            while (intensity > startIntensity)
            {
                OVRInput.SetControllerVibration(intensity, intensity, OVRInput.Controller.All);
                intensity -= step;
                yield return new WaitForSeconds(duration);
            }
        }
    }

    // Improved Water Vibration: Simulate waves with varying intensities and durations
    IEnumerator PulseVibration(float minIntensity, float maxIntensity, float minDuration, float maxDuration)
    {
        float intensity = minIntensity;
        float duration = minDuration;
        bool increasing = true;
        while (true)
        {
            OVRInput.SetControllerVibration(intensity, intensity, OVRInput.Controller.All);
            yield return new WaitForSeconds(duration);
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.All);
            yield return new WaitForSeconds(duration);

            // Adjust intensity and duration for the next pulse
            if (increasing)
            {
                intensity = Mathf.Min(maxIntensity, intensity + 0.1f);
                duration = Mathf.Min(maxDuration, duration + 0.1f);
                if (intensity == maxIntensity) increasing = false;
            }
            else
            {
                intensity = Mathf.Max(minIntensity, intensity - 0.1f);
                duration = Mathf.Max(minDuration, duration - 0.1f);
                if (intensity == minIntensity) increasing = true;
            }
        }
    }

    // Improved Earth Vibration: Simulate earthquake tremors with multiple hard vibrations
    // Continuous Earth Vibration: Repeats the original hard start and stop vibration pattern
    IEnumerator ContinuousEarthquakeVibration(float intensity, float duration, float pauseDuration)
    {
        while (true)
        {
            // Original hard start and stop vibration pattern
            OVRInput.SetControllerVibration(intensity, intensity, OVRInput.Controller.All);
            yield return new WaitForSeconds(duration); // Duration of the hard vibration
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.All);

            // Pause before repeating the pattern
            yield return new WaitForSeconds(pauseDuration); // Pause duration between repetitions
        }
    }

    // Improved Shock Vibration: Simulate lightning strikes with alternating and varying intensities
    IEnumerator ShockVibration(float minIntensity, float maxIntensity, float duration)
    {
        float intensity = minIntensity;
        bool isLeft = true;
        while (true)
        {
            if (isLeft)
            {
                OVRInput.SetControllerVibration(intensity, 0, OVRInput.Controller.LTouch);
                OVRInput.SetControllerVibration(0, intensity, OVRInput.Controller.RTouch);
            }
            else
            {
                OVRInput.SetControllerVibration(0, intensity, OVRInput.Controller.LTouch);
                OVRInput.SetControllerVibration(intensity, 0, OVRInput.Controller.RTouch);
            }
            intensity = Random.Range(minIntensity, maxIntensity); // Vary intensity
            yield return new WaitForSeconds(duration);
            isLeft = !isLeft; // Switch sides
        }
    }



    public void ToggleAura(bool toggle) 
    {
        playerAura.SetActive(toggle);
    }

    public void StartGameplay() 
    {
        startGameplay = true;
    }

    public void ActiveSpawner(Spawner spawner)
    {
        currentSpawner = spawner;
    }
}
