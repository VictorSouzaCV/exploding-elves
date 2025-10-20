using System;
using UnityEngine;
using UnityEngine.UI;
using ExplodingElves.Core;

namespace ExplodingElves.Engine
{
    public class ElfBehaviour : MonoBehaviour, IElfAdapter
    {
        public Action<float> OnTick { get; set; }
        public Action<IElfAdapter> OnHitElf { get; set; }
        public Action<IElfAdapter> OnExplode { get; set; } 
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Image _image;


        public void Move(float x, float y)
        {
            _rigidbody.velocity = new Vector2(x, y);
        }

        public void SetColor((float r, float g, float b, float a) color)
        {
            _image.color = new Color(color.r, color.g, color.b, color.a);
        }

        void FixedUpdate()
        {
            OnTick?.Invoke(Time.time);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Elf"))
            {
                OnHitElf?.Invoke(collision.gameObject.GetComponent<ElfBehaviour>());
            }
        }

        public void Explode()
        {
            OnExplode?.Invoke(this);
        }
    }
}
