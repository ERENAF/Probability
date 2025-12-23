using TMPro;
using UnityEngine;

[RequireComponent(typeof(DiceCharacter))]
public class Health : MonoBehaviour
{
    public float currHP;
    [SerializeField] float maxHP;
    [SerializeField] GameObject hpBar;
    [SerializeField] TextMeshProUGUI hpText;
    void Start()
    {
        SetHP();
    }
    void FixedUpdate()
    {
        IsAlive();
    }

    public void ChangeHP(int changeHP)
    {
        currHP = Mathf.Min(currHP+changeHP,maxHP);
    }
    private void SetHP()
    {
        maxHP+=(float)GetComponent<DiceCharacter>().ConstitutionMod;
        currHP = maxHP;
    }
    protected virtual void IsAlive()
    {
        if (currHP <= 0)
        {
            Death();
        }
        else
        {
            ImageVirtualBar();
        }
    }
    protected virtual void Death()
    {

    }

    protected virtual void ImageVirtualBar()
    {
        if (hpBar != null)
        {
            Transform hpBarTransform = hpBar.GetComponent<Transform>();
            hpBarTransform.localScale = new Vector3(Mathf.Max(currHP/maxHP,0),hpBarTransform.localScale.y,hpBarTransform.localScale.z);
        }
        if (hpText != null)
        {
            hpText.text = $"{(int)currHP}/{(int)maxHP}";
        }

    }

}
