using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCaller : MonoBehaviour
{
    public void DoAsyncLevelLoad(int levelID)
    {
        EventManager.ResetEventDictionary();
        SceneManager.LoadSceneAsync(levelID);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
