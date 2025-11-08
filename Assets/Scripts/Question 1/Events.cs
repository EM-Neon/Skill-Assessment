using UnityEngine;
using System;

public class Events : MonoBehaviour
{
    public static event Action OnBoxHit;
    public static event Action OnEnemyDeath;
    public static event Action OnLevelComplete;
}
