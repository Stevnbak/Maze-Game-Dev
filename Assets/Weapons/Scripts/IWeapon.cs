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
    float ammoMagTotal { get; set; }

    void Fire();
    void Reload();

    void Drop(Transform otherTrans);
}
