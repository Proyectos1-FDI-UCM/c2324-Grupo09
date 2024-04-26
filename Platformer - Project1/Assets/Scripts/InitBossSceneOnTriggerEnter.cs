using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class InitBossSceneOnTriggerEnter : MonoBehaviour
{
    [SerializeField]
    GameObject[] collidersToErase;
    [SerializeField]
    GameObject tileMapToActivate;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        for (int i = 0; i < collidersToErase.Length; i++) Destroy(collidersToErase[i]);

        tileMapToActivate.SetActive(true);
        try
        {
            AudioManager.Instance.InitializeMusic(FMODEvents.Instance.BossMusic);

        }
        catch
        {
            Debug.Log("Añadir Audio Uwu");
        }

        FindObjectOfType<BossIA>().BossStartAnim();

        Destroy(this.gameObject);
    }
}
