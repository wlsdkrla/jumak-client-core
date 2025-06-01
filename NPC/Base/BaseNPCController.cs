using UnityEngine;
using System;

namespace NPCSystem
{
    public abstract class BaseNPCController : MonoBehaviour, INPCAction
    {
        [Header("References")]
        [SerializeField] protected NPCConfig config;
        [SerializeField] protected Animator animator;
        [SerializeField] protected Rigidbody2D rb;

        protected NPCState currentState = NPCState.Idle;
        protected Transform target;

        protected virtual void Start()
        {
            if (animator == null) animator = GetComponent<Animator>();
            if (rb == null) rb = GetComponent<Rigidbody2D>();
            ApplyConfig();
            ChangeState(NPCState.Idle);
        }

        protected virtual void Update()
        {
            switch (currentState)
            {
                case NPCState.Idle:
                    PlayAnimation(config.idleAnimName);
                    break;

                case NPCState.Move:
                    MoveToTarget();
                    break;

                case NPCState.Action:
                    PerformAction();
                    break;

                case NPCState.Sit:
                    SitDown();
                    break;

                case NPCState.Exit:
                    ExitScene();
                    break;
            }
        }

        public virtual void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        public virtual void ChangeState(NPCState newState)
        {
            currentState = newState;
        }

        protected virtual void MoveToTarget()
        {
            if (target == null) return;

            Vector2 direction = (target.position - transform.position).normalized;
            rb.velocity = direction * config.moveSpeed;

            transform.localScale = new Vector3(direction.x < 0 ? -1 : 1, 1, 1);
            PlayAnimation(config.walkAnimName);

            float distance = Vector2.Distance(transform.position, target.position);
            if (distance < config.arrivalThreshold)
            {
                rb.velocity = Vector2.zero;
                OnArrival();
            }
        }

        protected virtual void OnArrival()
        {
            ChangeState(NPCState.Sit);
        }

        protected virtual void PerformAction() { }

        protected virtual void SitDown()
        {
            PlayAnimation(config.sitAnimName);
        }

        protected virtual void ExitScene()
        {
            gameObject.SetActive(false);
        }

        protected void PlayAnimation(string animName)
        {
            if (animator != null && !string.IsNullOrEmpty(animName))
                animator.Play(animName);
        }

        protected virtual void ApplyConfig()
        {
            if (config == null)
            {
                Debug.LogWarning($"{name}에 NPCConfig가 연결되어 있지 않습니다.");
            }
        }
    }
}
