using UnityEngine;
public interface IWeapon
{
    void Initialize();
    bool isFiring { get; set; }
    bool isReloading { get; set; }
    bool isADSing { get; set; }
    float fireTime { get; set; }
    float ammoInMag { get; set; }
    float ammoTotal { get; set; }

    void Fire();

    void Drop(Transform otherTrans);
}
