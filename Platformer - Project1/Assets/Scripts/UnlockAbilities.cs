using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnlockAbilities : MonoBehaviour
{
    RefactoredCharacterController _characterController;
    InputManager _inputManager;
    [SerializeField]
    int value;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _characterController=collision.GetComponent<RefactoredCharacterController>();
        if(_characterController != null) 
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.CollectAbility, this.transform.position);
            _characterController.Unlock(value);
            _characterController.gameObject.GetComponent<InputManager>().SpawnUnlockMenu(value);
            Destroy(this.gameObject);
        }
    }
}
