using System;
using UnityEngine;
using ExplodingElves.Core;

namespace ExplodingElves.Engine
{
    public class ElfBehaviour : MonoBehaviour, IElfAdapter
    {
        public Action<float> OnTick { get; set; }
        public Action<IElfAdapter> OnHitElf { get; set; }
        public void Move(float x, float y)
        {
            // TODO Move with rigidbody
            transform.position += new Vector3(x, 0, y);
        }

        void FixedUpdate()
        {
            OnTick?.Invoke(Time.time);
        }
    }
}
