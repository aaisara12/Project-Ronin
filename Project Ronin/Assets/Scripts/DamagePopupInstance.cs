using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopupInstance : MonoBehaviour
{
    private static DamagePopupInstance _i;
    public static DamagePopupInstance i{
        get {
            if(_i == null) 
                _i = Instantiate(Resources.Load<DamagePopupInstance>("DamagePopupInstance"));
            return _i;
        }
    }
    public Transform DmgPopup;
}
