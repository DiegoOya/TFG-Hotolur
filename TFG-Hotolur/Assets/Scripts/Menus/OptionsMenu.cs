using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// Script used to manage the Options menu
/// </summary>
public class OptionsMenu : MonoBehaviour {

    // AudioMixer of the game and the Dropdown of the resolution
    // If you want to change the parameters, delete [HideInInspector] temporally
    [HideInInspector]
    public AudioMixer audioMixer;

    [HideInInspector]
    public Dropdown resolutionDropdown;
    
    // List of the possible resolutions in the PC used
    Resolution[] resolutions;

    // Initialize variables
    private void Start()
    {
        // Get all the possibles resolutions in the actual PC and add the list to resolutionDropdown
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && 
                resolutions[i].width == Screen.currentResolution.width)
                currentResolutionIndex = i;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        SetResolution(currentResolutionIndex);
    }

    // When dragged the volume slider this function is called
    public void SetVolume(float volume)
    {
        // Set the value selected with the slider to the AudioMixer
        audioMixer.SetFloat("volume", volume);
    }

    // Called when an option of the quality dropdown is selected
    public void SetQuality(int qualityIndex)
    {
        // Set the quality selected to the game
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    // Called when the toggle button of full screen is selected
    public void SetFullScreen(bool isFullScreen)
    {
        // Activate or deactivate the fullScreen option
        Screen.fullScreen = isFullScreen;
    }

    // Called when an option of the resolution dropdown is selected
    public void SetResolution(int resolutionIndex)
    {
        // Set the resolution selected to the game
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
