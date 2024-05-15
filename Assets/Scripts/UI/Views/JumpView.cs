using System.Collections.Generic;
using CliffJump.Data;
using TMPro;
using UnityEngine;

namespace CliffJump.UI.Views
{
    public class JumpView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Canvas canvas;
        [SerializeField] private TMP_Text textPrefab;

        [Header("Spacing")]
        [SerializeField] private Vector2 origin;
        [SerializeField] private Vector2 offset;
        
        private readonly List<TMP_Text> currentPrefabs = new();

        public void ClearView()
        {
            foreach (var tmp in currentPrefabs)
                Destroy(tmp.gameObject);

            currentPrefabs.Clear();
        }

        public void AddQTELabel(QTEAction action)
        {
            var tmp = Instantiate(textPrefab, canvas.transform);
            tmp.text = action.Label;

            tmp.transform.localPosition = origin + (offset * currentPrefabs.Count);
            
            currentPrefabs.Add(tmp);
        }
    }
}