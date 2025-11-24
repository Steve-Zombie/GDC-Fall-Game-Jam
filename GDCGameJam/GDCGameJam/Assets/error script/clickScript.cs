using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class clickScript : MonoBehaviour, IPointerClickHandler
{
    public errorwindow errorwindowReference;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //int remainingPops = errorwindowReference.popUps;
        //Debug.Log(remainingPops);
    }
    public void OnPointerClick(PointerEventData eventData){
        //Debug.Log("This shit works");
        if (errorwindowReference != null){
            errorwindowReference.removePopup();
        }
        else{
            
        }
        this.gameObject.SetActive(false);
    }

    
}
