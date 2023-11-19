using UnityEngine;
using TMPro;

namespace Birdy.ClassicMode.Gameplay.UI
{
    public class ScoreWindow : MonoBehaviour
    {
        private TextMeshProUGUI _scoreText;

        private int _score;

        private void Awake()
        {
            _score = 0;

            _scoreText = transform.Find("ScoreText").GetComponentInChildren<TextMeshProUGUI>();
            _scoreText.text = _score.ToString();

            PipeGap.OnPipePassed += PipeGap_OnPipePassed;
        }

        private void OnDestroy()
        {
            PipeGap.OnPipePassed -= PipeGap_OnPipePassed;
        }

        private void PipeGap_OnPipePassed(int scoreIncrement)
        {
            _score += scoreIncrement;
            _scoreText.text = _score.ToString();
        }
    }
}