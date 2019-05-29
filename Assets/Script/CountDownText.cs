using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Text))]
public class CountDownText : MonoBehaviour {
    Text CountDown;
    public delegate void CountdownFinished();
    public static event CountdownFinished OnCountdownFinished;
     void OnEnable()
    {
        CountDown = GetComponent<Text>();
        CountDown.text = "3";
        StartCoroutine("Countdown");
    }
    IEnumerator Countdown()
    {
        int count = 3; 
        for(int i = 0; i< count; i++)
        {
            CountDown.text = (count - i).ToString();
            yield return new WaitForSeconds(1);
        }
        OnCountdownFinished();
    }
}
