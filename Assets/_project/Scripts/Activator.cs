using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Rock")
        {
            other.gameObject.SetActive(false);
        }
    }
}
