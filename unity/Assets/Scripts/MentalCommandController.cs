using UnityEngine;

/// <summary>
/// Gets the current mental command and handles what to do with it
/// </summary>
public class MentalCommandController : MonoBehaviour
{
    private MentalCommands mentalCommandsManager;
    public float speed = 2.0f; // Movement speed

    void Start()
    {
        GameObject mentalCommands = GameObject.FindGameObjectWithTag("MentalCommands");

        mentalCommandsManager = mentalCommands.GetComponent<MentalCommands>();

        if (mentalCommandsManager == null)
        {
            Debug.LogError("mentalCommandsManager script not found on the GameObject.");
        }
    }

    void Update()
    {
        if (mentalCommandsManager == null) return;

        // Get the mental command
        string command = mentalCommandsManager.GetMentalCommand();

        if (command == null)
            return;

        Vector3 direction = Vector3.zero;

        // Determine the direction based on the command
        switch (command)
        {
            case "left":
                direction = Vector3.left;
                break;
            case "right":
                direction = Vector3.right;
                break;
            case "lift":
                direction = Vector3.up;
                break;
            case "drop":
                direction = Vector3.down;
                break;
            case "neutral":
                // Stop the GameObject, so we don't set the direction
                break;
            default:
                Debug.LogWarning("Unknown command: " + command);
                break;
        }

        // Move the GameObject in the determined direction
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
