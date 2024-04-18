using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class SaveValues : MonoBehaviour
{
   public static SaveValues instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SavePrefs(float master,float music, float sfx, int toggle) 
    {
        //Guardo los valores del sonido y las partículas llamado desde el gamemanager
        PlayerPrefs.SetFloat("Master", master);
        PlayerPrefs.SetFloat("Music", music);
        PlayerPrefs.SetFloat("SFX", sfx);
        PlayerPrefs.SetInt("bool", toggle);
        PlayerPrefs.Save();
      
    }
    public void ReturnPrefs(out float master,out float music,out float sfx,out int toggle)
    {
        master=   PlayerPrefs.GetFloat("Master");
        music=    PlayerPrefs.GetFloat("Music");
        sfx=      PlayerPrefs.GetFloat("SFX");
        toggle=   PlayerPrefs.GetInt("bool");
    }
    public void Level1Completed() 
    {
        PlayerPrefs.SetInt("level1", 1);
    }
    public bool CheckLevel1() 
    {
        bool completed=false;
       
        if (PlayerPrefs.GetInt("level1") == 1) 
        {
        completed= true;
        }
        Debug.Log("a");
        return completed;
    }
}
