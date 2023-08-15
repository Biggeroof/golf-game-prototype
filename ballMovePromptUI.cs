using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballMovePromptUI : MonoBehaviour
{
    [SerializeField]
    private AimLine aimLine;

    // Start is called before the first frame update
    void Start()
    {
        aimLine.onToggleBallCanShoot += AimLine_onToggleBallCanShoot1;
    }

    private void AimLine_onToggleBallCanShoot1(object sender, AimLine.OnToggleBallCanShootArgs e)
    {
        gameObject.SetActive(e.canShoot);
    }
}
