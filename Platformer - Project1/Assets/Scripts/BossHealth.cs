using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{//en este script cambio los valores del slider que muestra la vida del bos para que se vea que sufre daño

    [SerializeField]
    Slider _bossHealth;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void ChangeHealth(int health)
    {
        _bossHealth.value = health;
    }
}
