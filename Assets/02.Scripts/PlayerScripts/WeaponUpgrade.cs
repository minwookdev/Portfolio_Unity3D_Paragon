using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgrade : MonoBehaviour
{
    private FireControll fControll;
 
    void Awake()
    {
        fControll = GetComponent<FireControll>();
    }
    public void PowerUpgrade(int number)
    {
        if (number == 0)
        {
            if(GameManager.instance.rifleGrade != 2)
            {
                fControll.currRiflePowerGrade += 1;
                GameManager.instance.currentGold -= 150;
                GameManager.instance.UpdateCurrentGold();
            }
            GameManager.instance.rifleGrade = fControll.currRiflePowerGrade;
        }

        else if (number == 1)
        {
            if (GameManager.instance.shotGrade != 2)
            {
                fControll.currShotPowerGrade += 1;
                GameManager.instance.currentGold -= 200;
                GameManager.instance.UpdateCurrentGold();

            }
            GameManager.instance.shotGrade = fControll.currShotPowerGrade;
        }

        else if (number == 2)
        {
            if (GameManager.instance.sniperGrade != 2)
            {
                fControll.currSniperPowerGrade += 1;
                GameManager.instance.currentGold -= 250;
                GameManager.instance.UpdateCurrentGold();

            }
            GameManager.instance.sniperGrade = fControll.currSniperPowerGrade;
        }

        Debug.Log("POWER : " + fControll.currRiflePowerGrade.ToString() + " , " + fControll.currShotPowerGrade.ToString() + " , " + fControll.currSniperPowerGrade.ToString() +
                    " AMMO : " + fControll.currRifleAmmoGrade.ToString() + " , " + fControll.currShotAmmoGrade.ToString() + " , " + fControll.currSniperAmmoGrade.ToString());
    }

    public void AmmoUpgrade(int number)
    {
        if (number == 0)
        {
            if (GameManager.instance.rifleAmmoGrade == 2) return;
            fControll.currRifleAmmoGrade += 1;
            GameManager.instance.rifleAmmoGrade = fControll.currRifleAmmoGrade;
            GameManager.instance.currentGold -= 150;
            GameManager.instance.UpdateCurrentGold();

            fControll.maxRifleAmmo += 10;
            fControll.UpdateAmmoInfo();
        }

        else if (number == 1)
        {
            if (GameManager.instance.shotAmmoGrade == 2) return;
            fControll.currShotAmmoGrade += 1;
            GameManager.instance.shotAmmoGrade = fControll.currShotAmmoGrade;
            GameManager.instance.currentGold -= 200;
            GameManager.instance.UpdateCurrentGold();

            fControll.maxShotAmmo += 2;
            fControll.UpdateAmmoInfo();
        }

        else if (number == 2)
        {
            if (GameManager.instance.sniperAmmoGrade == 2) return;
            fControll.currSniperAmmoGrade += 1;
            GameManager.instance.sniperAmmoGrade = fControll.currSniperAmmoGrade;
            GameManager.instance.currentGold -= 250;
            GameManager.instance.UpdateCurrentGold();

            fControll.maxSniperAmmo += 1;
            fControll.UpdateAmmoInfo();
        }

        Debug.Log("POWER : " + fControll.currRiflePowerGrade.ToString() + " , " + fControll.currShotPowerGrade.ToString() + " , " + fControll.currSniperPowerGrade.ToString() +
                    " AMMO : " + fControll.currRifleAmmoGrade.ToString() + " , " + fControll.currShotAmmoGrade.ToString() + " , " + fControll.currSniperAmmoGrade.ToString());
    }

}
