using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float m_ProjectileSpeed = 1;

    public void OnCollisionEnter2D(Collision2D coll) {

        Destroy(gameObject);
    }

	void Awake () {

        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 1).normalized * m_ProjectileSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		

	}
}
