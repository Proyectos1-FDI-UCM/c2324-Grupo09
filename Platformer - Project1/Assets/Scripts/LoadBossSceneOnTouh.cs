using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadBossSceneOnTouh : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SaveValues.instance.Level1Completed();
        SceneManager.LoadSceneAsync("Nivel_Boss",LoadSceneMode.Single);
    }
}
