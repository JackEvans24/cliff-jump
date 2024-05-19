using System.Collections.Generic;
using CliffJump.Data;
using UnityEngine;

namespace CliffJump.UI.Views
{
    public class JumpView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private QTEDisplay qteDisplay;

        [Header("Spacing")]
        [SerializeField] private Vector2 origin;
        [SerializeField] private Vector2 offset;
        
        private readonly List<QTEDisplay> currentPrefabs = new();

        private int currentQteIndex;

        public void ClearView()
        {
            foreach (var tmp in currentPrefabs)
                Destroy(tmp.gameObject);

            currentPrefabs.Clear();
            currentQteIndex = 0;
        }

        public void AddQTELabel(QTEAction action)
        {
            var qte = Instantiate(qteDisplay, transform);
            qte.Initialise(action.Direction);
            qte.SetActive(currentPrefabs.Count == 0);

            qte.transform.localPosition = origin + (offset * currentPrefabs.Count);
            
            currentPrefabs.Add(qte);
        }

        public void UpdateActiveLabel()
        {
            currentQteIndex++;
            for (int i = 0; i < currentPrefabs.Count; i++)
            {
                var qteLabel = currentPrefabs[i];
                qteLabel.SetActive(i == currentQteIndex);
            }
        }
    }
}