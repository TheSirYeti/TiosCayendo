using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCaller : MonoBehaviour
{
    public void DoAsyncLevelLoad(int levelID)
    {
        SceneManager.LoadSceneAsync(levelID);
    }
}
