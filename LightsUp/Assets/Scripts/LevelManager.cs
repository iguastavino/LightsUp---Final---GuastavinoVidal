using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public string levelToLoad;
    public string nextSceneName;
    public string previousSceneName;

    public Scene actualScene;

    public float waitToRespawn;

    public enum LevelState
    {
        BossFight,
        NormalLevel,
        Cinematic,
    }

    public LevelState levelState;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        actualScene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SceneManager.LoadScene(nextSceneName);
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene(previousSceneName);
        }

        if (Input.GetKeyDown(KeyCode.O) && DialogueManager.Instance.isDialogueActive)
        {
            DialogueManager.Instance.DisplayNextDialogueLine();
            if (levelState == LevelState.BossFight && BossIntro.instance.bossFight)
            {
                BossManager.instance.started = true;
            }
        }

    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(actualScene.name);
    }

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnCo());
    }

    IEnumerator RespawnCo()
    {
        yield return new WaitForSeconds(waitToRespawn - (1f / UIController.instance.fadeSpeed));

        UIController.instance.FadeToBlack();

        yield return new WaitForSeconds((1f / UIController.instance.fadeSpeed) + .2f);

        if (levelState == LevelState.BossFight)
        {
            ReloadScene();
        }

        UIController.instance.FadeFromBlack();

        PlayerMov.instance.transform.position = CheckpointController.instance.spawnPoint;

        PlayerHealth.instance.resetPlayerHealth();

    }

    public void EndLevel()
    {
        StartCoroutine(EndLevelCo());
    }

    public IEnumerator EndLevelCo()
    {
        PlayerMov.instance.stopInput = true;

        yield return new WaitForSeconds(1f);

        UIController.instance.FadeToBlack();

        yield return new WaitForSeconds(2f);

        //SceneManager.LoadScene(levelToLoad);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
