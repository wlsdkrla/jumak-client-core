// Scripts/NPCSystem/Enums/NPCSystem.cs

namespace NPCSystem
{
    /// <summary>
    /// NPC의 타입을 분류 (행위와 역할 기반으로 확장 가능)
    /// </summary>
    public enum NPCType
    {
        Customer,
        Servant,
        Manager
    }

    /// <summary>
    /// FSM 상태를 정의 (공통 FSM 컨트롤러와 연동됨)
    /// </summary>
    public enum NPCState
    {
        Idle,
        Move,
        Action,
        Sit,
        Exit
    }

    /// <summary>
    /// 공통 NPC 인터페이스 (상태 전환 및 목표 설정 기능 제공)
    /// </summary>
    public interface INPCAction
    {
        void SetTarget(UnityEngine.Transform target);
        void ChangeState(NPCState newState);
    }
}
