using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocationGenerator : MonoBehaviour
{
    [SerializeField] private Transform transformParent;
    [SerializeField] private LocationSettingsSO locSO;

    [SerializeField] private int maxAttemptsPerRoom = 15;
    [SerializeField] private float minRoomDistance = 1.0f;
    [SerializeField] private bool forceBossCreation = true;

    private GameObject hub;
    public GameObject Hub => hub;

    private List<GameObject> generatedRooms;
    private readonly List<DoorData> openDoors = new();

    public void Init(List<GameObject> generatedRooms)
    {
        this.generatedRooms = generatedRooms;
    }

    public void ResetInfo()
    {
        foreach (GameObject go in generatedRooms)
            Destroy(go);

        generatedRooms.Clear();

        openDoors.Clear();

        hub = null;
    }

    // ================= ENTRY =================

    public void GenerateLocation()
    { // Максимальное количество попыток перегенерации
        int currentRetry = 0;
        bool bossCreated = false;

        while (currentRetry < maxAttemptsPerRoom && !bossCreated)
        {
            Debug.Log($"Попытка генерации локации #{currentRetry + 1}");

            // Очищаем всё перед новой попыткой
            foreach (Transform child in transformParent)
                Destroy(child.gameObject);

            generatedRooms.Clear();
            openDoors.Clear();

            List<RoomType> plan = BuildGenerationPlan();

            // HUB
            hub = InstantiateHub();
            generatedRooms.Add(hub);

            RoomData hubData = hub.GetComponent<RoomData>();
            openDoors.AddRange(hubData.FreeDoors());

            bool allRoomsCreated = true;

            // GENERATION всех комнат, кроме босса
            foreach (var type in plan.Where(t => t != RoomType.Boss))
            {
                GameObject room = TryInstantiateRoom(type);

                if (room == null)
                {
                    Debug.LogWarning($"Комната {type} не создана");
                    allRoomsCreated = false;
                    break;
                }

                generatedRooms.Add(room);

                RoomData data = room.GetComponent<RoomData>();
                openDoors.AddRange(data.FreeDoors());
            }

            // Если все обычные комнаты созданы, пытаемся создать босса
            if (allRoomsCreated)
            {
                GameObject bossRoom = ForceInstantiateBoss();
                if (bossRoom != null)
                {
                    generatedRooms.Add(bossRoom);
                    RoomData bossData = bossRoom.GetComponent<RoomData>();
                    openDoors.AddRange(bossData.FreeDoors());
                    bossCreated = true;
                    Debug.Log($"Генерация успешна на попытке #{currentRetry + 1}");
                }
                else
                {
                    Debug.LogWarning($"Не удалось создать комнату босса, пробуем заново...");
                }
            }

            currentRetry++;
        }

        if (!bossCreated)
        {
            Debug.LogError($"Не удалось создать локацию после {maxAttemptsPerRoom} попыток!");
            // Можно создать хотя бы базовую локацию с хабом
            CreateFallbackLocation();
        }
    }

    private void CreateFallbackLocation()
    {
        // Очищаем всё
        foreach (Transform child in transformParent)
            Destroy(child.gameObject);

        generatedRooms.Clear();
        openDoors.Clear();

        // Создаем только хаб
        GameObject hub = InstantiateHub();
        generatedRooms.Add(hub);

        Debug.LogWarning("Создана только базовая локация с хабом");
    }

    // ================= HUB =================

    private GameObject InstantiateHub()
    {
        GameObject prefab =
            locSO.hubRoomsList[Random.Range(0, locSO.hubRoomsList.Count)];
        return Instantiate(prefab, transformParent);
    }

    // ================= PLAN =================

    private List<RoomType> BuildGenerationPlan()
    {
        List<RoomType> plan = new();

        AddRooms(plan, RoomType.Loot, locSO.lootRoomsCount);
        AddRooms(plan, RoomType.Enemy, locSO.enemyRoomsCount);
        AddRooms(plan, RoomType.Passage, locSO.passageRoomsCount);
        AddRooms(plan, RoomType.Other, locSO.otherRoomsCount);

        ShufflePlan(plan);
        plan.Add(RoomType.Boss);

        return plan;
    }

    private void AddRooms(List<RoomType> list, RoomType type, int count)
    {
        for (int i = 0; i < count; i++)
            list.Add(type);
    }

    private void ShufflePlan<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    // ================= SPAWN =================

    private RoomData FindFarthestRoom()
    {
        if (generatedRooms.Count == 0)
            return null;

        RoomData hub = generatedRooms[0].GetComponent<RoomData>();
        if (hub == null)
            return null;

        RoomData farthest = hub;
        float maxDist = 0f;

        foreach (var room in generatedRooms)
        {
            RoomData roomData = room.GetComponent<RoomData>();
            if (roomData == null)
                continue;

            float dist = Vector3.Distance(
                hub.transform.position,
                room.transform.position);

            if (dist > maxDist)
            {
                maxDist = dist;
                farthest = roomData;
            }
        }

        return farthest;
    }

    private GameObject TryInstantiateRoom(RoomType type)
    {
        if (openDoors.Count == 0)
        {
            Debug.LogWarning("Нет свободных дверей для создания комнаты");
            return null;
        }

        List<GameObject> prefabs = GetRoomPrefabs(type);
        if (prefabs == null || prefabs.Count == 0)
        {
            Debug.LogWarning($"Нет префабов для комнаты типа {type}");
            return null;
        }

        // Создаем копию списка дверей для итерации
        List<DoorData> doorsToTry = new List<DoorData>(openDoors);

        // Перемешиваем двери для случайного выбора
        ShuffleList(doorsToTry);

        foreach (var targetDoor in doorsToTry)
        {
            if (targetDoor == null || targetDoor.IsUsed)
                continue;

            // Перемешиваем префабы для разнообразия
            List<GameObject> shuffledPrefabs = new List<GameObject>(prefabs);
            ShuffleList(shuffledPrefabs);

            foreach (var prefab in shuffledPrefabs)
            {
                GameObject room = TryAttachRoom(prefab, targetDoor);

                if (room != null)
                    return room;
            }

            // Если после всех попыток комната не прикрепилась - помечаем дверь как тупик
            if (openDoors.Contains(targetDoor))
            {
                openDoors.Remove(targetDoor);
                targetDoor.IsUsed = true;

                // Обновляем состояние двери в родительской комнате
                RoomData parentRoom = targetDoor.GetComponentInParent<RoomData>();
                if (parentRoom != null)
                {
                    parentRoom.UpdateDoorState();
                }
            }
        }

        Debug.LogWarning($"Не удалось создать комнату типа {type}");
        return null;
    }

    // ================= BOSS =================

    private GameObject ForceInstantiateBoss()
    {
        if (locSO.bossRoomsList == null || locSO.bossRoomsList.Count == 0)
        {
            Debug.LogError("Нет префабов для комнаты босса");
            return null;
        }

        // ШАГ 1: Собираем список всех комнат с их свободными дверями
        List<KeyValuePair<RoomData, List<DoorData>>> roomsWithFreeDoors = new List<KeyValuePair<RoomData, List<DoorData>>>();

        foreach (var roomObj in generatedRooms)
        {
            RoomData roomData = roomObj.GetComponent<RoomData>();
            if (roomData != null)
            {
                List<DoorData> freeDoors = new List<DoorData>();
                foreach (var door in roomData.Doors)
                {
                    if (door != null && !door.IsUsed && !roomData.IsDoorUsed(door))
                    {
                        freeDoors.Add(door);
                    }
                }

                if (freeDoors.Count > 0)
                {
                    roomsWithFreeDoors.Add(new KeyValuePair<RoomData, List<DoorData>>(roomData, freeDoors));
                }
            }
        }

        Debug.Log($"Найдено комнат со свободными дверями для босса: {roomsWithFreeDoors.Count}");

        // Сортируем комнаты по удаленности от хаба (самые дальние первыми)
        if (generatedRooms.Count > 0)
        {
            Transform hubTransform = generatedRooms[0].transform;
            roomsWithFreeDoors = roomsWithFreeDoors
                .OrderByDescending(r => Vector3.Distance(r.Key.transform.position, hubTransform.position))
                .ToList();
        }

        // Перемешиваем префабы босса
        List<GameObject> bossPrefabs = new List<GameObject>(locSO.bossRoomsList);
        ShuffleList(bossPrefabs);

        // ШАГ 2: Пробуем прикрепиться к свободным дверям в порядке удаленности комнат
        foreach (var roomPair in roomsWithFreeDoors)
        {
            RoomData targetRoom = roomPair.Key;
            List<DoorData> freeDoors = roomPair.Value;

            // Перемешиваем двери в комнате
            ShuffleList(freeDoors);

            Debug.Log($"Проверяем комнату {targetRoom.gameObject.name} с {freeDoors.Count} свободными дверями для босса");

            foreach (var targetDoor in freeDoors)
            {
                if (targetDoor == null || targetDoor.IsUsed || targetRoom.IsDoorUsed(targetDoor))
                    continue;

                foreach (var prefab in bossPrefabs)
                {
                    GameObject bossRoom = TryAttachBossRoom(prefab, targetDoor, targetRoom);
                    if (bossRoom != null)
                    {
                        Debug.Log($"Комната босса успешно создана и пристыкована к двери комнаты {targetRoom.gameObject.name}");
                        return bossRoom;
                    }
                }
            }
        }

        // ШАГ 3: Если не удалось к свободным дверям, пробуем все двери (даже использованные)
        Debug.LogWarning("Не удалось прикрепиться к свободным дверям. Пробуем все двери...");

        List<KeyValuePair<RoomData, List<DoorData>>> allRoomsWithDoors = new List<KeyValuePair<RoomData, List<DoorData>>>();

        foreach (var roomObj in generatedRooms)
        {
            RoomData roomData = roomObj.GetComponent<RoomData>();
            if (roomData != null && roomData.Doors != null)
            {
                List<DoorData> allDoors = roomData.Doors.Where(d => d != null).ToList();
                if (allDoors.Count > 0)
                {
                    allRoomsWithDoors.Add(new KeyValuePair<RoomData, List<DoorData>>(roomData, allDoors));
                }
            }
        }

        // Сортируем по удаленности
        if (generatedRooms.Count > 0)
        {
            Transform hubTransform = generatedRooms[0].transform;
            allRoomsWithDoors = allRoomsWithDoors
                .OrderByDescending(r => Vector3.Distance(r.Key.transform.position, hubTransform.position))
                .ToList();
        }

        // Создаем отдельный список префабов для этого этапа
        List<GameObject> allBossPrefabs = new List<GameObject>(locSO.bossRoomsList);
        ShuffleList(allBossPrefabs);

        foreach (var roomPair in allRoomsWithDoors)
        {
            RoomData targetRoom = roomPair.Key;
            List<DoorData> allDoors = roomPair.Value;

            ShuffleList(allDoors);

            foreach (var targetDoor in allDoors)
            {
                if (targetDoor == null)
                    continue;

                foreach (var prefab in allBossPrefabs)
                {
                    GameObject bossRoom = TryForceAttachBossRoom(prefab, targetDoor, targetRoom);
                    if (bossRoom != null)
                    {
                        Debug.LogWarning($"Комната босса принудительно прикреплена к двери комнаты {targetRoom.gameObject.name}");
                        return bossRoom;
                    }
                }
            }
        }

        // ШАГ 4: Крайняя мера - создание отдельно
        if (forceBossCreation)
        {
            Debug.LogWarning("Все попытки прикрепить босса провалились. Создаем отдельно...");
            return CreateBossAsLastResort();
        }

        Debug.LogError("Не удалось создать комнату босса!");
        return null;
    }

    // Новый метод для прикрепления комнаты босса с дополнительными проверками
    // ================= BOSS - ИСПРАВЛЕННЫЕ МЕТОДЫ =================

    // Новый метод для прикрепления комнаты босса с правильным позиционированием
    private GameObject TryAttachBossRoom(GameObject prefab, DoorData targetDoor, RoomData targetRoom)
    {
        if (targetDoor == null || targetRoom == null)
            return null;

        // Проверяем, что дверь действительно свободна
        if (targetDoor.IsUsed || targetRoom.IsDoorUsed(targetDoor))
            return null;

        GameObject bossRoom = Instantiate(prefab, transformParent);
        RoomData bossData = bossRoom.GetComponent<RoomData>();

        if (bossData == null)
        {
            Destroy(bossRoom);
            return null;
        }

        // Ищем подходящую дверь у комнаты босса
        List<DoorData> bossDoors = new List<DoorData>();
        foreach (var door in bossData.Doors)
        {
            if (door != null && !door.IsUsed)
            {
                bossDoors.Add(door);
            }
        }

        if (bossDoors.Count == 0)
        {
            Destroy(bossRoom);
            return null;
        }

        ShuffleList(bossDoors);

        foreach (var bossDoor in bossDoors)
        {
            // Сохраняем оригинальное вращение префаба
            Quaternion originalRotation = bossRoom.transform.rotation;

            // Сбрасываем позицию и вращение
            bossRoom.transform.position = Vector3.zero;
            bossRoom.transform.rotation = Quaternion.identity;

            // Получаем направления дверей в локальных координатах
            Vector3 targetDoorForward = targetDoor.transform.forward;
            Vector3 bossDoorForward = bossDoor.transform.forward;

            bossRoom.transform.position = Vector3.up * 100;

            // Нормализуем и убираем компоненту Y
            targetDoorForward.y = 0f;
            bossDoorForward.y = 0f;

            targetDoorForward.Normalize();
            bossDoorForward.Normalize();

            // Вычисляем угол между направлениями дверей
            // Нам нужно, чтобы bossDoor смотрел в противоположную сторону от targetDoor
            float angle = Vector3.SignedAngle(bossDoorForward, -targetDoorForward, Vector3.up);

            // Поворачиваем всю комнату босса
            bossRoom.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Теперь вычисляем позицию: совмещаем bossDoor с targetDoor
            // Сначала получаем мировую позицию bossDoor после вращения комнаты
            Vector3 bossDoorWorldPos = bossRoom.transform.TransformPoint(bossDoor.transform.localPosition);

            // Вычисляем смещение, чтобы совместить двери
            Vector3 offset = targetDoor.transform.position - bossDoorWorldPos;

            // Применяем смещение ко всей комнате
            bossRoom.transform.position += offset;

            // Выравниваем высоту с родительской комнатой
            float heightCorrection = targetRoom.transform.position.y - bossRoom.transform.position.y;
            bossRoom.transform.position += new Vector3(0f, heightCorrection, 0f);

            // Проверяем коллизии - ВАЖНО: проверяем ПОСЛЕ правильного позиционирования
            if (HasOverlap(bossRoom))
            {
                // Возвращаем оригинальное вращение для следующей попытки
                bossRoom.transform.rotation = originalRotation;
                continue;
            }

            if (TooClose(bossRoom))
            {
                bossRoom.transform.rotation = originalRotation;
                continue;
            }

            bossRoom.transform.position = Vector3.up * 100;

            // Успешное прикрепление
            targetDoor.IsUsed = true;
            bossDoor.IsUsed = true;

            targetRoom.MarkDoorAsUsed(targetDoor);
            bossData.MarkDoorAsUsed(bossDoor);

            if (openDoors.Contains(targetDoor))
            {
                openDoors.Remove(targetDoor);
            }

            // Добавляем свободные двери босса
            foreach (var newDoor in bossData.FreeDoors())
            {
                if (newDoor != null && !newDoor.IsUsed && !openDoors.Contains(newDoor))
                {
                    openDoors.Add(newDoor);
                }
            }

            // Отладочная информация
            Debug.Log($"Босс прикреплен: {targetRoom.gameObject.name} -> {bossData.gameObject.name}");
            Debug.Log($"Позиция босса: {bossRoom.transform.position}, Вращение: {bossRoom.transform.rotation.eulerAngles}");

            bossRoom.transform.position = Vector3.up * 100;

            return bossRoom;
        }

        Destroy(bossRoom);
        return null;
    }

    // Метод для принудительного прикрепления (игнорирует статус двери)
    private GameObject TryForceAttachBossRoom(GameObject prefab, DoorData targetDoor, RoomData targetRoom)
    {
        if (targetDoor == null || targetRoom == null)
            return null;

        GameObject bossRoom = Instantiate(prefab, transformParent);
        RoomData bossData = bossRoom.GetComponent<RoomData>();

        if (bossData == null)
        {
            Destroy(bossRoom);
            return null;
        }

        // Берем первую доступную дверь у босса
        DoorData bossDoor = bossData.Doors.FirstOrDefault(d => d != null);
        if (bossDoor == null)
        {
            Destroy(bossRoom);
            return null;
        }

        // Сохраняем оригинальное вращение
        Quaternion originalRotation = bossRoom.transform.rotation;

        // Сбрасываем позицию и вращение
        bossRoom.transform.position = Vector3.zero;
        bossRoom.transform.rotation = Quaternion.identity;

        // Получаем направления дверей
        Vector3 targetDoorForward = targetDoor.transform.forward;
        Vector3 bossDoorForward = bossDoor.transform.forward;

        targetDoorForward.y = 0f;
        bossDoorForward.y = 0f;

        targetDoorForward.Normalize();
        bossDoorForward.Normalize();

        // Вычисляем угол поворота
        float angle = Vector3.SignedAngle(bossDoorForward, -targetDoorForward, Vector3.up);

        // Поворачиваем комнату
        bossRoom.transform.rotation = Quaternion.Euler(0f, angle, 0f);

        // Вычисляем позицию
        Vector3 bossDoorWorldPos = bossRoom.transform.TransformPoint(bossDoor.transform.localPosition);
        Vector3 offset = targetDoor.transform.position - bossDoorWorldPos;
        bossRoom.transform.position += offset;

        bossRoom.transform.position = Vector3.up * 100;

        // Выравниваем высоту
        float heightCorrection = targetRoom.transform.position.y - bossRoom.transform.position.y;
        bossRoom.transform.position += new Vector3(0f, heightCorrection, 0f);

        // Проверяем коллизии
        if (HasOverlap(bossRoom) || TooClose(bossRoom))
        {
            Destroy(bossRoom);
            return null;
        }

        // Принудительное прикрепление
        targetDoor.IsUsed = true;
        bossDoor.IsUsed = true;

        targetRoom.MarkDoorAsUsed(targetDoor);
        bossData.MarkDoorAsUsed(bossDoor);

        if (openDoors.Contains(targetDoor))
        {
            openDoors.Remove(targetDoor);
        }

        // Добавляем свободные двери босса
        foreach (var newDoor in bossData.FreeDoors())
        {
            if (newDoor != null && !newDoor.IsUsed && !openDoors.Contains(newDoor))
            {
                openDoors.Add(newDoor);
            }
        }

        Debug.LogWarning($"Босс принудительно прикреплен: {targetRoom.gameObject.name} -> {bossData.gameObject.name}");
        Debug.Log($"Позиция: {bossRoom.transform.position}, Вращение: {bossRoom.transform.rotation.eulerAngles}");

        bossRoom.transform.position = Vector3.up * 100;

        return bossRoom;
    }

    // Крайняя мера - создание без прикрепления
    // Крайняя мера - создание без прикрепления (только если forceBossCreation = true)
    private GameObject CreateBossAsLastResort()
    {
        // Находим самую дальнюю комнату
        RoomData farthestRoom = FindFarthestRoom();
        if (farthestRoom == null)
        {
            Debug.LogError("Не удалось найти комнату для создания босса");
            return null;
        }

        // Создаем комнату босса
        GameObject prefab = locSO.bossRoomsList[Random.Range(0, locSO.bossRoomsList.Count)];
        GameObject bossRoom = Instantiate(prefab, transformParent);

        RoomData bossData = bossRoom.GetComponent<RoomData>();
        if (bossData == null)
        {
            Destroy(bossRoom);
            return null;
        }
        bossRoom.transform.position = Vector3.up * 100;
        // Пытаемся найти позицию для пристыковки к дальней комнате
        DoorData farthestDoor = null;
        List<DoorData> farthestDoors = farthestRoom.FreeDoors().Where(d => d != null).ToList();
        if (farthestDoors.Count == 0 && farthestRoom.Doors != null)
        {
            farthestDoors = farthestRoom.Doors.Where(d => d != null).ToList();
        }

        if (farthestDoors.Count > 0)
        {
            farthestDoor = farthestDoors[Random.Range(0, farthestDoors.Count)];
        }
        bossRoom.transform.position = Vector3.up * 100;
        // Если есть дверь у дальней комнаты, пытаемся пристыковаться
        if (farthestDoor != null)
        {
            // Находим подходящую дверь у комнаты босса
            DoorData bossDoor = null;
            foreach (var door in bossData.Doors)
            {
                if (door != null)
                {
                    bossDoor = door;
                    break;
                }
            }

            if (bossDoor != null)
            {
                // Вычисляем позицию для пристыковки
                bossRoom.transform.rotation = Quaternion.LookRotation(-farthestDoor.transform.forward);

                Vector3 bossDoorWorld = bossDoor.transform.position;
                Vector3 delta = farthestDoor.transform.position - bossDoorWorld;
                bossRoom.transform.position += delta;

                // Проверяем коллизии
                if (!HasOverlap(bossRoom) && !TooClose(bossRoom))
                {
                    // Помечаем двери как использованные
                    farthestDoor.IsUsed = true;
                    bossDoor.IsUsed = true;
                    farthestRoom.UpdateDoorState();
                    bossData.UpdateDoorState();

                    Debug.LogWarning($"Комната босса создана рядом с {farthestRoom.gameObject.name} (полупристыкована)");
                    return bossRoom;
                }
            }
        }

        // Если не удалось пристыковаться, создаем рядом
        Vector3 offset = farthestRoom.transform.forward * 15f;
        bossRoom.transform.position = farthestRoom.transform.position + offset;

        // Корректируем позицию, чтобы избежать коллизий
        int attempts = 0;
        while ((HasOverlap(bossRoom) || TooClose(bossRoom)) && attempts < 20)
        {
            float angle = Random.Range(0f, 360f);
            float distance = Random.Range(10f, 20f);
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            offset = direction * distance;
            bossRoom.transform.position = farthestRoom.transform.position + offset;
            bossRoom.transform.rotation = Quaternion.LookRotation(-direction);
            attempts++;
        }

        // Добавляем в списки
        generatedRooms.Add(bossRoom);

        // Добавляем свободные двери босса в общий список
        foreach (var door in bossData.FreeDoors())
        {
            if (door != null && !door.IsUsed)
            {
                openDoors.Add(door);
            }
        }

        bossRoom.transform.position = Vector3.up * 100;

        Debug.LogWarning($"Комната босса создана рядом с {farthestRoom.gameObject.name} (отдельно)");
        return bossRoom;
    }

    // ================= CORE =================

    private GameObject TryAttachRoom(GameObject prefab, DoorData targetDoor)
    {
        if (targetDoor == null)
            return null;

        RoomData parentRoom = targetDoor.GetComponentInParent<RoomData>();
        if (parentRoom != null)
        {
            if (parentRoom.IsDoorUsed(targetDoor))
            {
                targetDoor.IsUsed = true;
                return null;
            }
        }
        else if (targetDoor.IsUsed)
        {
            return null;
        }

        GameObject room = Instantiate(prefab, transformParent);
        RoomData curr = room.GetComponent<RoomData>();
        RoomData prev = targetDoor.GetComponentInParent<RoomData>();

        if (curr == null || prev == null)
        {
            Destroy(room);
            return null;
        }

        List<DoorData> availableDoors = new List<DoorData>();
        foreach (var currDoor in curr.Doors)
        {
            if (currDoor != null && !currDoor.IsUsed)
            {
                availableDoors.Add(currDoor);
            }
        }

        if (availableDoors.Count == 0)
        {
            Destroy(room);
            return null;
        }

        ShuffleList(availableDoors);

        foreach (var currDoor in availableDoors)
        {
            // Сохраняем оригинальное вращение
            Quaternion originalRotation = room.transform.rotation;

            // Сбрасываем позицию и вращение
            room.transform.position = Vector3.zero;
            room.transform.rotation = Quaternion.identity;

            Vector3 prevFwd = targetDoor.transform.forward;
            Vector3 currFwd = currDoor.transform.forward;

            prevFwd.y = 0f;
            currFwd.y = 0f;

            prevFwd.Normalize();
            currFwd.Normalize();

            float dot = Vector3.Dot(prevFwd, -currFwd);
            if (dot < 0.7f)
            {
                room.transform.rotation = originalRotation;
                continue;
            }

            float angle = Vector3.SignedAngle(currFwd, -prevFwd, Vector3.up);
            room.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Используем TransformPoint для правильного позиционирования
            Vector3 currDoorWorldPos = room.transform.TransformPoint(currDoor.transform.localPosition);
            Vector3 delta = targetDoor.transform.position - currDoorWorldPos;
            room.transform.position += delta;

            // Выравниваем высоту
            float heightCorrection = prev.transform.position.y - room.transform.position.y;
            room.transform.position += new Vector3(0f, heightCorrection, 0f);

            if (HasOverlap(room))
            {
                room.transform.rotation = originalRotation;
                continue;
            }

            if (TooClose(room))
            {
                room.transform.rotation = originalRotation;
                continue;
            }

            // Успешное прикрепление
            targetDoor.IsUsed = true;
            currDoor.IsUsed = true;


            if (prev != null)
            {
                prev.MarkDoorAsUsed(targetDoor);
            }
            if (curr != null)
            {
                curr.MarkDoorAsUsed(currDoor);
            }

            if (openDoors.Contains(targetDoor))
            {
                openDoors.Remove(targetDoor);
            }

            foreach (var newDoor in curr.FreeDoors())
            {
                if (newDoor != null && !newDoor.IsUsed && !openDoors.Contains(newDoor))
                {
                    openDoors.Add(newDoor);
                }
            }

            return room;
        }

        Destroy(room);
        return null;
    }

    // Добавьте метод для отладки позиционирования (опционально)
    private void DebugDoorInfo(DoorData door, string prefix)
    {
        if (door != null)
        {
            Debug.Log($"{prefix}: {door.name}");
            Debug.Log($"  Позиция: {door.transform.position}");
            Debug.Log($"  Локальная позиция: {door.transform.localPosition}");
            Debug.Log($"  Вперед: {door.transform.forward}");
            Debug.Log($"  Использована: {door.IsUsed}");

            RoomData parent = door.GetComponentInParent<RoomData>();
            if (parent != null)
            {
                Debug.Log($"  Родитель: {parent.gameObject.name}");
            }
        }
    }

    // ================= COLLISION =================

    private bool HasOverlap(GameObject room)
    {
        Collider[] cols = room.GetComponentsInChildren<Collider>();
        if (cols.Length == 0)
            return false;

        foreach (Transform child in transformParent)
        {
            if (child.gameObject == room)
                continue;

            Collider[] otherCols = child.GetComponentsInChildren<Collider>();

            foreach (var c1 in cols)
            {
                if (c1 == null || c1.isTrigger)
                    continue;

                foreach (var c2 in otherCols)
                {
                    if (c2 == null || c2.isTrigger)
                        continue;

                    if (Physics.ComputePenetration(
                        c1, c1.transform.position, c1.transform.rotation,
                        c2, c2.transform.position, c2.transform.rotation,
                        out _, out float dist))
                    {
                        if (dist > 0.01f)
                            return true;
                    }
                }
            }
        }

        return false;
    }

    private bool TooClose(GameObject room)
    {
        foreach (var r in generatedRooms)
        {
            if (r == null || r == room)
                continue;

            if (Vector3.Distance(
                    r.transform.position,
                    room.transform.position) < minRoomDistance)
                return true;
        }
        return false;
    }

    // ================= PREFABS =================

    private List<GameObject> GetRoomPrefabs(RoomType type)
    {
        return type switch
        {
            RoomType.Loot => locSO.lootRoomsList,
            RoomType.Enemy => locSO.enemyRoomsList,
            RoomType.Passage => locSO.passageRoomsList,
            RoomType.Other => locSO.otherRoomsList,
            RoomType.Boss => locSO.bossRoomsList,
            _ => null
        };
    }

    // ================= UTILITY =================

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}