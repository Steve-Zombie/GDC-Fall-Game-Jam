using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "Docs/Item")]
public class Item_23 : ScriptableObject
{
    public Sprite sprite;
    public string tagName;

    public string getTagName()
    {
        return tagName;
    }

    
    

}
