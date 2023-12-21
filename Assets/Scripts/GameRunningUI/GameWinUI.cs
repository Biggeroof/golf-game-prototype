using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameWinUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI strokeText;

    // Start is called before the first frame update
    void Start()
    {
        GoalManager.Instance.OnGoalReached += Instance_OnGoalReached;
        this.gameObject.SetActive(false);
    }

    private void Instance_OnGoalReached(object sender, System.EventArgs e)
    {
        strokeText.text = StrokeCountManager.instance.getPar().ToString();
        this.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update() { }
}
