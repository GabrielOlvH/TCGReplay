using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Board : MonoBehaviour
{
    public TextureManager TextureManager;
    public Transform activeAnchor;
    public Transform discardAnchor;
    public Transform deckAnchor;
    public Transform prizeAnchor;
    public Transform benchAnchor;
    public Transform handAnchor;

    public Transform temporaryAnchor;

    public TextMeshProUGUI RoundInfoText;
    public TextMeshProUGUI Log;

    public TextMeshProUGUI PlayedInfo;
    private static int time;

    public void ShowPlayed(string text)
    {
        PlayedInfo.SetText(text);
        PlayedInfo.enabled = true;
        time = 120;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (--time < 0)
        {
            PlayedInfo.enabled = false;
        }
    }
}
