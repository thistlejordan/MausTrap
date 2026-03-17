using Assets.Scripts.Enums;
using Assets.Scripts.Helpers;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterComponent : MonoBehaviour
{
    // Debugging
    public Text _queueText = null;

    // View
    [SerializeField] protected Animator _animator;
    [SerializeField] protected Collider2D _collider;
    public Rigidbody2D _rigidbody;
    public SpriteRenderer _spriteRenderer;

    public Vector2 _facing;
    [SerializeField] private float _rotation;
    [SerializeField] private float moveSpeed;
    [SerializeField] protected float moveSpeedMultiplier;

    public FactionsEnum _faction;
    public bool _invincible;
    public int _maxHealth;
    public int _health;
    public int _defense;


    private List<DirectionEnum> inputs;
    private Vector2 currentDirection = Vector2.zero;

    // Model
    protected bool controlLoss = false;

    public Collider2D Collider { get => _collider; }
    public bool ControlLoss { get => this.controlLoss; }
    public Vector2 CurrentDirection { get => this.currentDirection; }
    public Vector2 Facing { get => _facing; }
    public FactionsEnum Faction { get => _faction; }
    public List<DirectionEnum> Inputs { get => this.inputs; }
    public bool Invincible { get => _invincible; }
    public float MoveSpeedMultiplier { get => this.moveSpeedMultiplier; }
    public float Rotation { get => _rotation; }

    public void InputMove(Vector2 newInputs)
    {
        this.inputs ??= new List<DirectionEnum>();

        if (newInputs.x != this.currentDirection.x)
        {
            if (this.currentDirection.x < 0) { this.inputs.Remove(DirectionEnum.LEFT); }
            if (this.currentDirection.x > 0) { this.inputs.Remove(DirectionEnum.RIGHT); }

            this.currentDirection.x = newInputs.x;

            if (this.currentDirection.x < 0) { this.inputs.Add(DirectionEnum.LEFT); }
            if (this.currentDirection.x > 0) { this.inputs.Add(DirectionEnum.RIGHT); }
        }

        if (newInputs.y != this.currentDirection.y)
        {
            if (this.currentDirection.y < 0) { this.inputs.Remove(DirectionEnum.DOWN); }
            if (this.currentDirection.y > 0) { this.inputs.Remove(DirectionEnum.UP); }

            this.currentDirection.y = newInputs.y;

            if (this.currentDirection.y < 0) { this.inputs.Add(DirectionEnum.DOWN); }
            if (this.currentDirection.y > 0) { this.inputs.Add(DirectionEnum.UP); }
        }

        // Debug
        if (_queueText != null) UpdateQueueText();

        if (!this.controlLoss)
        {
            if (this.inputs.Count > 0) UpdateFacing(this.inputs[0]);
            Move(newInputs);
        }
    }

    public void Move(Vector2 newInputs) => Move(newInputs, this.moveSpeedMultiplier);

    public void Move(Vector2 newInputs, float moveSpeedMultiplier)
    {
        if (newInputs == Vector2.zero && _animator.GetBool("moving")) { _animator.SetBool("moving", false); }
        if (newInputs != Vector2.zero && !_animator.GetBool("moving")) { _animator.SetBool("moving", true); }

        _rigidbody.linearVelocity = newInputs.normalized * this.moveSpeed * moveSpeedMultiplier;
    }

    public virtual void RestoreHealth(int value) => _health += (_health + value > _maxHealth) ? 0 : value;

    private int CalculateTrueDamage(int damage) => (damage - _defense > 0) ? damage - _defense : 1;

    private void TakeDamage(int trueDamage) => _health = (_health - trueDamage >= 0) ? _health - trueDamage : 0;

    private void CheckForDeath()
    {
        if (_health <= 0 && !_animator.GetBool("dead"))
        {
            Debug.Log($"Health: {_health}");
            _animator.SetBool("dead", true);
            Die();
        }
    }

    public virtual void ReceiveAttack(AttackModel attack)
    {
        if (!_invincible)
        {
            InvincibilityFrames();
            TakeDamage(CalculateTrueDamage(attack.Damage));
            Knockback(attack.KnockbackDirection, attack.KnockbackForce);
            CheckForDeath();
        }
    }

    public void UpdateFacing(DirectionEnum direction)
    {
        _facing = CharacterHelper.GetFacing(direction);
        _rotation = CharacterHelper.GetRotation(direction);

        _animator.SetFloat("input_x", _facing.x);
        _animator.SetFloat("input_y", _facing.y);
    }

    public void UpdateQueueText()
    {
        _queueText.text = "";
        foreach (DirectionEnum input in this.inputs) { _queueText.text += input.ToString() + "\n"; }
    }

    private void InvincibilityFrames()
    {
        _invincible = true;
        StartCoroutine(IInvincibilityFrames());
    }

    private IEnumerator IInvincibilityFrames()
    {
        float _time = 0f;
        int _i = 1;

        while (_time < GameManager.Instance.IFramesDuration)
        {
            _spriteRenderer.enabled = _i > GameManager.Instance.m_IFrameFlickerRate ? true : false;
            if (_i < (11 - GameManager.Instance.m_IFrameFlickerRate) * 2) { ++_i; } else { _i = 1; }
            _time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _spriteRenderer.enabled = true;
        _invincible = false;
    }

    public void Knockback(Vector2 direction, float force) => StartCoroutine(IKnockback(direction, force));

    private IEnumerator IKnockback(Vector2 direction, float force)
    {
        this.controlLoss = true;
        _rigidbody.linearVelocity = direction * force;
        yield return new WaitForSeconds(0.1f);
        this.controlLoss = false;
    }

    private void Die() => StartCoroutine(IDie());

    private IEnumerator IDie()
    {
        _animator.SetBool("dead", true);
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }

    public virtual void SetControlLoss(float duration) => StartCoroutine(IControlLoss(duration));

    public virtual void SetControlLoss(bool value) => this.controlLoss = value;

    public IEnumerator IControlLoss(float duration)
    {
        this.controlLoss = true;
        yield return new WaitForSeconds(duration);
        this.controlLoss = false;
    }

    public IEnumerator IControlLoss(Coroutine coroutine)
    {
        this.controlLoss = true;
        yield return coroutine;
        this.controlLoss = false;
    }

    public void SetMoveSpeedMultiplier(float value) => this.moveSpeedMultiplier = value;
}