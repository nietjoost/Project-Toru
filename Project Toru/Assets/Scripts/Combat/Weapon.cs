using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public GameObject bullet;

    private SpriteRenderer spriteRenderer;

    [NonSerialized]
    public GameObject weaponHolder;
    [NonSerialized]
    public bool weaponOut = false;

    public float damage = 10;
    public float RoundsPerMinute = 300;
    private float Timer = 0;

    private void Start()
    {
        bullet.GetComponent<Bullet>().weapon = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    private void Update()
    {
        if(Timer > 0)
        {
            Timer -= Time.deltaTime;
        }
    }

    public void Shoot()
	{
        if (!weaponOut)
        {
            RevealGun();
        }

        if(Timer <= 0)
        {
            Timer = 60 / RoundsPerMinute;
            Instantiate(bullet, transform.position, transform.rotation);
        }
	}

    public void RevealGun()
    {
		if (weaponOut) return;
        spriteRenderer.enabled = true;
        weaponOut = true;
        weaponHolder.GetComponent<Animator>().SetBool("isHoldingGun", true);
		LevelManager.emit("IsHoldingGun", weaponHolder);
    }

    public void HideGun()
    {
		if (!weaponOut) return;
        spriteRenderer.enabled = false;
        weaponOut = false;
        weaponHolder.GetComponent<Animator>().SetBool("isHoldingGun", false);
    }
}
