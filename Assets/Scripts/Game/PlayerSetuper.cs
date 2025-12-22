using UnityEngine;

public class PlayerSetuper : MonoBehaviour
{
    [SerializeField] private GameObject PlayerPrefab;

    private GameObject player;

    public void CreatePlayer(Transform setupPoint)
    {
        if (player != null) Destroy(player);

        player = Instantiate(PlayerPrefab);

        player.transform.position = setupPoint.position;
        player.transform.localRotation = setupPoint.localRotation;
    }
}
