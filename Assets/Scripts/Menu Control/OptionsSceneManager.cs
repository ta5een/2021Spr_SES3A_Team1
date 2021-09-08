using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsSceneManager : MonoBehaviour
{
    public void GoBackToTitleScene()
    {
        Debug.Log("Going back to title scene...");
        SceneManager.LoadScene("TitleScene");
    }

    public void ChangeAudioVolume(float newVolume)
    {
        Debug.Log($"Setting volume: {newVolume}");
        PlayerPrefs.SetFloat("GlobalGameVolume", newVolume);
        AudioListener.volume = newVolume;
    }
}
