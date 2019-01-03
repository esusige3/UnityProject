using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingAtk : MonoBehaviour
{

    public Transform firePoint;
    public GameObject bullet;//bullet prefabs
    private WaitForSeconds fireGap = new WaitForSeconds(0.3f);
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(Attackrootin());
    }
    
    IEnumerator Attackrootin()
    {
        while (true) {

            if (Input.GetKeyDown(KeyCode.X))
            {
                Shoot();
                yield return fireGap;
            }
            yield return null;
        }
    }
    void Shoot()
    {
        Instantiate(bullet, firePoint.position, firePoint.rotation);
    }
}
