using UnityEngine;

public class ThrowableObjectsAndPlayerDestroy : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
            if (other.gameObject.tag == "ThrowableItem")
            {
            other.gameObject.SetActive(false);
            }
    }
 }

