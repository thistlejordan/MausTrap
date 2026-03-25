using Assets.Scripts.Components;
using Assets.Scripts.Enums;
using Assets.Scripts.Helpers;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterComponent : DestructableComponent
{
    // Debugging
    public Text _queueText = null;

    public Vector2 _facing;
    [SerializeField] private float _rotation;
    [SerializeField] private float moveSpeed;
    [SerializeField] protected float moveSpeedMultiplier;

    public FactionsEnum _faction;

    private List<DirectionEnum> inputs;
    private Vector2 currentDirection = Vector2.zero;

    // Model
    protected bool controlLoss = false;

    public bool ControlLoss { get => this.controlLoss; }
    public Vector2 CurrentDirection { get => this.currentDirection; }
    public Vector2 Facing { get => _facing; }
    public FactionsEnum Faction { get => _faction; }
    public List<DirectionEnum> Inputs { get => this.inputs; }
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
        if (newInputs == Vector2.zero && this.Animator.GetBool("moving")) { this.Animator.SetBool("moving", false); }
        if (newInputs != Vector2.zero && !this.Animator.GetBool("moving")) { this.Animator.SetBool("moving", true); }

        this.Rigidbody.linearVelocity = newInputs.normalized * this.moveSpeed * moveSpeedMultiplier;
    }

    public void UpdateFacing(DirectionEnum direction)
    {
        _facing = CharacterHelper.GetFacing(direction);
        _rotation = CharacterHelper.GetRotation(direction);

        this.Animator.SetFloat("input_x", _facing.x);
        this.Animator.SetFloat("input_y", _facing.y);
    }

    public void UpdateQueueText()
    {
        _queueText.text = "";
        foreach (DirectionEnum input in this.inputs) { _queueText.text += input.ToString() + "\n"; }
    }

    public override Coroutine Knockback(Vector2 vector2, float force) => this.StartCoroutine(IKnockback(vector2, force));

    private IEnumerator IKnockback(Vector2 direction, float force)
    {
        this.controlLoss = true;
        yield return base.Knockback(direction, force);
        this.controlLoss = false;
    }

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