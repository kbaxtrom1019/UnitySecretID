using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class IconBundleLoader : MonoBehaviour
{

    public List<string> OnDiskBundles = new List<string>();
    public IEnumerator LoadBundle()
    {
        IconLibrary library = IconLibrary.GetInstance();
        foreach(string bundleName in OnDiskBundles)
        {
            string path = Path.Combine(Application.streamingAssetsPath, bundleName);
            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path);
            yield return request;

            AssetBundle bundle = request.assetBundle;
            if (bundle != null)
            {
                AssetBundleRequest assetLoadRequest = bundle.LoadAllAssetsAsync<Sprite>();
                yield return assetLoadRequest;

                Object[] loadedAssets = assetLoadRequest.allAssets;
                if (loadedAssets != null)
                {
                    for(int index = 0; index < loadedAssets.Length; ++index)
                    {
                        Object asset = loadedAssets[index];
                        Debug.Log("LoadedIconAsync:" + asset);
                        library.AddIcon(asset as Sprite);
                    }
                    
                }
            } 
        }
        library.SortLibrary();


    }
    void Start()
    {


        StartCoroutine(LoadBundle());
    }
}
