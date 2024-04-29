using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior3 : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Destroy(this.transform.parent.gameObject);

            Debug.Log("Spear, one weapon needed to defeat spongebob");
        }
    }



}
