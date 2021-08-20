using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhenAttacking : MonoBehaviour
{
    // Config Params
    [SerializeField] int damage = 5;

    public int GetDamage()
    {
        return damage;
    }
}
