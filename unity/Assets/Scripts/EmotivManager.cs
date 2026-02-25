using UnityEngine;

/// <summary>
/// Manages the Emotiv device and what headset is connected
/// </summary>
public class EmotivManager : MonoBehaviour
{
    public static EmotivManager Instance { get; private set; }

    [Header("Emotiv Settings")]
    public string clientId = "YIigrmQjG2DGrTt4NHa48ETSRVOplAPHfyygSm7E";
    public string clientSecret = "XfaaHTZqdRJVxlKZcqQZZ9HYdT9OtVtO4SV58Vws5Lh7TTVZrB9DIqSGUHtfRGVvOHcp87ROTkhQo9C0PWuPQWydOqb7A5f8GIynNXzzwkfpiiENHT4ywxcqXwL8QWNt";
    public string appName = "MSI3";
    public string appVersion = "3.3.0";

    [Header("Profile Information")]
    [HideInInspector] public string[] headsetNames;
    [SerializeField] public int selectedHeadsetIndex = 0;
    [HideInInspector] public string headsetId;
    public string profileName = "wanyea";

    private void Start()
    {
        headsetNames = new string[]
        {
            "INSIGHT2-A068A3B7",
            "EPOCX-E50208B2",
            "EPOCX-FF138AB1",
            "EPOCX-16EF2451",
            "EPOCX-BFDB334A",
            "INSIGHT2-A3D20368",
            "INSIGHT2-DC3B479A",
            "INSIGHT2-EC3D8746",
            "INSIGHT2-A3D2036F"
            
        };
    }

    private void Update()
    {
        headsetId = headsetNames[selectedHeadsetIndex];
    }
}

