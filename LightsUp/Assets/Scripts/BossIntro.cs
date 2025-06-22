using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIntro : MonoBehaviour
{
    public GameObject door;
    public CinemachineVirtualCamera CameraStart;
    public CinemachineVirtualCamera CameraFight;
    public static BossIntro instance;


    public bool bossFight = false;

    public GameObject gulaHealthBar;

    void Start()
    {
        instance = this;
        CameraManager.SwitchCamera(CameraStart);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && DialogueManager.Instance.isDialogueActive && bossFight)
        {
            DialogueManager.Instance.DisplayNextDialogueLine();
            if (LevelManager.instance.levelState == LevelManager.LevelState.BossFight)
            {
                BossManager.instance.started = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            CameraManager.SwitchCamera(CameraFight);

            door.gameObject.GetComponent<Door>().SetIsOpening(false);

            bossFight = true;

            gulaHealthBar.SetActive(true);
        }
    }
}
