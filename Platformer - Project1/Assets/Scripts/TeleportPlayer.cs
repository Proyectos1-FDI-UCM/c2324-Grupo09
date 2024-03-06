using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    private void Desactivar()
    {
        this.gameObject.SetActive(false);
    }
    private void TeleportPlayers()
    {
        GameManager.Instance.PlayerTeleport();

    }
    
}
