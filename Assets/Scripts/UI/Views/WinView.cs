﻿using CliffJump.Data;
using TMPro;
using UnityEngine;

namespace CliffJump.UI.Views
{
    public class WinView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text runLabel;
        [SerializeField] private TMP_Text jumpLabel;
        [SerializeField] private TMP_Text diveLabel;
        [SerializeField] private TMP_Text finalScoreLabel;
        
        public void SetResults(GameResult result)
        {
            runLabel.text = result.RunSpeed.ToString("0.00");
            jumpLabel.text = result.JumpScore.ToString("0.00");
            diveLabel.text = result.DiveAngle.ToString("0.00");

            finalScoreLabel.text = result.FinalScore.ToString("0.00");
        }
    }
}