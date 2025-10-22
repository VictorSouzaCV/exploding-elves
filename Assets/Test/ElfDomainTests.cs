using System;
using NUnit.Framework;
using ExplodingElves.Core;

namespace ExplodingElves.Tests
{
    public class ElfDomainTests
    {
        private MockElfAdapter _mockElfAdapter;
        private MockElfData _mockElfData;
        private MockClockAdapter _mockClockAdapter;
        private ElfDomain _elfDomain;

        [SetUp]
        public void SetUp()
        {
            _mockElfAdapter = new MockElfAdapter();
            _mockElfData = new MockElfData();
            _mockClockAdapter = new MockClockAdapter();
            _mockClockAdapter.CurrentTime = 0.1f;
            _elfDomain = new ElfDomain(_mockElfAdapter, _mockElfData, _mockClockAdapter, 0f);
        }

        [Test]
        public void ElfDomain_WhenCreated_ShouldBeInMinorState()
        {
            Assert.That(_mockElfAdapter.LastStateShown, Is.EqualTo(ElfState.Minor));
        }

        [Test]
        public void ElfDomain_WhenYoung_ShouldNotBeAbleToBreed()
        {
            _mockClockAdapter.CurrentTime = 1f; // Less than ReadyToBreedAge (5f)
            Assert.That(_elfDomain.CanBreed, Is.False);
        }

        [Test]
        public void ElfDomain_WhenOldEnough_ShouldBeAbleToBreed()
        {
            _mockClockAdapter.CurrentTime = 10f; // More than ReadyToBreedAge (5f)
            _mockClockAdapter.OnTick?.Invoke(0.1f); // Trigger state calculation
            Assert.That(_elfDomain.CanBreed, Is.True);
        }

        [Test]
        public void ElfDomain_WhenExploded_ShouldNotBeAbleToExplode()
        {
            _elfDomain.Explode();
            Assert.That(_elfDomain.CanExplode, Is.False);
        }

        [Test]
        public void ElfDomain_WhenBecomeParent_ShouldBeTiredOfBreeding()
        {
            _mockClockAdapter.CurrentTime = 10f;
            _mockClockAdapter.OnTick?.Invoke(0.1f);
            _elfDomain.BecomeParent();
            Assert.That(_elfDomain.CanBreed, Is.False);
        }

        [Test]
        public void ElfDomain_WhenExploded_ShouldInvokeOnExplodeEvent()
        {
            bool eventFired = false;
            _elfDomain.OnExplode += (adapter) => eventFired = true;

            _elfDomain.Explode();

            Assert.That(eventFired, Is.True);
        }
    }

    public class MockElfAdapter : IElfAdapter
    {
        public (float x, float y) Position { get; private set; }
        public Action<IElfAdapter> OnHitElf { get; set; }
        public ElfState LastStateShown { get; private set; }
        public (float x, float y) LastMovement { get; private set; }

        public void ChangeMovement(float x, float y)
        {
            LastMovement = (x, y);
        }

        public void SetPosition(float x, float y)
        {
            Position = (x, y);
        }

        public void SetColor((float r, float g, float b, float a) color)
        {
        }

        public void ShowStateVisualChange(ElfState state)
        {
            LastStateShown = state;
        }
    }

    public class MockElfData : IElfData
    {
        public (float r, float g, float b, float a) Color => (1f, 0f, 0f, 1f);
        public ElfType ElfType => ElfType.Red;
        public float SpawnFrequency => 2f;
        public float Speed => 3f;
        public float ReadyToBreedAge => 5f;
        public float BreedCooldown => 10f;
        public float WanderFactor => 0.5f;
    }

    public class MockClockAdapter : IClockAdapter
    {
        public Action<float> OnTick { get; set; }
        public float CurrentTime { get; set; }
    }
}
