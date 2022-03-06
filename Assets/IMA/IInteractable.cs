using UnityEngine;
public interface IInteractable
{
    void interact();
    float time { get; set; }

    bool hovering { get; set; }
}
