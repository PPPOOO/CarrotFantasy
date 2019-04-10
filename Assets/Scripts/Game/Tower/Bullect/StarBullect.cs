using UnityEngine;
using System.Collections;

public class StarBullect : MonoBehaviour
{

    public int attackValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.activeSelf)
        {
            return;
        }
        if (collision.tag == "Monster" || collision.tag == "Item")
        {
            collision.SendMessage("TakeDamage", attackValue);
        }
    }
}
