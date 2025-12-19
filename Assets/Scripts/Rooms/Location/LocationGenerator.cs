using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LocationGenerator : MonoBehaviour
{
    [SerializeField] Transform transformParent;

    [SerializeField] private LocationSettingsSO locSO;

    List<GameObject> generatedRoomsList;

    static int deep = 0;

    private int lootRoomsCount = 0;
    private int enemyRoomsCount = 0;
    private int passageRoomsCount = 0;
    private int otherRoomsCount = 0;
    private int bossRoomsCount = 0;

    public void GenerateLocation()
    {
        deep = 0;

        GameObject hub = InstantiateHub();

        Process(hub);
    }

    private GameObject InstantiateHub()
    {
        GameObject hubObj = locSO.hubRoomsList[Rand(locSO.hubRoomsList.Count)];

        GameObject hubRoom = Instantiate(hubObj, transformParent, true);

        return hubRoom;
    }

    private void Process(GameObject currRoom)
    {
        if (deep >= 10)
        {
            return;
        }

        GameObject newRoom = InstantiateRoom(GetRandRoom(), currRoom);

        if (newRoom != null)
        {
            deep++;
            Process(newRoom);
        }
    }

    private GameObject InstantiateRoom(GameObject roomPrefab, GameObject prevRoom)
    {
        GameObject res = Instantiate(roomPrefab, transformParent);

        RoomData currRoomData = res.GetComponent<RoomData>();
        RoomData prevRoomData = prevRoom.GetComponent<RoomData>();

        if (currRoomData == null || prevRoomData == null || 
            currRoomData.RoomDoorsPoints == null || prevRoomData.RoomDoorsPoints == null ||
            currRoomData.RoomDoorsPoints.Count == 0 || prevRoomData.RoomDoorsPoints.Count == 0)
        {
            Destroy(res);
            return null;
        }

        CollliderChecker currRoomCC = res.GetComponent<CollliderChecker>();
        if (currRoomCC == null)
        {
            Destroy(res);
            return null;
        }

        Vector3 initialPosition = res.transform.position;
        Quaternion initialRotation = res.transform.rotation;

        foreach (var prevPoint in prevRoomData.RoomDoorsPoints)
        {
            foreach (var currPoint in currRoomData.RoomDoorsPoints)
            {
                res.transform.position = initialPosition;
                res.transform.rotation = initialRotation;

                Physics.SyncTransforms();
                currRoomCC.ClearCollisions();

                Vector3 prevPointWorldPos = prevPoint.position;
                Vector3 prevPointForward = prevPoint.forward;

                Vector3 currPointLocalPos = currPoint.localPosition;
                Vector3 currPointWorldPosAtInit = res.transform.TransformPoint(currPointLocalPos);

                Vector3 offset = prevPointWorldPos - currPointWorldPosAtInit;

                Vector3 newRoomPosition = initialPosition + offset;
                newRoomPosition.y = prevPointWorldPos.y;

                res.transform.position = newRoomPosition;

                Physics.SyncTransforms();

                for (int i = 0; i < 10; i++)
                {
                    Physics.Simulate(Time.fixedDeltaTime);
                }

                Vector3 currPointFinalWorldPos = currPoint.position;
                Vector3 prevPointFinalWorldPos = prevPoint.position;
                
                float distance = Vector3.Distance(currPointFinalWorldPos, prevPointFinalWorldPos);

                Vector3 currPointForward = currPoint.forward;
                float angleBetweenForwards = Vector3.Angle(currPointForward, -prevPointForward);
                
                bool directionsMatch = angleBetweenForwards < 60f;

                bool hasBadOverlap = HasSignificantOverlap(res, prevRoom);

                if (distance < 0.2f && directionsMatch && !hasBadOverlap)
                {
                    return res;
                }
            }
        }

        Destroy(res);
        return null;
    }

    private bool HasSignificantOverlap(GameObject newRoom, GameObject prevRoom)
    {
        Collider[] newColliders = newRoom.GetComponentsInChildren<Collider>();
        
        foreach (Transform child in transformParent)
        {
            if (child.gameObject == newRoom || child.gameObject == prevRoom)
                continue;

            Collider[] otherColliders = child.GetComponentsInChildren<Collider>();
            
            foreach (var newCol in newColliders)
            {
                if (newCol.isTrigger) continue;
                
                foreach (var otherCol in otherColliders)
                {
                    if (otherCol.isTrigger) continue;
                    
                    if (newCol.bounds.Intersects(otherCol.bounds))
                    {
                        float volume = GetOverlapVolume(newCol.bounds, otherCol.bounds);
                        if (volume > 5f)
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    private float GetOverlapVolume(Bounds b1, Bounds b2)
    {
        float minX = Mathf.Max(b1.min.x, b2.min.x);
        float maxX = Mathf.Min(b1.max.x, b2.max.x);
        float minY = Mathf.Max(b1.min.y, b2.min.y);
        float maxY = Mathf.Min(b1.max.y, b2.max.y);
        float minZ = Mathf.Max(b1.min.z, b2.min.z);
        float maxZ = Mathf.Min(b1.max.z, b2.max.z);
        
        if (maxX < minX || maxY < minY || maxZ < minZ) return 0f;
        
        return (maxX - minX) * (maxY - minY) * (maxZ - minZ);
    }

    private GameObject GetRandRoom()
    {
        GameObject resRoom = null;

        if (Rand(10) < 2)
        {
            passageRoomsCount ++;
            resRoom = locSO.passageRoomsList[Rand(locSO.passageRoomsList.Count)];
        }
        if (Rand(10) < 2)
        {
            lootRoomsCount++;
            resRoom = locSO.lootRoomsList[Rand(locSO.lootRoomsList.Count)];
        }
        if (Rand(10) < 2)
        {
            enemyRoomsCount++;
            resRoom = locSO.enemyRoomsList[Rand(locSO.enemyRoomsList.Count)];
        }
        if (Rand(10) < 2)
        {
            otherRoomsCount++;
            resRoom = locSO.otherRoomsList[Rand(locSO.otherRoomsList.Count)];
        }

        if (resRoom == null) return GetRandRoom();

        return resRoom;
    }

    private List<Transform> GetPoints(Transform transformObg)
    {
        List<Transform> points = new();

        foreach (Transform item in transformObg)
        {
            if (item.CompareTag("RoomDoorPoint"))
                points.Add(item);
        }

        return points;
    }
    private int Rand(int size)
    {
        return UnityEngine.Random.Range(0, size);
    }
}
