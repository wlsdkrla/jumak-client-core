using UnityEngine;
using System.Collections;

namespace NPCSystem
{
    public class CustomerNPCController : BaseNPCController
    {
        private Transform seatZone;
        private Transform exitZone;
        private bool hasSat = false;

        public void Initialize(Transform seat, Transform exit, NPCConfig npcConfig)
        {
            config = npcConfig;
            seatZone = seat;
            exitZone = exit;

            ApplyConfig(); 

            if (seatZone == null || exitZone == null)
            {
                Debug.LogError($"{name} : 좌석 또는 출구 위치가 지정되지 않았습니다.");
                return;
            }

            SetTarget(seatZone);
            ChangeState(NPCState.Move);
        }

        protected override void OnArrival()
        {
            if (target == seatZone && !hasSat)
            {
                hasSat = true;
                ChangeState(NPCState.Sit);
                StartCoroutine(SitAndLeave());
            }
            else if (target == exitZone)
            {
                ChangeState(NPCState.Exit);
            }
        }

        private IEnumerator SitAndLeave()
        {
            float duration = Mathf.Max(1f, config.sitDuration); // 최소 대기시간 보장
            yield return new WaitForSeconds(duration);

            SetTarget(exitZone);
            ChangeState(NPCState.Move);
        }
    }
}
