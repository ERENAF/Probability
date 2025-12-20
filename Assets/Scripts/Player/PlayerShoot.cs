using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;


public enum ChosenWeapon
{
    first = 0,
    second = 1
}
public class PlayerShoot : MonoBehaviour
{
    [SerializeField]private GameObject weapon1;
    [SerializeField]private GameObject weapon2;
    public GameObject chosenWeapon;
    public ChosenWeapon chosenWeaponIndex = ChosenWeapon.first;
    [SerializeField]public DicePlayer diceCharacter;
    public Transform start;
    public TextMeshProUGUI ammoText;

    void Start()
    {
        ChangeWeapon();
    }

    void Update()
    {
        if (chosenWeapon != null)
        {
            chosenWeapon.GetComponent<Weapon>().Use(diceCharacter);
            chosenWeapon.GetComponent<Weapon>().Reload();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (chosenWeaponIndex == ChosenWeapon.first)
            {
                chosenWeaponIndex = ChosenWeapon.second;
            }
            else
            {
                chosenWeaponIndex = ChosenWeapon.first;
            }
            ChangeWeapon();
        }
        if (Input.GetKeyDown(KeyCode.Q) && chosenWeapon != null)
        {
            chosenWeapon.GetComponent<Weapon>().DropItem(start,10);
            chosenWeapon = null;
            if (chosenWeaponIndex == ChosenWeapon.first)
            {
                chosenWeaponIndex = ChosenWeapon.second;
                weapon1 = null;
            }
            else
            {
                chosenWeaponIndex = ChosenWeapon.first;
                weapon2 = null;
            }
            ChangeWeapon();
        }

        if (chosenWeapon != null)
        {
            if (ammoText != null) ammoText.text = chosenWeapon.GetComponent<Weapon>().ToStringAmmo();
        }
    }

    private void ChangeWeapon()
    {
        if (weapon1 != null || weapon2 != null)
        switch (chosenWeaponIndex)
        {
            case ChosenWeapon.first:
                if (weapon1 != null)
                {
                    if (chosenWeapon != null)
                    {
                        if (weapon2 != null)
                        {
                            weapon2.GetComponent<Weapon>().allAmmo = chosenWeapon.GetComponent<Weapon>().allAmmo;
                            weapon2.GetComponent<Weapon>().currAmmo = chosenWeapon.GetComponent<Weapon>().currAmmo;
                        }
                        Destroy(chosenWeapon);
                    }
                    chosenWeapon = Instantiate(weapon1,start);
                    chosenWeapon.GetComponent<Weapon>().AttachToTransform(start);
                    chosenWeapon.GetComponent<Weapon>().playerCamera = GetComponent<PlayerCameraController>();
                }
                break;
            case ChosenWeapon.second:
                if (weapon2 != null)
                {
                    if (chosenWeapon != null)
                    {

                        if (weapon1 != null)
                        {
                            weapon1.GetComponent<Weapon>().allAmmo = chosenWeapon.GetComponent<Weapon>().allAmmo;
                            weapon1.GetComponent<Weapon>().currAmmo = chosenWeapon.GetComponent<Weapon>().currAmmo;
                        }
                        Destroy(chosenWeapon);
                    }
                    chosenWeapon = Instantiate(weapon2,start);
                    chosenWeapon.GetComponent<Weapon>().AttachToTransform(start);
                    chosenWeapon.GetComponent<Weapon>().playerCamera = GetComponent<PlayerCameraController>();
                }
                break;
        }
    }

    public void SetWeapon(GameObject newWeapon)
    {
        if (weapon1 == null && weapon2 == null)
        {
            weapon1 = newWeapon;
            chosenWeaponIndex = ChosenWeapon.first;
        }
        else if (weapon1 != null && weapon2 == null)
        {
            weapon2 = newWeapon;
            chosenWeaponIndex = ChosenWeapon.second;
        }
        else if(weapon1 == null && weapon2 != null)
        {
            weapon1 = newWeapon;
            chosenWeaponIndex = ChosenWeapon.first;
        }
        else if (weapon1 != null && weapon2 != null)
        {
            switch (chosenWeaponIndex)
            {
                case ChosenWeapon.first:
                    weapon1 = newWeapon;
                    break;
                case ChosenWeapon.second:
                    weapon2 = newWeapon;
                    break;
            }
        }

    }
}
