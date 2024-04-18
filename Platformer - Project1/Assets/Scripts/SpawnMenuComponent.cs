using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMenuComponent : MonoBehaviour
{
    [SerializeField]
    GameObject [] _abilitiesExplanation;

    public void SetMenu(int i)
    {
        _abilitiesExplanation[i].SetActive(true);
    }
}
