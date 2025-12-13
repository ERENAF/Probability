using System.Runtime.CompilerServices;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float currHP;
    [SerializeField] float maxHP;
    [SerializeField] GameObject hpBar;

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

    private void ImageVirtualBar()
    {
        Transform hpBarTransform = hpBar.GetComponent<Transform>();
        hpBarTransform.localScale = new Vector3(Mathf.Max(currHP/maxHP,0),hpBarTransform.localScale.y,hpBarTransform.localScale.z);
    }

}
