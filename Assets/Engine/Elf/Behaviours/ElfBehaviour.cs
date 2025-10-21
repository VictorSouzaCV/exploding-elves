using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ExplodingElves.Core;

namespace ExplodingElves.Engine
{
    public class ElfBehaviour : MonoBehaviour, IElfAdapter
    {
        public Action<IElfAdapter> OnHitElf { get; set; }
        public Action<IElfAdapter> OnExplode { get; set; } 
        public Action OnHitWall { get; set; }
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _normalSprite;
        [SerializeField] private Sprite _explosionSprite;

        public (float x, float y) Position => new (transform.position.x, transform.position.y);
        WaitForSeconds _explosionDuration = new WaitForSeconds(0.25f);

        public void Move(float x, float y)
        {
            _rigidbody.velocity = new Vector2(x, y);
        }

        public void SetColor((float r, float g, float b, float a) color)
        {
            _image.color = new Color(color.r, color.g, color.b, color.a);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Elf"))
            {
                OnHitElf?.Invoke(collision.gameObject.GetComponent<ElfBehaviour>());
            }

            if (collision.gameObject.CompareTag("Wall"))
            {
                OnHitWall?.Invoke();
            }
        }

        public void Explode()
        {
            StartCoroutine(ExplodeCoroutine());
        }

        private IEnumerator ExplodeCoroutine()
        {
            _image.sprite = _explosionSprite;
            yield return _explosionDuration;
            _image.sprite = _normalSprite;
            OnExplode?.Invoke(this);
        }
    }
}
