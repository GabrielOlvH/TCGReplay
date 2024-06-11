using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class TextureManager : MonoBehaviour
{
    private static readonly Dictionary<string, Texture> TEXTURES = new();
    private static readonly Dictionary<string, List<Action<Texture>>> DOWNLOADING = new();
    public Texture BackCover;

    private void Start()
    {
    }

    public void Request(string url, Action<Texture> action)
    {
        StartCoroutine(aa(url, action));
    }

    private IEnumerator aa(string url, Action<Texture> action)
    {
        if (TEXTURES.TryGetValue(url, out var texture))
        {
            action.Invoke(texture);
            yield return null;
        }

        if (DOWNLOADING.TryGetValue(url, out var actions))
        {
            actions.Add(action);
            yield return null;
        }
        else
        {
            DOWNLOADING.Add(url, new() { action });
        }
        var request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        if(request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            var tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
            TEXTURES[url] = tex;
            if (DOWNLOADING.TryGetValue(url, out var actions1))
            {
                actions1.ForEach(a => a.Invoke(tex));
            }
        }
    }
}
