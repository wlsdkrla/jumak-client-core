using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using NPCSystem;

public class NPCManager : MonoBehaviour
{
    [Header("NPC Configs")]
    [SerializeField] private NPCConfig customerConfig;
    [SerializeField] private NPCConfig servantConfig;

    [Header("Spawn Points")]
    [SerializeField] private List<Transform> customerSpawnPoints;
    [SerializeField] private List<Transform> customerSeatZones;
    [SerializeField] private Transform customerExitZone;
    [SerializeField] private Transform servantSpawnPoint;
    [SerializeField] private Transform kitchenZone;

    void Start()
    {
        SpawnCustomers(5);
        SpawnServant();
    }

    void SpawnCustomers(int count)
    {
        var shuffledSeats = customerSeatZones.OrderBy(_ => Random.value).ToList();

        for (int i = 0; i < count && i < shuffledSeats.Count; i++)
        {
            var spawn = customerSpawnPoints[Random.Range(0, customerSpawnPoints.Count)];
            var customer = Instantiate(customerConfig.prefab, spawn.position, Quaternion.identity);

            var ctrl = customer.GetComponent<CustomerNPCController>();
            if (ctrl != null)
                ctrl.Initialize(shuffledSeats[i], customerExitZone, customerConfig);
        }
    }

    void SpawnServant()
    {
        var servant = Instantiate(servantConfig.prefab, servantSpawnPoint.position, Quaternion.identity);

        var ctrl = servant.GetComponent<ServantNPCController>();
        if (ctrl != null)
            ctrl.Initialize(kitchenZone, customerSeatZones.ToArray(), servantConfig);
    }
}
