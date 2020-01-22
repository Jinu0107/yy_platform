using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager instance;
    public GameObject bulletPrefab;

    private List<Bullet> bulletList = new List<Bullet>();

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("다수의 불렛매니저 인스턴스가 실행중");
        }
        instance = this;
    }

    public Bullet GetOrCreate()
    {
        Bullet b = bulletList.Find(o => o.gameObject.activeSelf == false);

        //리스트에 총알이 없다면
        if (!b)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            b = bullet.GetComponent<Bullet>();
            bulletList.Add(b);
        }else
        {
            //있다면 그 총알을 반환
            b.gameObject.SetActive(true);
        }
        return b;
    }

}
