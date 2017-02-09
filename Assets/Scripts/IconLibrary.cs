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
}
