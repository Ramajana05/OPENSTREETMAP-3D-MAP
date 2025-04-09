using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Material skyboxMaterial;
    public Light sun;
    public float currentTime = 12f; // Default time is noon (12:00 PM)
    public bool useSlider = false; // Use a slider to control time in Unity Editor

    void Update()
    {
        if (useSlider)
        {
            // Update current time using a slider
            // This allows you to test the day/night cycle in the Unity Editor
            currentTime = Mathf.Clamp(currentTime, 0f, 24f); // Ensure time stays within 24-hour range
        }
        else
        {
            // Use real-world time
            currentTime = CalculateTimeOfDay();
        }

        // Adjust skybox color and sun based on time of day
        AdjustSkyAndLight(currentTime);
    }

    float CalculateTimeOfDay()
    {
        // Example: Simulate a 24-hour day cycle where timeOfDay ranges from 0 to 24
        // In a real-world scenario, you might use System.DateTime or a custom time system
        float timeOfDay = Time.time % 24f;
        return timeOfDay;
    }

    void AdjustSkyAndLight(float timeOfDay)
    {
        float sunRotation;
        if (timeOfDay >= 6f && timeOfDay < 18f)
        {
            // It's day time
            RenderSettings.skybox.SetColor("_Tint", Color.Lerp(Color.black, Color.white, Mathf.Clamp01((timeOfDay - 6f) / 12f)));
            sun.intensity = Mathf.Lerp(0.2f, 1.5f, Mathf.Clamp01((timeOfDay - 6f) / 12f));
            sunRotation = Mathf.Lerp(0f, 180f, (timeOfDay - 6f) / 12f); // Sun rises in the east (0°) and sets in the west (180°)
        }
        else
        {
            // It's night time
            RenderSettings.skybox.SetColor("_Tint", Color.Lerp(Color.white, Color.black, Mathf.Clamp01((timeOfDay - 18f) / 6f)));
            sun.intensity = Mathf.Lerp(0.3f, 0f, Mathf.Clamp01((timeOfDay - 18f) / 6f));
            sunRotation = Mathf.Lerp(180f, 360f, (timeOfDay - 18f) / 6f); // Moon rises in the east (180°) and sets in the west (360°)
        }
        sun.transform.rotation = Quaternion.Euler(sunRotation - 90f, 170f, 0f); // Adjust this to match the correct directional light rotation
    }
}
