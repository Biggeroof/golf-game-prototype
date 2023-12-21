using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StrokeCounterUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI strokeText;

    private void Start()
    {
        GoalManager.Instance.OnGoalReached += Instance_OnGoalReached;
    }

    private void Instance_OnGoalReached(object sender, System.EventArgs e)
    {
        this.gameObject.SetActive(!this.enabled);
        this.enabled = !this.enabled;
    }

    void Update()
    {
        strokeText.text = StrokeCountManager.instance.getPar().ToString();
    }
}
