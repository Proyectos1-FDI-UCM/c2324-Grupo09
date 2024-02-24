using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunWallComponent : MonoBehaviour
{
    private Vector2 _holdSpeed;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {

        RefactoredCharacterController player = collision.GetComponent<RefactoredCharacterController>();
        if (player != null)
        {
            player.WallRunToggle(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        RefactoredCharacterController player = collision.GetComponent<RefactoredCharacterController>();
        if (player != null)
        {
            player.WallRunToggle(false);
        }
    }
}
