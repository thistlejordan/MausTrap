using Assets.Scripts.Enums;
using Assets.Scripts.Managers;
using System;
using System.Collections;
using UnityEngine;

[Obsolete("Character is obsolete. Use CharacterComponent instead.")]
public class Character : MonoBehaviour {

    [HideInInspector] public Rigidbody2D m_Rigidbody;
    [HideInInspector] public Animator m_Animator;
    [HideInInspector] public Collider2D m_Collider;

    public Vector2 m_CharacterFacingVector = new Vector2();
    public float m_CharacterRotation;
    public float m_MoveSpeed = 1f;
    public float m_MoveSpeedMultiplier = 1f;
    public bool m_IFramesActive = false;
    private bool m_ControlLoss = false;

    // ------- Stats and Other Game-Related Flags ------- //
    public int m_MaxHP = 10;
    public int m_HP = 10;
    public int m_Defense = 0;

    public int m_Money = 0;
    // -------------------------------------------------- //

    void Awake() {

        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_Collider = GetComponent<Collider2D>();
    }

    void Update() {

        m_HP = Mathf.Clamp(m_HP, 0, m_MaxHP);

        if(!GetComponent<CharacterComponent>()) {
            MoveCharacter(Vector2.zero);
        }
    }

    public void Knockback(Vector2 direction, float force) {
    

        StartCoroutine(IKnockback(direction, force));
    }

    public void MoveCharacter(Vector2 movement) {

        if(!m_ControlLoss) {
            if(movement == Vector2.zero && m_Animator.GetBool("moving")) { m_Animator.SetBool("moving", false); }
            if(movement != Vector2.zero && !m_Animator.GetBool("moving")) { m_Animator.SetBool("moving", true); }

            m_Rigidbody.velocity = movement.normalized * m_MoveSpeed * m_MoveSpeedMultiplier;
        }
    }

    public void UpdateCharacterFacing(DirectionEnum m_CharacterFacingNew) {

        //Convert from InputDirection to corresponding Vector2
        switch(m_CharacterFacingNew) {
            case DirectionEnum.DOWN: m_CharacterFacingVector = Vector2.down; m_CharacterRotation = 180f; break;
            case DirectionEnum.UP: m_CharacterFacingVector = Vector2.up; m_CharacterRotation = 0f; break;
            case DirectionEnum.LEFT: m_CharacterFacingVector = Vector2.left; m_CharacterRotation = 90f; break;
            case DirectionEnum.RIGHT: m_CharacterFacingVector = Vector2.right; m_CharacterRotation = 270f; break;
            default: Debug.Log("MovingObject.cs >> UpdateFacing(InputDirection) [InputDirection not found]: Could not update sprite facing."); break;
        }

        //Set Animator Component's parameters for blend tree input
        m_Animator.SetFloat("input_x", m_CharacterFacingVector.x); 
        m_Animator.SetFloat("input_y", m_CharacterFacingVector.y);
    }

    public virtual void TakeDamage(int damage) {

        if(!m_IFramesActive) {
            int _TrueDamage = (damage - m_Defense >= GameManager.Instance.MinimumDamage) ? damage - m_Defense : GameManager.Instance.MinimumDamage;

            InvincibilityFrames();

            m_HP = (m_HP > 0) ? m_HP - _TrueDamage : 0;

            CheckForDeath();
        }
    }

    private void CheckForDeath() {

        if(m_HP <= 0 && !m_Animator.GetBool("dead")) {
            //Play Death Animation
            StartCoroutine(IDeath());
        }
    }

    private IEnumerator IDeath() {

        Debug.Log("Is this still being called?");
        //Eventually, instead of destroying Collider, Rigidbody, and GameObject they should be set inactive so that they can no longer be interacted with

        Destroy(GetComponent<Collider2D>()); 
        Destroy(GetComponent<Rigidbody2D>());
        m_Animator.SetBool("dead", true);
        yield return new WaitForSeconds(m_Animator.GetCurrentAnimatorStateInfo(0).length);

        //Eventially, set Character Controller to inactive instead of destroying GameObject
        Destroy(gameObject);
    }

    //Invincibility frames prevent character from taking damage and occur immediately after character takes damage

    public void InvincibilityFrames() {

        if(!m_IFramesActive) { StartCoroutine(IInvincibilityFrames()); }
    }

    IEnumerator IInvincibilityFrames() {

        m_IFramesActive = true;

        float _time = 0f;
        int _i = 1;

        while(_time < GameManager.Instance.IFramesDuration) {

            GetComponent<SpriteRenderer>().enabled = _i > GameManager.Instance.m_IFrameFlickerRate ? true : false;

            if(_i < (11 - GameManager.Instance.m_IFrameFlickerRate) * 2) { ++_i; } else { _i = 1; }
            _time += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        GetComponent<SpriteRenderer>().enabled = true;

        m_IFramesActive = false;
    }

    IEnumerator IKnockback(Vector2 direction, float force) {
    //IEnumerator IKnockback(Collision2D coll) {

        //Disable Player Movement
        m_ControlLoss = true;

        //Add Force
        m_Rigidbody.velocity = direction * force;
        yield return new WaitForSeconds(0.1f);

        //Re-Enable Player Movement
        m_ControlLoss = false;
    }
}