using UnityEngine;
using System.Collections;

namespace NPCSystem
{
    public class ServantNPCController : BaseNPCController
    {
        private Transform kitchenZone;
        private Transform[] customerSeats;

        private int currentIndex = 0;
        private bool isServing = false;
        private GameObject carriedFood;

        public void Initialize(Transform kitchen, Transform[] seats, NPCConfig npcConfig)
        {
            config = npcConfig;
            kitchenZone = kitchen;
            customerSeats = seats ?? new Transform[0];
            currentIndex = 0;

            ApplyConfig(); 

            SetTarget(kitchenZone);
            ChangeState(NPCState.Move);
        }

        protected override void OnArrival()
        {
            if (currentState == NPCState.Move)
                ChangeState(NPCState.Action);
        }

        protected override void PerformAction()
        {
            if (config == null)
            {
                Debug.LogError($"{name} : NPCConfig 누락됨");
                return;
            }

            if (isServing)
                StartCoroutine(DeliverFood());
            else
                StartCoroutine(PickupFood());
        }

        private IEnumerator PickupFood()
        {
            yield return new WaitForSeconds(1f);

            // ✅ 음식 프리팹이 null일 수 있으므로 확인
            if (config.foodPrefab != null)
            {
                carriedFood = Instantiate(config.foodPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity, transform);
            }
            else
            {
                Debug.LogWarning($"{name} : foodPrefab이 NPCConfig에 지정되지 않음");
            }

            isServing = true;

            // ✅ 좌석 존재 여부 체크
            if (currentIndex < customerSeats.Length)
            {
                SetTarget(customerSeats[currentIndex]);
                ChangeState(NPCState.Move);
            }
            else
            {
                Debug.LogWarning($"{name} : customerSeats가 비어 있거나 인덱스 초과");
                ChangeState(NPCState.Idle);
            }
        }

        private IEnumerator DeliverFood()
        {
            yield return new WaitForSeconds(1f);

            if (carriedFood != null)
            {
                Destroy(carriedFood);
            }

            carriedFood = null;
            isServing = false;
            currentIndex++;

            if (currentIndex < customerSeats.Length)
            {
                SetTarget(kitchenZone);
                ChangeState(NPCState.Move);
            }
            else
            {
                ChangeState(NPCState.Idle);
            }
        }
    }
}
