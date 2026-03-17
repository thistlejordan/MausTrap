using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AnimHelper : MonoBehaviour {

    private Animator m_Anim;
    private IEnumerator m_IAnimation; //IEnumerator object used in Coroutine
    private bool m_AlreadyShowingAnimation;
    private bool m_AnimationInProgress;
    public bool AnimationInProgressFlag { get { return m_AnimationInProgress; } } //Intentionally read-only
    private bool m_AnimationAcknowledged = true;
    public bool AnimationActiveFlag { get { return !m_AnimationAcknowledged; } } //Intentionally read-only

    public void ProcessAnimationInput() { // Both interrupts and acknowledges current Animation

        if(m_AnimationInProgress) {

            InterruptAnimation();
        }

        if(!m_AnimationAcknowledged) {

            AcknowledgeAnimation();
        }
    }

    public void ProcessAnimationInputLong() { // First interrupts Animation, then acknowledges

        if(m_AnimationInProgress) {

            InterruptAnimation();

        } else {

            AcknowledgeAnimation();
        }
    }

    public void ProcessAnimationInputUnskippable() { // Does not allow acknowledge until Animation is complete

        if(!m_AnimationInProgress) {

            AcknowledgeAnimation();
        }
    }

    public void UpdateAnimation(Animator anim) {

        SetAnimatorComponent(anim);
        ShowSprite();

        if(!m_AlreadyShowingAnimation) {

            m_AlreadyShowingAnimation = true;
            PlayAnimation();
        }
    }

    private void PlayAnimation() {

        GetComponent<Player>().m_Acknowledge = false;
        m_AnimationAcknowledged = false;
        SetIAnimation();
        StartAnimationCoroutine();
    }

    private void AcknowledgeAnimation() {

        m_AlreadyShowingAnimation = false;
        m_AnimationAcknowledged = true;
        HideSprite();
        ClearAnimatorComponent();
    }

    private void ShowSprite() {

        m_Anim.GetComponent<SpriteRenderer>().enabled = true;
    }

    private void HideSprite() {

        m_Anim.GetComponent<SpriteRenderer>().enabled = false;
        m_Anim.SetInteger("state", 0);
    }

    private void InterruptAnimation() {

        StopAnimationCoroutine();
        m_AnimationInProgress = false;
        HideSprite();
    }

    private void SetIAnimation() {

        m_IAnimation = IUpdateAnimation(m_Anim);
    }

    private void StartAnimationCoroutine() {

        StartCoroutine(m_IAnimation);
    }

    private void StopAnimationCoroutine() {

        StopCoroutine(m_IAnimation);
    }

    private void ClearAnimatorComponent() {

        m_Anim = null;
    }

    private void SetAnimatorComponent(Animator anim) {

        m_Anim = anim;
    }

    IEnumerator IUpdateAnimation(Animator anim) {

        m_AnimationInProgress = true;

        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;

        for(int i = 1; i < clips.Length; i++) {
            anim.SetInteger("state", i);
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        }

        m_AnimationInProgress = false;

        if(!anim.GetBool("require_acknowledge")) {
            AcknowledgeAnimation();
            GetComponent<Player>().m_Acknowledge = true;
        }

        while(!m_AnimationAcknowledged) { yield return null; }
    }
}
