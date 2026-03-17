using System.Collections;
using System.Linq;
using UnityEngine;

public class Bomb : Item
{
    public float m_DurationBeforeFlash, m_DurationFlash, m_DurationExplosion = 1f;
    public float _knockbackForce = 30.0f;

    public override void Use() => StartDetonation();

    private void StartDetonation() => StartCoroutine(IBombAnimation());

    private void ProcessDetonation()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, _range);

        foreach (var collider in colliders.Where(collider => collider.GetComponent<Character>()))
        {
            var character = collider.GetComponent<Character>();
            float rangeMultiplier = (_range - Vector2.Distance(collider.transform.position, transform.position)) / _range;
            Vector2 direction = (character.transform.position - transform.position).normalized;

            Debug.Log("Bomb hit: " + character.name);
            character.TakeDamage((int)(rangeMultiplier * _damage));
            character.Knockback(direction, rangeMultiplier * _knockbackForce);
        }
    }

    IEnumerator IBombAnimation()
    {
        ShowSprite();
        yield return new WaitForSeconds(m_DurationBeforeFlash);
        GetComponent<Animator>().SetInteger("state", 1); //Flashing Animation
        yield return new WaitForSeconds(m_DurationFlash);
        GetComponent<Animator>().SetInteger("state", 2); //Boom! Animation
        ProcessDetonation();
        yield return new WaitForSeconds(m_DurationExplosion);

        Destroy(gameObject); //Remove object after it explodes
    }
}
