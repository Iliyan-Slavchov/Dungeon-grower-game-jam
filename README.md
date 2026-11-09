# Tale of the Baumkönig

**A 2D, wave-based dungeon-defence game built in Unity and C# for the UWE Starting Block Game Jam 2026.**

**[Download the Windows build (V10)](https://puxceto.itch.io/tale-of-the-baumknig)** · **[Technical code review](#technical-code-review)** · **[Technical retrospective](#technical-retrospective)**

![A gameplay screenshot showing the dungeon, defensive towers, adventurers and Tree King](GameplayImage.png)

| | |
| --- | --- |
| **My role** | Project Lead, Game Designer & Sole Unity Developer |
| **Team** | Five people: one developer and four creative contributors |
| **Event** | UWE Starting Block Game Jam 2026 (one week) |
| **My time investment** | Approximately 20 hours of development/planning and 10 hours of team coordination |
| **Technology** | Unity `6000.6.2f1`, C#, Unity Input System, 2D physics, Unity UI and TextMeshPro |
| **Release** | Windows V10 game-jam build; original project preserved for review |

## The game

In *Tale of the Baumkönig*, you defend the Tree King against adventurers invading his dungeon. Instead of playing the adventurer, you place defensive towers, earn gold by defeating attackers, and push the Tree King farther from the entrance as the dungeon expands.

The game opens with an illustrated, voiced introduction. In the main level, adventurers advance through the dungeon, damaging towers and the Tree King. The player places one of four tower types using the number keys and spends gold earned from completed waves.

Clearing a wave reveals another room and moves the Tree King farther from the entrance, up to the dungeon's room limit. Wave 10 introduces a story cutscene and the Adventurer King. After defeating that boss, play continues in **endless mode**, with a boss encounter and distinct boss music every ten waves. The introductory boss cutscene does not repeat. When the Tree King dies, the player can restart without replaying the opening story.

![A short recording of tower placement, combat and wave progression](GameplayVideo.gif)

## What I owned

I was the **project lead, gameplay designer and only person using Unity** on a team with separate contributors for art, music/sound effects, voice acting and writing. The artist drew the level's visual design; I developed its gameplay layout and assembled it in Unity, incorporating feedback from the team.

My responsibilities included:

- **Engineering:** all C# gameplay code; enemy and tower behaviour; combat; wave and boss progression; resource economy; input; UI; narrative integration; camera controls; and build production.
- **Unity implementation:** scene construction, prefabs, level assembly, animation setup, and integration of art, audio, voice acting and story assets.
- **Game design and QA:** gameplay decisions, balancing, playtesting, debugging and deciding which issues to address before release.
- **Leadership and delivery:** translating rough initial ideas into a workable game, assigning and coordinating creative tasks, managing scope and integrating work from all five contributors.

Creative work and feedback were collaborative; the artwork, music/sound effects, performance and writing are credited individually below.

## Delivery under the jam constraints

The jam lasted one week during the start of the university year. My approximately **30 hours of work** were split between development/planning (~20 hours) and team coordination (~10 hours). Requirements evolved as development progressed; narrative sequences and some presentation features were added after the original gameplay plan.

The goal was a **complete, demonstrable game**, not a production-ready codebase. The V10 build implemented the intended progression from the introduction through the first boss encounter and into endless mode. During the jam showcase, it was played on my computer, teammates' computers and university machines without any known progression-blocking bugs in those tests. Verification was primarily manual playtesting rather than an automated test suite.

Late work included the controls and credits menus, alongside integration and build fixes. When time became scarce, I prioritised problems that prevented playing or completing the experience over non-blocking defects. That decision enabled delivery, but left gameplay correctness and maintainability issues documented below.

The [repository history](https://github.com/Iliyan-Slavchov/Dungeon-grower-game-jam/commits/main/) shows this progression, including a [bug-fixing commit](https://github.com/Iliyan-Slavchov/Dungeon-grower-game-jam/commit/7bb2defbb5c82f31c6a4915fd7161afbc6bf874d) and the [final build and integration commit](https://github.com/Iliyan-Slavchov/Dungeon-grower-game-jam/commit/0968e1f7c9feb08334a32e2ea88b6fdc6c37796c).

## Technical code review

If you want to review the code, these files are a good starting point. They contain the core gameplay logic and show where I would make changes if I worked on the project again.

| Start here | What to inspect | What it demonstrates |
| --- | --- | --- |
| [`WaveManager.cs`](Assets/Managers/WaveManager.cs) | Spawning, enemy-death event subscriptions, wave completion, coroutine-based transitions, rewards, room progression and music changes | Enemy tracking, wave transitions and communication between scripts |
| [`TowerSpawningManager.cs`](Assets/Managers/TowerSpawningManager.cs) | Input actions, mouse screen-to-world conversion, bounds and gold checks, prefab instantiation | Input handling, gold checks and tower spawning |
| [`Adventurer.cs`](Assets/Entities/Adventurer.cs), [`Tower.cs`](Assets/Entities/Tower.cs), [`TreeKing.cs`](Assets/Entities/TreeKing.cs) | Physics triggers, combat damage, health and death handling | 2D collision triggers, health and death handling, and a known combat bug |
| [`BossBattleStoryManager.cs`](Assets/Managers/BossBattleStoryManager.cs), [`StartStoryManager.cs`](Assets/Managers/StartStoryManager.cs) | Image and voice-line sequencing, dialogue input, gameplay interruption and return | Displaying cutscenes and returning to gameplay |
| [`CameraMovement.cs`](Assets/Camera%20Movement/CameraMovement.cs) | Pointer-relative orthographic zoom, panning, camera bounds and aspect-ratio handling | Camera panning, zoom calculations and aspect-ratio handling |

### Wave progression and state coordination

[`WaveManager`](Assets/Managers/WaveManager.cs) instantiates adventurers and subscribes to their `Died` events; the handler unsubscribes and decrements the active-enemy count. Once all enemies for a wave are defeated, a coroutine sequences the next phase, including feedback, room reveal, Tree King movement/growth, gold rewards and the next wave. Every tenth wave adds a boss and switches music.

This was a quick way to connect the parts of the game. The downside is that the script handles several unrelated tasks, and some wave and animation decisions are hardcoded.

### Tower placement and economy

[`TowerSpawningManager`](Assets/Managers/TowerSpawningManager.cs) uses the Unity Input System to handle four tower-placement actions. It converts the pointer location to world space, checks placement bounds and available gold, creates the selected tower prefab, then updates the UI.

I initially expected one or two tower types, so separate conditionals were quick to implement and easy to follow. Even at four types, I believe that simplicity was justified for a one-week prototype. The duplication would become more costly if further tower types or new placement rules were introduced; at that point, I would consolidate the shared spawning logic instead of adding more branches.

### Combat and entity lifecycle

[`Adventurer`](Assets/Entities/Adventurer.cs) exposes a C# death event so wave management does not have to continuously search for surviving enemies. Combat interactions use Unity 2D trigger callbacks and entity health methods.

A known defect is that damage occurs during each `OnTriggerStay2D` physics callback, rather than on explicit attack intervals. Combined with inadequate death-state guards and a lack of tower-overlap checks, this allowed unintentionally rapid damage when towers were placed inside enemies. This did **not** block the jam's intended progression, but it affected combat behaviour and balance.

### Narrative and presentation

The introduction and first boss encounter use illustrations, recorded lines, UI and dialogue input. The boss narrative is played once before wave 10 resumes, while later boss waves continue without replaying it. Separate image and voice-line arrays were convenient to wire in the Inspector, but rely on matching indices and lengths.

[`CameraMovement`](Assets/Camera%20Movement/CameraMovement.cs) supports drag-to-pan, mouse-wheel zoom around the pointer and viewport constraints. The zoom code keeps the world position under the pointer stable as the camera zooms, but playtests suggested that players rarely used the feature.

## Technical retrospective

This is the **original game-jam implementation**, not a refactored version produced later for a portfolio. The limitations below are part of the work I delivered. They are included to show how I now analyse its technical decisions.

| Area | What I did during the jam | What I would change today |
| --- | --- | --- |
| **Combat timing and death** | Applied damage every physics tick and had no consistent one-time death transition | Track attack cooldowns independently of trigger callbacks; implement an explicit alive/dead state; disable collision/damage behaviour after death; reject invalid tower overlaps |
| **Tower extensibility** | Used direct conditional branches, a proportionate choice for four tower types under a short deadline | Keep the simple approach for this prototype; if the roster or placement rules expand, extract shared validation, payment and spawning into one reusable workflow. Consider data-driven tower definitions only if the additional configuration warrants them |
| **Cutscene data** | Wired parallel Inspector arrays of images and voice lines as the only person editing the Unity scenes | If narrative content grows, pair each image and voice line in one dialogue entry. Add editor-time reference validation when multiple people edit that content or configuration errors become costly |
| **Game progression** | Coordinated waves, rooms, animations, rewards and audio in `WaveManager`, while other managers maintained their own room counters | Establish a clear owner for progression state from the outset, and separate responsibilities before a single MonoBehaviour grows into a tightly coupled manager |
| **Camera scope** | Implemented pan and zoom in addition to aspect-ratio handling | Validate whether navigation controls are needed for the level framing; remove features that do not materially improve play, retaining viewport handling where useful |

### What I learned about scope and quality

I spent considerable time planning, but I was working from rough ideas rather than a stable specification. I made assumptions about the scope of individual systems that did not always hold as the game developed. When requirements changed, I often extended the existing implementation because delivering and stabilising the playable build took priority over restructuring code that already worked.

As the only person using Unity, I did not add extensive validation for Inspector references and other editor configuration. At the time, a missing or incorrect reference was usually straightforward for me to find and fix, so I focused on finishing the gameplay instead. If other developers or designers were editing those systems, or if mistakes became harder to diagnose, I would add more checks. That reasoning does **not** apply to gameplay bugs such as damage timing, death handling or invalid tower placement: these affect the player regardless of who set up the project.

For a similar project today, I would establish clearer ownership of shared gameplay state and keep responsibilities separated before a large, tightly coupled MonoBehaviour develops. I would also start a small, repeatable regression checklist as soon as the core gameplay loop worked, running it after changes to combat, wave progression, cutscenes and menus rather than relying mainly on testing at the end. Earlier integration checkpoints and a short, severity-ranked bug list would help expose risky interactions sooner.

I would still favour the simplest implementation that fits the actual scope. Refactoring duplicated tower placement, introducing data-driven assets or adding editor tooling should be driven by a real increase in content, collaborators or maintenance cost rather than by a desire to add architecture to a small prototype.

**Delivery trade-off:** Prioritising a playable showcase build was appropriate for the jam deadline. Some known combat defects remained because they were less urgent than issues affecting completion or build stability; this did not make them harmless. With more time, I would address those correctness issues and verify the fixes through regression testing.

## Play the game

The **Windows V10 build** is available on [itch.io](https://puxceto.itch.io/tale-of-the-baumknig):

1. Download `V10.zip`.
2. Extract the archive fully.
3. Run `Tale of the baumkönig.exe`.

The same original build is also committed under [`Latest Build/V10`](Latest%20Build/V10), but the itch.io ZIP is the simplest way to play it.

### Controls

| Input | Action |
| --- | --- |
| `1` / `2` / `3` / `4` | Place a green, flying, mushroom or zombie tower at the pointer (if affordable and within placement bounds) |
| `Space` | Advance cutscene dialogue |
| Right mouse button + drag | Pan camera |
| Mouse wheel | Zoom camera |
| `Tab` | Toggle credits menu |
| `H` | Toggle controls menu |
| `Esc` | Quit the application |

The defeat screen also provides a restart button that returns to gameplay without replaying the introduction.

## Open the Unity project

**Editor version:** `6000.6.2f1` (as recorded in [`ProjectVersion.txt`](ProjectSettings/ProjectVersion.txt)).

The repository uses **Git LFS** for binary assets. Install Git LFS before cloning, then run:

```bash
git lfs install
git clone https://github.com/Iliyan-Slavchov/Dungeon-grower-game-jam.git
cd Dungeon-grower-game-jam
git lfs pull
```

Open the cloned folder as a Unity project through Unity Hub using the editor version above. The primary scenes are [`StartScene`](Assets/Scenes/StartScene.unity) and [`Level1`](Assets/Scenes/Level1.unity). Open `StartScene` to follow the intended flow, or inspect `Level1` directly for gameplay systems.

The repository contains source assets, scenes, prefabs, input actions and project settings. The published V10 build was tested during the jam. For a source build, use the included project settings and select the intended scenes in Unity's build configuration.

## Team credits

Created by a five-person team for **UWE Starting Block Game Jam 2026**:

| Contributor | Credit |
| --- | --- |
| **Iliyan Slavchov** | Project Lead, Game Design & Unity Development |
| **Harrison** | Art |
| **Yasandu Kalansooriya** | Music Composition & Sound Effects |
| **Victor Andron** | Voice Acting |
| **Finn Fagan** | Writing & Story |

The team contributed feedback and creative direction; I was responsible for coordinating production and implementing the playable Unity game. See the [original itch.io submission](https://puxceto.itch.io/tale-of-the-baumknig) for the public release and credits.

## AI assistance disclosure

A small amount of AI-assisted code examples and debugging support was used during development, as disclosed on the original itch.io page. I remained responsible for technical decisions, writing and integrating the game's systems, testing, project leadership and final delivery. The creative contributors supplied the game's art, audio, voice performance and writing.

## Rights and reuse

This repository does not currently include a software or asset licence. Public access to the source and game build does **not** grant permission to reuse the team's code, artwork, music, sound effects, voice performances or writing. Please obtain permission from the relevant contributors before reuse.
