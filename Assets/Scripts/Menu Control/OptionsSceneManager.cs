using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsSceneManager : MonoBehaviour
{
    public void GoBackToTitleScene()
    {
        Debug.Log("Going back to title scene...");
        SceneManager.LoadScene("TitleScene");
        Vibrator.Vibrate(Vibration.SHORT);  // 100 ms
    }

    public void ChangeAudioVolume(float newVolume)
    {
        Debug.Log($"Setting volume: {newVolume}");
        AudioListener.volume = newVolume;
        PlayerPrefs.SetFloat("GlobalGameVolume", newVolume);
        Vibrator.Vibrate(Vibration.SHORT);  // 100 ms
    }
    public void ChangeNoBots()
    {
        int numBots = (int)GameObject.Find("numBots").GetComponent<Slider>().value;
        Debug.Log(numBots);
        PlayerPrefs.SetInt("GlobalBotNum", numBots);
        GameObject.Find("BotNum").GetComponent<saveBotNum>().setBotNum(numBots);
        Vibrator.Vibrate(Vibration.SHORT);  // 100 ms

    }
}
