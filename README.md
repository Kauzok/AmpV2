## Amp
- WIP Amp, a melee/range hybrid who fights with electromagnetic attacks. Updated to new HenryMod template. Original name battlemage, will adjust other name tokens later.

## Skills To Do
- Stormblade
  - Animations and the like
  - Consider adjusting swordswing2sfx to lower prominence of electricity sfx
  - Fix checkroll behavior
  - Add swing/on hit VFX
  - Limit sfx sound so hitting multiple enemies doesn't make it super loud
- Lorentz Cannon
  - Make spike prep SFX cancel on fire
  - Reduce range
  - Make prep gameobject spawn properly in multiplayer
- Bolt 
  - Make it cancellable/fix skill not exiting in multiplayer
  - Fix effect shaking when running into objects
  - Potentially work on hitbox filtering to avoid hitting same enemy twice
- Pulse Leap
  - Add skill
- Fulmination
  - Fix chain effect
  - Adjust position of VFX
  - Consider looking into fixedupdate NRE (doesnt do anything but might cause unseen issues)
- Voltaic Bombardment
  - Adjust hitbox to make it hit higher
  - Figure out why tesselation vfx is only affected on clients
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
