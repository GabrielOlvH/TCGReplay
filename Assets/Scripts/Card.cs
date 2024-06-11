using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using simulator;
using UnityEngine;
using UnityEngine.Networking;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Card : MonoBehaviour
{
    
    public Board board;
    public GameObject front;
    public GameObject back;

    public bool facingUp;
    public List<Card> attachedCards = new();

    public CardMatcher Matcher;
    public string Name;
    private float movingProgress = 0;
    private Vector3 target;
    private Quaternion targetRotation;
    public bool isAlreadyRevealed = false; 
    
    void Start()
    {
        var renderer = front.GetComponent<Renderer>();
        renderer.material.mainTexture = board.TextureManager.BackCover;
        back.GetComponent<Renderer>().material.mainTexture = board.TextureManager.BackCover;
    }

    public bool IsThisOrAttached(string name)
    {
        bool isEvolved = false;
        foreach (var attachedCard in attachedCards)
        {
            if (attachedCard.Matcher.IsPokemon())
            {
                
                Debug.Log("iS ALREADY EVOLVED!" + name + " evolved into " + attachedCard.Matcher.DisplayName);
                isEvolved = true;
            }
        }
        if (!isEvolved && Name == name)
        {
            return true;
        }
        return attachedCards.Any(c => c.Name == name);
    }

    public bool CanAttach(string name)
    {
        return attachedCards.Any(c => c.Matcher.Matches.Any(c1 => c1.EvolvesFrom == name));
    }

    public void SetCardMatcher(CardMatcher matcher)
    {
        if (Matcher != null && matcher != null)
        {
            Debug.Log("Switched " + Matcher.DisplayName + " to " + matcher.DisplayName);
        }
        Matcher = matcher;
        if (Matcher != null)
        {
            TCGApi.FindCardByName(Matcher.DisplayName);
            set = false;

            if (front != null) 
                front.GetComponent<Renderer>().material.mainTexture = board.TextureManager.BackCover;
            Name = Matcher.DisplayName;
        }
        else
        {
            if (front != null) 
                front.GetComponent<Renderer>().material.mainTexture = board.TextureManager.BackCover;
            set = false;
            Name = "";
        }
    }

    public void MoveTo(Vector3 target, Quaternion targetRotation)
    {
        if (target != transform.position)
        {
            this.target = target;
            this.targetRotation = targetRotation;
        }
    }

    private bool set = false;
    void Update()
    {
        
        if (Matcher != null && !set && TCGApi.Cache.TryGetValue(Matcher.DisplayName, out var matches) && matches.Count > 0)
        {
            set = true;
            var renderer = front.GetComponent<Renderer>();
            
            
            board.TextureManager.Request(Matcher.SelectDisplay(), (tex) =>
            {
                renderer.material.mainTexture = tex;
            });
        }
        foreach (var attachedCard in attachedCards)
        {
            attachedCard.Update();

            if (attachedCard.Matcher.Matches.Any(m => m.Supertype == "Pokémon"))
            {
                front.GetComponent<Renderer>().enabled = false;
                back.GetComponent<Renderer>().enabled = false;
            }
            else
            {
                attachedCard.transform.localScale = Vector3.one * .5f + Vector3.up * 0.15f;
            }

            attachedCard.target = transform.position + Vector3.up * 0.5f;
        }
        var targetZ = facingUp ? 0 : 180;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.LerpAngle(transform.rotation.eulerAngles.z, targetZ, Time.deltaTime*8));

        if (target != Vector3.zero)
        {
            movingProgress += Time.deltaTime * 0.75f;
            var thisTransform = transform;
            var pos = thisTransform.position;
            var rot = thisTransform.rotation;
        
            var targetPos = target;
            var targetRot = targetRotation.eulerAngles.x;
        
            if (movingProgress < .7 && !isAlreadyRevealed)
            {
                targetPos = board.temporaryAnchor.position;
                targetRot = board.temporaryAnchor.rotation.eulerAngles.x;
            }
        
        
            var x = Mathf.Lerp(pos.x, targetPos.x, Time.deltaTime*24);
            var y = Mathf.Lerp(pos.y, targetPos.y, Time.deltaTime*24);
            var z = Mathf.Lerp(pos.z, targetPos.z, Time.deltaTime*24);
            thisTransform.position = new Vector3(x, y, z);
            var xRot = Mathf.LerpAngle(rot.eulerAngles.x, targetRot, Time.deltaTime*8);
            thisTransform.rotation = Quaternion.Euler(xRot, rot.eulerAngles.y, rot.eulerAngles.z);

            if (movingProgress > (isAlreadyRevealed ? 1 : 2))
            {
                isAlreadyRevealed = true;
                movingProgress = 0;
                target = Vector3.zero;
            }
        }
    }
}
