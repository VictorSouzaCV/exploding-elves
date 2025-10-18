using System;
using UnityEngine;
using ExplodingElves.Core;

namespace ExplodingElves.Engine
{
    public class ElfBehaviour : MonoBehaviour, IElfAdapter
    {
        public Action<float> OnTick { get; set; }
        public Action<IElfAdapter> OnHitElf { get; set; }
        [SerializeField] private Rigidbody2D _rigidbody;

        public void Move(float x, float y)
        {
            _rigidbody.velocity = new Vector2(x, y);
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
            Destroy(gameObject);
        }
    }
}
