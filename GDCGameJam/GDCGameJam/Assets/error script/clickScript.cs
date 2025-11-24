using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class clickScript : MonoBehaviour, IPointerClickHandler
{
    //Reference to errorwindow script
    public errorwindow errorwindowReference;


    //Calls reference to increment counter and close popups
    public void OnPointerClick(PointerEventData eventData){
        if (errorwindowReference != null){
            errorwindowReference.removePopup();
        }
        this.gameObject.SetActive(false);
    }

    
}
