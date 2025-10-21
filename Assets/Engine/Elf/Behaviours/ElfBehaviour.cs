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
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private Image _minorElf;
        [SerializeField] private Image _grownUpElf;
        [SerializeField] private Image _tiredImage;
        [SerializeField] private Image _explosionImage;

        public (float x, float y) Position => new(transform.position.x, transform.position.y);
        WaitForSeconds _explosionDuration = new WaitForSeconds(0.25f);

        public void ChangeMovement(float x, float y)
        {
            _rigidbody.velocity = new Vector2(x, y);
        }

        public void SetPosition(float x, float y)
        {
            transform.position = new Vector2(x, y);
        }

        public void SetColor((float r, float g, float b, float a) color)
        {
            var newColor = new Color(color.r, color.g, color.b, color.a);
            _minorElf.color = newColor;
            _grownUpElf.color = newColor;
            _explosionImage.color = newColor;
        }

        void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Elf"))
            {
                OnHitElf?.Invoke(collision.gameObject.GetComponent<ElfBehaviour>());
            }
        }

        private void HideAllStateVisuals()
        {
            _minorElf.enabled = false;
            _grownUpElf.enabled = false;
            _tiredImage.enabled = false;
            _explosionImage.enabled = false;
        }

        public void ShowStateVisualChange(ElfState state)
        {
            HideAllStateVisuals();
            switch (state)
            {
                case ElfState.Minor:
                    _minorElf.enabled = true;
                    break;
                case ElfState.ReadyToBreed:
                    _grownUpElf.enabled = true;
                    break;
                case ElfState.TiredOfBreeding:
                    _grownUpElf.enabled = true;
                    _tiredImage.enabled = true;
                    break;
                case ElfState.Exploded:
                    StartCoroutine(ExplodeCoroutine());
                    break;
            }
        }

        private IEnumerator ExplodeCoroutine()
        {
            _explosionImage.enabled = true;
            yield return _explosionDuration;
            OnExplode?.Invoke(this);
        }
    }
}
