using UnityEngine;
using System.Collections;

public class Slingshot : Item {

    public Projectile m_Bullet;
    float m_Speed;

    public override void Use() {

        Projectile _bulletInstance = Instantiate(m_Bullet, m_Owner.transform);
        _bulletInstance.GetComponent<Rigidbody2D>().linearVelocity = transform.forward * m_Speed;
    }
}
