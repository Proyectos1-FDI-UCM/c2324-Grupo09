using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        FindObjectOfType<BossIA>().BossStartAnim();

        Destroy(this.gameObject);
    }
}
