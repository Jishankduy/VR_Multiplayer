using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public TextMeshProUGUI time;
    public float timer;

    public int player;

    public bool gameComplete;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameComplete != true)
        {
            timer += Time.deltaTime;
            int minutes = Mathf.FloorToInt(timer / 60);
            int secends = Mathf.FloorToInt(timer % 60);

            time.text = string.Format("{0:00}:{1:00}", minutes, secends);
        }
        if (player == 2)
        {
            gameComplete = true;
        }
    }

    public void EndpointTriggerEnter()
    {
        player++;
    }
    
    public void EndpointTriggerExit()
    {
        player--;
    }
}
