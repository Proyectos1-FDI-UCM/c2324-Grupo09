using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnComponent : MonoBehaviour
{
    private Transform _transform;
    private void Start()
    {
        _transform = transform;
        GetComponent<SpriteRenderer>().enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        RefactoredCharacterController character = collision.GetComponent<RefactoredCharacterController>();
        if (character != null)
        {
            character.AssignSpawnPoint(_transform);
            Debug.Log("Respawn set at" + _transform.position);
        }
    }
}
