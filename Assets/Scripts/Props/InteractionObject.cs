using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Базовый абстрактный класс для объектов, с которыми может взаимодействовать пользователь
/// </summary>

public abstract class InteractionObject : MonoBehaviour
{
    public string interactionHelpText;
    public bool enableInteraction = true;

    public virtual void Interact() { }
}
