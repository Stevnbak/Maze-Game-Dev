using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionItem : MonoBehaviour, IInteractable
{
    public float time { get; set; }
    public float setTime;
    void Start()
    {
        time = setTime;
    }

    public void interact()
    {
        AudioSource.PlayClipAtPoint(GetComponent<AudioSource>().clip, transform.position);
        Destroy(gameObject);
    }
}
