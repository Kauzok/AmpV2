## Amp
- WIP Amp, a melee/range hybrid who fights with electromagnetic attacks.
## Skills To Do/Bug Fixing
- Stormblade
  - Fix checkroll behavior in multiplayer
- Lorentz Cannon
  - Make spike prep SFX cancel on fire
  - Make prep gameobject spawn properly in multiplayer
  - fix laggy ghost in multiplayer
- Plasma Slash
  - Make unlockable
  - Fix multiplayer ghost becoming invisible
- Magnetic Vortex
  - Consider adding ability to remotely detonate vortex
  - Add field representing vortex range/or tendrils
  - Change sound origin
  - Make radial damage appear in multiplayer
- Surge
  - Fix effect shaking when running into objects
  - Consider adjusting entry VFX for visibility
  - Fix not being able to exit ability if remaining charges are left
  - Watch out for "received networktransform data for object that doesn't exist error"
- Pulse Leap
  - Adjust exit animation
  - Make skill remove fall damage in multiplayer & remove onexit error (possibly related?)
  - Keep an eye on launchblast method in OnEnter
- Fulmination
  - Consider adjusting chain effect
- Voltaic Bombardment
  - Adjust muzzle vfx
- Bulwark of Storms
  - Add VFX & SFX to channelling
  - Make worm unset skill override on death in multiplayer
  - Make unlockable
- Charge
  - Add in alternate effect asides from just damage

## Item Displays To Do
- Fix frost relic item display/revise current displays
- Adjust masking of model for limb replacement

## Animations To Do
- Adjust sideways walks
- Polish shockblade slashes
- Fix weird roll -> run transition
- Add standing still animations for shockblade, lorentz cannon, magnetic vortex & fulmination
- Polish stationary landing animation

## VFX To Do
- Consider modifying helmet/chest glow

## Misc. To Do
- Cleanup code base, finish comments
- Fix method of loading in assetbundles/soundbanks (and fix soundbank error)
- Switch VFX Shaders to cloud remap
- Emote and other API Support
- Add configs
- Adjust character portrait

## Changelog
`0.0.1`
- Initial release

