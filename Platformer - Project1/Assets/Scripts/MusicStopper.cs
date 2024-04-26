using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicStopper : MonoBehaviour
{
    private void Start()
    {
        try
        {

            AudioManager.Instance.InitializeMusic(FMODEvents.Instance.Music);
        }
        catch
        {
            Debug.Log("Mete audiomanager porfaaa");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<RefactoredCharacterController>() != null)
        {
            try
            {

                Debug.Log("ME cago en tus muertos");
                AudioManager.Instance.StopMusic(FMODEvents.Instance.Music);

            }
            catch
            {
                Debug.Log("Añadir Audio Uwu");
            }

        }

    }
}

