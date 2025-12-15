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
    public Transform start;
    public TextMeshProUGUI ammoText;

    void Start()
    {
        ChangeWeapon();
        chosenWeapon.GetComponent<Weapon>().Reload();
    }

    void Update()
    {
        if (chosenWeapon != null)
        {
            chosenWeapon.GetComponent<Weapon>().Use(GetComponent<DiceCharacter>());
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
        if (ammoText != null) ammoText.text = chosenWeapon.GetComponent<Weapon>().ToStringAmmo();
    }

    private void ChangeWeapon()
    {
        switch (chosenWeaponIndex)
        {
            case ChosenWeapon.first:
                Destroy(chosenWeapon);
                chosenWeapon = Instantiate(weapon1,start);
                chosenWeapon.GetComponent<Weapon>().AttachToTransform(start);
                break;
            case ChosenWeapon.second:
                Destroy(chosenWeapon);
                chosenWeapon = Instantiate(weapon2);
                chosenWeapon.GetComponent<Weapon>().AttachToTransform(start);
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
