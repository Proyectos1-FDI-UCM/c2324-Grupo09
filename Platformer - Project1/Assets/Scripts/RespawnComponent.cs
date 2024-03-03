using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class RespawnComponent : MonoBehaviour
{
    private void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        //Mudas la piel, y con ella, otra parte de tí.
    }
}
