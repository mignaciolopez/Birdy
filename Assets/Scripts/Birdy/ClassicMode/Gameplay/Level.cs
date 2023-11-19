using UnityEngine;
using Birdy.Shared.UI;
using System.Collections.Generic;
using Birdy.ClassicMode.Gameplay.UI;

namespace Birdy.ClassicMode.Gameplay
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private float _speed = 1.0f;
        [SerializeField] private float _spawnInterval = 2f;

        private Design _design;

        private List<GameObject> _pipeList;
        private SpriteRenderer _pipeBodyRenderer;
        
        private float _elapsedTime;

        private float _maxGapSize = 4f;
        private float _minGapSize = 2f;
        private float _gapSize = 4f;

        private void Awake()
        {
            _pipeList = new List<GameObject>();
        }

        private void Start()
        {
            _design = FindAnyObjectByType<Design>();
            _pipeBodyRenderer = GameAssets.Instance.PipeBody.GetComponent<SpriteRenderer>();

            _gapSize = _maxGapSize;
        }

        private void Update()
        {
            CleanPipes();
            HandlePipeMovement();
            HandlePipeSpawn();
        }

        private void HandlePipeSpawn()
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= _spawnInterval)
            {
                _elapsedTime -= _spawnInterval;
                SpawnPipe();

                _gapSize -= Time.deltaTime;
                _gapSize = Mathf.Clamp(_gapSize, _minGapSize, _maxGapSize);
            }
        }

        private void SpawnPipe()
        {
            float gapY = Random.Range(2.5f, 5.5f);
            CreateGapPipes(gapY, _gapSize, _design.ViewBorders.y + _pipeBodyRenderer.size.x);
        }

        private void CleanPipes()
        {
            for (int i = _pipeList.Count - 1; i >= 0; i--)
            {
                if (_pipeList[i].transform.position.x < _design.ViewBorders.x - _pipeBodyRenderer.size.x)
                {
                    Destroy(_pipeList[i]);
                    _pipeList.RemoveAt(i);
                }
            }
        }

        private void HandlePipeMovement()
        {
            foreach(var pipe in _pipeList)
            {
                var pos = pipe.transform.position;
                pos.x += -_speed * Time.deltaTime;
                pipe.transform.position = pos;
            }
        }

        private void CreateGapPipes(float gapY, float gapSize, float xPosition)
        {
            CreatePipe(Mathf.Abs(gapY - gapSize * .5f), xPosition);
            CreatePipe(Mathf.Abs(_design.ActiveCamera.orthographicSize * 2f - gapY - gapSize * .5f), xPosition, false);
            CreateGapCollider(gapY, gapSize, xPosition);
        }

        private void CreatePipe(float height, float xPosition, bool createBottom = true)
        {
            float isBottom = -1f;

            if (!createBottom)
                isBottom = 1f;

            //Body
            var pipeBody = Instantiate(UI.GameAssets.Instance.PipeBody);
            _pipeList.Add(pipeBody);

            pipeBody.transform.position = new Vector3
            {
                x = xPosition, 
                y = isBottom * _design.ActiveCamera.orthographicSize,
                z = 0
            };

            pipeBody.GetComponent<SpriteRenderer>().size = new Vector2
            {
                x = pipeBody.GetComponent<SpriteRenderer>().size.x,
                y = isBottom * -height
            };

            var pipeBodyBoxCollider = pipeBody.GetComponent<BoxCollider2D>();
            pipeBodyBoxCollider.size = new Vector2
            {
                x = pipeBody.GetComponent<SpriteRenderer>().size.x,
                y = height
            };

            pipeBodyBoxCollider.offset = new Vector2
            {
                x = 0f,
                y = isBottom * -height * 0.5f
            };

            //Head
            var pipeHead = Instantiate(UI.GameAssets.Instance.PipeHead);
            _pipeList.Add(pipeHead);
            pipeHead.transform.position = new Vector3
            {
                x = xPosition,
                y = isBottom * _design.ActiveCamera.orthographicSize + isBottom * -height + isBottom * (Mathf.Round(pipeHead.GetComponent<SpriteRenderer>().size.y * 10f) / 10f) / 2f,
                z = 0
            };
        }

        private void CreateGapCollider(float gapY, float gapSize, float xPosition)
        {
            //Gap
            var pipeGap = Instantiate(UI.GameAssets.Instance.PipeGap);
            _pipeList.Add(pipeGap);
            pipeGap.transform.position = new Vector3
            {
                x = xPosition,
                y = gapY - gapSize,
                z = 0
            };
            var pipeGapBoxCollider = pipeGap.GetComponent<BoxCollider2D>();
            pipeGapBoxCollider.size = new Vector2
            {
                x = 0.2f,
                y = gapSize
            };
            pipeGapBoxCollider.offset = new Vector2
            {
                x = -0.15f,
                y = 0f
            };
        }
    }
}