using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeSignText : MonoBehaviour
{

    private const string WJhint = @"On air: 
{Jump}";

    //a esto se le pasa en el input manager:  ///////////////////////m_PlayerInput.actions["pickup"].GetBindingDisplayString());
    private void UpdateText(string str)
    {
        GetComponent<TextMeshPro>().text = WJhint.Replace("{Jump}", str);
    }

    private void Start()
    {
        //UpdateText("s");
    }
}
