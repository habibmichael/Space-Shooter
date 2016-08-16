using UnityEngine;
using System.Collections;

public class Shredder : MonoBehaviour {

    //Destoys enemy/player projectiles
	void OnTriggerEnter2D(Collider2D col )
    {
        Destroy(col.gameObject);
    }
}
