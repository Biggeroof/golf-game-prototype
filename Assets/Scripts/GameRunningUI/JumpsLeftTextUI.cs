using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JumpsLeftTextUI : MonoBehaviour
{
    [SerializeField]
    BallLogic ball;

    [SerializeField]
    TextMeshProUGUI jumpsLeftText;

    private void Start()
    {
        GoalManager.Instance.OnGoalReached += GoalReached;
    }

    private void GoalReached(object sender, System.EventArgs e)
    {
        this.gameObject.SetActive(!this.enabled);
        this.enabled = !this.enabled;
    }

    // Update is called once per frame
    void Update()
    {
        jumpsLeftText.text = ball.getJumpsLeft().ToString();
    }
}
