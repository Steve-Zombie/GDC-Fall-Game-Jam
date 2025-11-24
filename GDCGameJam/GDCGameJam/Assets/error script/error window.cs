using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class errorwindow : MonoBehaviour
{
    public GameObject mother;
    private GameObject[] clones;
    public TextMeshProUGUI remainingText; 
    public int popUps = 8;
    public Button exitButton;
    public float startTime = 5f;
    public Transform myCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        clones = new GameObject[7];
        for(int i = 0; i < 7; i++){
            clones[i] = Instantiate(mother);
            clones[i].transform.SetParent(myCanvas.transform);
            clones[i].transform.position = new Vector3(Random.Range(-70f,700f),Random.Range(-60f,370f),0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (popUps > 0){
            remainingText.text = popUps.ToString() + " popups left";
            
        }
        else{
            remainingText.text = "Congradulations! All popups have been removed.";
            if (startTime > 0){
                if (startTime == 5){
                    Debug.Log("Time has started");
                }
                startTime -= Time.deltaTime;
            }
            else{
                startTime = 0;
                Debug.Log("Time has ended");
                //SceneManager.LoadScene();
            }
        }
    }

    public void removePopup(){
        popUps--;
        Debug.Log("Button has been clicked.");
        //this.gameObject.SetActive(false);
    }

}
