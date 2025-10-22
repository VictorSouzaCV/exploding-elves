# Exploding Elves

## Overview

- **Breeding**: Elves of the same color can breed when they collide
    - To prevent stack overflow, I've implemented some extra rules
    1) Minimum age for breeding
    2) Cooldown after breeding
- **Exploding**: Elves of different colors explode when they collide

## Editor Tool
- **Selection History**: Simple editor extension for quickly navigate between object selections

## Architecture

- **Clean Architecture**: Separation between Core domain logic and Unity Engine implementation
- **Domain-Driven Design**: Clear separation of concerns with dedicated domains for Elf, ElfSpawner, and ElfHit
- **Adapter Pattern**: Interface-based design allowing for easy testing and modularity
- **Event-Driven**: Decoupled components communicating through events and actions

```
Assets/
├── Core/                          # Domain logic (framework-agnostic)
│   ├── Clock/                     # Time management interfaces
│   ├── Elf/                       # Elf domain logic and states
│   ├── ElfHit/                    # Collision and interaction handling
│   └── ElfSpawner/                # Elf spawning logic
└── Engine/                        # Unity-specific implementations
    ├── Arena/                     # Game environment
    ├── Elf/                       # Unity elf behaviors and prefabs
    ├── ElfSpawner/                # Unity spawner implementations
    └── Scenes/                    # Game scenes
```

## Optimizations

- **Separate Assemblies**: Domain, engine and tests scripts are split in different assembly definitions to speed up compilation time and enforce separation of concerns

### Object Pooling

- **Unity Object Pool**: ElfSpawnerBehaviour uses Unity's built-in ObjectPool system to reuse elf GameObjects

### Physics Optimization

- **Rigidbody2D Movement**: Direct velocity manipulation instead of transform changes for better physics performance
- **Collision Matrix**: Elf x Elf / Elf x Wall
- **Axis-Aligned Bounding Box colliders**: All colliders are rectangular shape with fixed rotation, which results in cheaper collision calculation
- ** 
- **OnCollisionStay2D**: Continuous collision detection optimized for multiple simultaneous interactions
- **Component Caching**: All Unity components referenced via SerializeField to avoid GetComponent calls

### Rendering Optimization
- **Lighting**: disabled as all visuals are 2D with unlit native shaders
- **Shadows**: disabled
- **POT**: All sprites are POT (power of two resolution)
- **Sprite Atlas**: global sprite atlas to reduce draw calls 

### Architecture Benefits

- **Domain Separation**: Core logic runs independently of Unity's update cycle, reducing MonoBehaviour overhead

## Core Components

### ElfDomain

Handles individual elf behavior including movement, state transitions, and breeding logic.

### ElfSpawnerDomain

Manages automatic elf spawning based on configured frequencies and handles elf lifecycle events.

### ElfHit

Coordinates interactions between elves, handling both breeding and explosion scenarios.

### SceneOrchestrator

Unity MonoBehaviour that orchestrates the simulation initialization by injecting dependencies and managing time.
