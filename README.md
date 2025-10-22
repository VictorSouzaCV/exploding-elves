# Exploding Elves

## Overview

● Four spawners, each one generates one type of elf (black, red, white, blue).

● Elves move randomly in the scene.

● When two elves of the same color collide, they spawn one additional elf of that type ([see Challenges section](#challenges))

● When elves of different colors collide, both explode.

● The player can change the interval at which each spawner generates
elves.

## Structure

- **Clean Architecture**: Separation between Core domain logic and Unity Engine implementation
- **Domain-Driven Design**: Clear separation of concerns with dedicated domains for Elf, ElfSpawner, and ElfHit
- **Adapter Pattern**: Interface-based design allowing for easy testing and modularity
- **Event-Driven**: Decoupled components communicating through events and actions
- **Separate Assemblies**: Domain, engine, tests scripts and third-party packages are split in different assembly definitions to speed up compilation time and enforce separation of concerns

```
Assets/
├── Core/                          # Domain logic (framework-agnostic)
├── Engine/                        # Unity-specific implementations
├── Packages/                      # Third-party imports
└── Tests/                         # Unit tests for domain logic
```

## Optimizations

### Memory

- **Object Pool**: ElfSpawnerBehaviour uses Unity's built-in ObjectPool system to reuse elf GameObjects

### Physics

- **Collision Matrix**: Elf x Elf / Elf x Wall
- **Axis-Aligned Bounding Box Colliders**: colliders with rectangular shape with fixed rotation
- **Fixed Update**: all events are bound to FixedUpdate. Besides, only one MonoBehaviour has the event method (SceneOrchestrator : IClockAdapter).
- **Raycast Target**: the only Images with enabled raycasting are the Sliders that adjust the frequency of elves spawn.

### Rendering

- **Lighting**: disabled as all visuals are 2D with unlit native shaders
- **Shadows**: disabled
- **POT**: All sprites are POT (power of two resolution)
- **Sprite Atlas**: global sprite atlas (512x512) to reduce draw calls
- **Texture import settings**: sprites have reasonable max size and proper import flags

### Architecture Benefits

- **Domain Separation**: Core logic runs independently of Unity's update cycle, reducing MonoBehaviour overhead

## Editor Tool

- **Selection History**: simple editor extension for quickly navigate between object selections

## Tests

- Uses Moq and NUnit in editor unit tests that mock adapters and make assertions to validate some of the core domain logic.

## Challenges

- **Breeding Stack Overflow**: in order to prevent infinite recursion during elves reprodution, some adaptations were made to limit the frequency of breeding:

  1. Elves have a minimum age for breeding. This is state is represented visually by a shorter elf.
  2. Elves get tired of breeding and must wait some time before breeding again. This is state is represented visually by a sweat sprite on the elf's face.

  The time until maturity, the cooldown after breeding, and other parameters are all adjustable via each elf ScriptableObject (ElfConfig : IElfData)

