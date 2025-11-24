using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FolderBin : MonoBehaviour, IDropHandler
{
    public Item_23[] files;

    public String tagNamer;

    [SerializeField]private Image folderImage;

    private int i = 0;
    
    public void OnDrop(PointerEventData eventData)
    {
        GameObject drop = eventData.pointerDrag;
        DraggableItem_23 itemDropped = drop.GetComponent<DraggableItem_23>();

        UpdateFolderBin(itemDropped.Item);

    }

    public void UpdateFolderBin(Item_23 item)
    {
        if (item == null)
        {
            Debug.Log("NULLING " + name);
            return;
        }

        files[i] = item;
        i++;
    }
    
    
    public bool checkIfMatching()
    {
        for(int i = 0; i < files.Length; i++)
        {
            if (!files[i].getTagName().Equals(tagNamer) || files[i] == null)
            {
                return false;
            }
        }
        return true;
    }

    


}
