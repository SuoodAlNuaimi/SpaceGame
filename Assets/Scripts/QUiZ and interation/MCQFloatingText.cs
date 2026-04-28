using UnityEngine;
using TMPro;
using System.Collections;

namespace QuizSystem
{
    public class MCQFloatingText : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float moveSpeed = 50f;
        [SerializeField] private float fadeSpeed = 2f;
        [SerializeField] private float lifeTime = 1.5f;
        [SerializeField] private Vector3 startScale = new Vector3(0.5f, 0.5f, 0.5f);
        [SerializeField] private Vector3 targetScale = Vector3.one;
        [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private TextMeshProUGUI textMesh;
        private float timer;

        public void Setup(string text, Color color)
        {
            textMesh = GetComponent<TextMeshProUGUI>();
            if (textMesh != null)
            {
                textMesh.text = text;
                textMesh.color = color;
            }
            
            transform.localScale = startScale;
            StartCoroutine(Animate());
        }

        private IEnumerator Animate()
        {
            float elapsed = 0;
            Color startColor = textMesh != null ? textMesh.color : Color.white;

            while (elapsed < lifeTime)
            {
                elapsed += Time.deltaTime;
                float normalizedTime = elapsed / lifeTime;

                // Move upward
                transform.position += Vector3.up * moveSpeed * Time.deltaTime;

                // Scale pop
                if (normalizedTime < 0.2f)
                {
                    transform.localScale = Vector3.Lerp(startScale, targetScale * 1.2f, normalizedTime / 0.2f);
                }
                else if (normalizedTime < 0.4f)
                {
                    transform.localScale = Vector3.Lerp(targetScale * 1.2f, targetScale, (normalizedTime - 0.2f) / 0.2f);
                }

                // Fade out
                if (textMesh != null)
                {
                    float alpha = Mathf.Lerp(1, 0, (normalizedTime - 0.5f) / 0.5f);
                    textMesh.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Clamp01(alpha));
                }

                yield return null;
            }

            Destroy(gameObject);
        }
    }
}
