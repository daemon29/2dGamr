﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CurrentScore : MonoBehaviour {
    Text score;
    void OnEnable()
    {
        score = GetComponent<Text>();
        score.text = "Your Score: " + GameManager.Instance.score;
    }
}
