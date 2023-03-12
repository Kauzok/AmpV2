  ## Amp
- WIP Amp, a melee/range hybrid who fights with electromagnetic attacks.
## Skills To Do
- Shockblade
  - Add smear?
- Lorentz Cannon
  - Make spike prep SFX cancel on fire
  - Make prep gameobject spawn properly in multiplayer
  - fix laggy ghost in multiplayer
- Plasma Slash
  - Fix multiplayer projectile becoming invisible
- Magnetic Vortex
  - Add field representing vortex range/or tendrils
  - Non-boss enemies are silenced; bosses have their damage reduced by 25%.
- Surge
  - Fix not being able to exit ability if remaining charges are left
  - Watch out for "received networktransform data for object that doesn't exist error"
  - Make momentum cancel on early exit in multiplayer
  - Doesn't work correctly on clients? Look into it.
- Pulse Leap
  - Keep an eye on launchblast method in OnEnter
- Fulmination
  - Consider adjusting chain effect
  - Keep an eye on your fix for bad statetype null
- Bulwark of Storms
  - Add VFX & SFX to channelling
  - Make worm unset skill override on death in multiplayer
  - Make Melvin follow Amp
  - Red Melvin?

## Item Displays To Do
- Fix frost relic item display/revise current displays
- Adjust masking of model for limb replacement

## Animations To Do
- Adjust sideways walks
- Polish shockblade slashes
- Fix weird roll -> run transition
- Add standing still animations for shockblade, lorentz cannon, magnetic vortex & fulmination

## Bugs to Fix
- Amp sometimes switches skins mid run
- Obliterating on Monsoon will not unlock Red Sprite
- Beating level 1 of eclipse will not unlock the next level
- Some mod combinations will result in Amp messing up unlockables for other chars?

## Misc. To Do
- Cleanup code base, finish comments
- Fix method of loading in assetbundles/soundbanks (and fix soundbank error
- Emote and other API Support
- Add configs
- Adjust character portrait
