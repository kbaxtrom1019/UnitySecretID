using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IconLibrary : MonoBehaviour
{
    private List<Sprite> IconResources = new List<Sprite>();

    private static IconLibrary instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    public static IconLibrary GetInstance()
    {
        if (instance == null)
        {
            instance = (IconLibrary)FindObjectOfType(typeof(IconLibrary));
            if (instance == null)
            {
                // Create gameObject and add component
                instance = (new GameObject("IconLibrary")).AddComponent<IconLibrary>();
            }
        }
        return instance;
    }

    public List<Sprite> GetAllIcons()
    {
        return IconResources;
    }

    public void AddIcon(Sprite icon)
    {
        IconResources.Add(icon);
    }

    public void SortLibrary()
    {
        IconResources.Sort(SortSpriteByName);
    }

    private int SortSpriteByName(Sprite a, Sprite b)
    {
        return a.name.CompareTo(b.name);
    }
}
