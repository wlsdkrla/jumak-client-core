// ✅ NPCConfig.cs (ScriptableObject 기반 NPC 설정 데이터)
using UnityEngine;

namespace NPCSystem
{
    [CreateAssetMenu(fileName = "NPCConfig", menuName = "NPC/Create NPC Config", order = 0)]
    public class NPCConfig : ScriptableObject
    {
        public NPCType npcType;
        public GameObject prefab;

        [Header("Movement")]
        public float moveSpeed = 2f;
        public float arrivalThreshold = 0.1f;

        [Header("Animation")] 
        public string walkAnimName = "Walk";
        public string idleAnimName = "Idle";
        public string sitAnimName = "Sit";

        [Header("Customer Only")]
        public float sitDuration = 60f;

        [Header("Servant Only")]
        public GameObject foodPrefab;
    }
}
