## Amp
- WIP Amp, a melee/range hybrid who fights with electromagnetic attacks. Updated to new HenryMod template. Original name battlemage, will adjust other name tokens later.

## Skills To Do
- Stormblade
  - Animations and the like
  - Fix checkroll behavior
  - Consider adjusting swordswing2sfx to lower prominence of electricity sfx
  - Add swing/on hit VFX
  - Limit sfx sound so hitting multiple enemies doesn't make it super loud
- Lorentz Cannon
  - Make spike prep SFX cancel on fire
  - Adjust range
  - Make prep gameobject spawn properly in multiplayer
- Magnetic Vortex
  - Add skill
- Bolt 
  - Fix effect shaking when running into objects
  - Consider adjusting entry VFX for visibility
  - Look into "Received networktransform data for an object that doesn't exist" warning
  - Potentially work on hitbox filtering to avoid hitting same enemy twice
  - Fix calling of invincibility buff
- Pulse Leap
  - Add skill
- Fulmination
  - Fix chain effect
  - Keep an eye out for OnExit NRE; *should* be fixed but not sure yet
  - Adjust position of VFX
- Voltaic Bombardment
  - Adjust hitbox to make it hit higher
  - Figure out why tessellation vfx is only affected on clients
- Charge Passive
  - Consider making VFX more visible

## Misc. To Do
- Cleanup code base, finish comments
- Fix name tokens
- Character Model
- Character Model Animations
- Skill Icons
- Switch VFX Shaders to cloud remap

## Changelog
`0.0.1`
- Initial release

