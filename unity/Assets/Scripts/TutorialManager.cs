using UnityEngine;
using TMPro;

/// <summary>
/// Handles stepping through each step of the tutorial
/// </summary>
public class TutorialManager : MonoBehaviour
{
    public GameObject startMenuButton;
    public GameObject tutorialUI;
    private TextMeshProUGUI tutorialText;
    [SerializeField] private GameObject tutorialOrbs;
    [SerializeField] private GameObject auraVisual;
    [SerializeField] private GameObject exampleElement;

    public enum TutorialStep
    {
        Step1,
        Step2,
        Step3,
        Step4,
        Step5,
        Step6,
        Step7,
        Step8,
        Step9,
        Step10,
        Step11,
        TutorialComplete
    }

    public TutorialStep currentStep = TutorialStep.Step2;

    void Start()
    {
        tutorialText = tutorialUI.GetComponent<TextMeshProUGUI>();
        currentStep = TutorialStep.Step2;

    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
        {

        }

        // Check if the trigger button was pressed
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger)) 
        {
            AdvanceTutorial(true);
        }

    }

    void AdvanceTutorial(bool forward)
    {
        switch (currentStep)
        {
            case TutorialStep.Step1:
                tutorialText.text = "Welcome to Mindforce Arena!";
                tutorialText.fontSize = 87.36f;

                break;
            case TutorialStep.Step2:
                Debug.Log("Current Step is 2");
                StartStepTwo();
                break;
            case TutorialStep.Step3:
                StartStepThree();
                Debug.Log("Current Step is 3");
                break;
            case TutorialStep.Step4:
                StartStepFour();
                Debug.Log("Current Step is 4");
                break;
            case TutorialStep.Step5:
                StartStepFive();
                Debug.Log("Current Step is 5");
                break;
            case TutorialStep.Step6:
                StartStepSix();
                Debug.Log("Current Step is 6");
                break;
            case TutorialStep.Step7:
                StartStepSeven();
                Debug.Log("Current Step is 7");
                break;
            case TutorialStep.Step8:
                StartStepEight();
                Debug.Log("Current Step is 8");
                break;
            case TutorialStep.Step9:
                StartStepNine();
                Debug.Log("Current Step is 9");
                break;
            case TutorialStep.Step10:
                StartStepTen();
                Debug.Log("Current Step is 10");
                break;
            case TutorialStep.Step11:
                StartStepEleven();
                Debug.Log("Current Step is 11");
                break;
            case TutorialStep.TutorialComplete:
                Debug.Log("Current Step is Tutorial Complete");
                TutorialComplete();
                break;
 
        }

        // Move to the next step
        if (currentStep < TutorialStep.TutorialComplete && forward)
        {
            currentStep++;
        } else if (currentStep > TutorialStep.Step1 && !forward)
        {
            currentStep--;
        }
    }

    void StartStepTwo() 
    {
        if (tutorialText != null)
        {
            tutorialText.fontSize = 40.0f;
            tutorialText.text = "This tutorial will explain how to play Mindforce Arena!";
        }
        else
        {
            Debug.Log("No TextMeshPro");
        }
    }

    void StartStepThree() 
    {
        if (tutorialText != null)
        {
            tutorialText.fontSize = 40.0f;
            tutorialText.text = "You will be battling the opponent across from you and need to place spell down in a lane to attack and defend yourself.";
        }
        else
        {
            Debug.Log("No TextMeshPro");
        }
    }

    void StartStepFour()
    {
        if (tutorialText != null)
        {
            tutorialText.fontSize = 40.0f;
            tutorialText.text = "To put down a spell, you first must pick a lane, aim your cursor on a crystal spawner, and click the trigger!";
        }
        else
        {
            Debug.Log("No TextMeshPro");
        }
    }

    void StartStepFive()
    {
        if (tutorialText != null)
        {
            tutorialText.fontSize = 40.0f;
            tutorialText.text = "You will then need to pick the type of spell you want to cast by choosing a spell with the left thumbstick.";
            auraVisual.SetActive(false);
            exampleElement.SetActive(false);
        }
        else
        {
            Debug.Log("No TextMeshPro");
        }
    }

    void StartStepSix()
    {
        if (tutorialText != null)
        {
            tutorialText.fontSize = 40.0f;
            tutorialText.text = "Once the pedestal starts glowing, set the spell power of your element by thinking of the same element as the one you picked!";
            auraVisual.SetActive(true);
            exampleElement.SetActive(false);

        }
        else
        {
            Debug.Log("No TextMeshPro");
        }
    }

    void StartStepSeven()
    {
        if (tutorialText != null)
        {
            tutorialText.fontSize = 40.0f;
            tutorialText.text = "The element you thought of will appear on the pedestal for a moment and your spell will begin moving down the lane.";
            exampleElement.SetActive(true);
            auraVisual.SetActive(true);
        }
        else
        {
            Debug.Log("No TextMeshPro");
        }
    }

    void StartStepEight()
    {
        if (tutorialText != null)
        {
            tutorialText.fontSize = 40.0f;
            tutorialText.text = "Thinking the correct element will award you a spell with a power level of 3, thinking the element its weak to will award you a power of 1, thinking of the other two will award you a power of 2";
            tutorialOrbs.SetActive(false);
            exampleElement.SetActive(false);
            auraVisual.SetActive(false);

        }
        else
        {
            Debug.Log("No TextMeshPro");
        }
    }

    void StartStepNine()
    {
        if (tutorialText != null)
        {
            tutorialOrbs.SetActive(true);
            tutorialText.fontSize = 40.0f;
            tutorialText.text = "Fire is weak to Water! \n Water is weak to Lightning! \n Lightning is weak to Earth! \n Earth is weak to Fire! \n";

        }
        else
        {
            Debug.Log("No TextMeshPro");
        }
    }

    void StartStepTen()
    {
        if (tutorialText != null)
        {
            tutorialText.fontSize = 40.0f;
            tutorialText.text = "When two spells meet, the stronger spell will keep moving. Spells types that are stronger than the other do one more point of damage!";

            tutorialOrbs.SetActive(false);

        }
        else
        {
            Debug.Log("No TextMeshPro");
        }
    }

    void StartStepEleven()
    {
        if (tutorialText != null)
        {
            tutorialText.fontSize = 40.0f;
            tutorialText.text = "If your health bar drops to zero, you lose!";

            tutorialOrbs.SetActive(false);

        }
        else
        {
            Debug.Log("No TextMeshPro");
        }
    }

    void TutorialComplete()
    {
        tutorialText.fontSize = 40.0f;
        tutorialUI.transform.root.gameObject.SetActive(false);
        startMenuButton.SetActive(true);
    }
}
