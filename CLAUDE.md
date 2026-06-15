# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

A Unity 2022.3.67f2 (LTS) project implementing a turret training/shooting range simulation. Uses the **Universal Render Pipeline (URP 14.0.12)**. Two scenes: `MainMenu` and `GameScene`.

## Working with the Project

This is a Unity project — there are no CLI build or test commands. All development happens through the Unity Editor:

- Open the project in Unity Hub using Unity **2022.3.67f2**
- Main game scene: `Assets/GameScene.unity`
- Play in the Editor to test; mouse or arrow keys aim, LMB/Space shoots
- Both "MainMenu" and "GameScene" must be in Build Settings (in that order) for `SceneManager.LoadScene` to work

## Architecture

All C# scripts live in `Assets/TurretTraining/Scripts/` under the `TurretTraining` namespace.

### Component relationships

**Turret GameObject** requires `TurretStructure`, `TurretAimLaser`, and `TurretShooter` on the same object:
- `TurretStructure` — exposes `YawPivot`, `PitchPivot`, and `Muzzle` transforms; handles arrow-key + mouse input; serialized `_keyboardSpeed`, `_mouseSensitivity`, `_invertMouseY`; locks cursor in `Start()`; Escape toggles cursor lock
- `TurretAimLaser` — fetches the `Muzzle` from the co-located `TurretStructure` in `Start()`; fires a `Physics.Raycast` from the muzzle each frame and updates a `LineRenderer`
- `TurretShooter` — reads LMB/Space input, raycasts from `Muzzle`, calls `TargetBoard.RegisterHit()` on a hit; fires `ShotFired` event; plays shoot `AudioClip` via `AudioSource` and triggers a `ParticleSystem` muzzle flash; disabling this component stops all shooting

**Target hierarchy**: a `Target` component holds a reference to a child `TargetBoard`:
- `Target` — subscribes to `_board.HitDetected` in `Awake`; on board hit: plays `_hitEffect` ParticleSystem + `_hitClip` audio, calls `Toggle(false)`, fires its own `HitDetected` event. **`_hitEffect` must be on the Target root, not a child of `_board`'s GameObject, or it will be deactivated before it finishes playing.**
- `TargetBoard` — the actual hit zone; bridges a serialized `UnityEvent _hit` (wirable in the Inspector) to a C# `HitDetected` event; `RegisterHit()` invokes `_hit`

**Session layer**:
- `SessionManager` — call `BeginSession(config)` to start or restart a session in-place (no scene reload needed); `Cleanup()` unsubscribes the previous session's events before re-wiring; tracks `RemainingTime/Shots/ActiveTargets`; fires `SessionEnded` with a `SessionStats` struct payload; `IsActive` gates all logic
- `SessionController` — owns the `SessionPlaylist`; calls `SessionManager.BeginSession()` on start and after each session; shows/hides the `SessionHUD` end panel; exposes `OnNextSessionPressed()` / `OnReturnToMenuPressed()` for button `onClick` wiring in the inspector
- `SessionHUD` — updates live timer/shots/targets labels while `_session.IsActive`; `ShowEndScreen(stats, isLast)` populates stat labels and shows the correct button (Next or Main Menu); `HideEndScreen()` for the next session start
- `MainMenuController` — in the MainMenu scene; `StartGame()` / `Quit()` wired to buttons via inspector

### Data flow on session end

```
SessionManager.EndSession() → fires SessionEnded(sender, SessionStats)
SessionController.OnSessionEnded() → plays end sound → calls SessionHUD.ShowEndScreen(stats, isLast)
Player clicks "Next" → onClick → SessionController.OnNextSessionPressed()
  → SessionHUD.HideEndScreen() → SessionManager.BeginSession(nextConfig)
```

### Configuration system

**`GameplayConfig`** (`Assets > Create > TurretTraining > Gameplay Config`):
- `SessionDuration` — seconds the session lasts
- `MaxShotCount` — total shots available
- `ActiveTargets bool[]` — parallel to `SessionManager._targets[]`; `true` = target starts active

**`SessionPlaylist`** (`Assets > Create > TurretTraining > Session Playlist`):
- `Sessions GameplayConfig[]` — ordered configs played through in sequence

Swap the `SessionPlaylist` reference on `SessionController` to change the full scenario without modifying code.

### Asset layout

```
Assets/
  GameScene.unity                  # main game scene
  Rendering/                       # URP pipeline and renderer assets
  TurretTraining/
    Content/
      BigBlit/ShootingRange/       # 3rd-party shooting range meshes, materials, prefabs, textures
      Ground/                      # floor material and textures
      Reticle/                     # crosshair UI prefab and texture
      Skybox/                      # overcast skybox materials and scene
      Targets/                     # Target prefab
      Turret/Model/                # TurretA FBX meshes and PBR materials
    Scripts/                       # all gameplay C# code
```
