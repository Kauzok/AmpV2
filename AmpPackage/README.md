# Amp Overview
- Adds in Amp, an agile melee/ranged custom survivor who fights with the electrifying power of electromagnetism.
- Featuring 9 custom skills, a unique passive, lore, a mastery skin, item displays, unique Mithrix dialogue, and more!
- **DISCLAIMER**: Amp is still a WIP survivor, so many things are subject to change, and you may encounter some bugs (in the case of multiplayer, "may" turns into "will").

For feedback & bug reporting, message Neon#2032 on discord or just @ me in the modding server  (https://discord.gg/VuRshdPgvU). I really appreciate any and all feedback; I want Amp to be as fun to play as possible!

NOTE: IF YOU HAVE AN ERROR THAT MENTIONS UNLOCKABLEAPI, AND CAUSES AMP TO NOT APPEAR IN THE LOBBY, UPDATE BEPINEXPACK.

[![](https://cdn.discordapp.com/attachments/226439908657463296/1007789478032707614/AmpDisplay.png)]()

# Skills (picture is slightly outdated)
[![](https://cdn.discordapp.com/attachments/226439908657463296/1007782321094656132/AmpSkills.png)]()


[![](https://cdn.discordapp.com/attachments/226439908657463296/1007782708614803537/AmpDash.png)]()
[![](https://cdn.discordapp.com/attachments/226439908657463296/1007782759227457576/AmpLightning.png)]()
[![](https://cdn.discordapp.com/attachments/226439908657463296/1007782614360395907/AmpItems.png)]()


# To Do
- Fix multiplayer functionality
- Adjust some animations
- Add in configs for players to tweak some of Amp's values
- For a more detailed WIP list as well as a list of known bugs, visit the github!
- Also if anybody is interested in making some new skill icons for Amp, just hit me up on discord! pls

# Credits
- Neon: Animations, Code, VFX, SFX
- Gaborade: Modeling
- NotWhoYouThink: Lorentz Cannon Code & VFX, Charge Orb Networking
- Vale-X (Manipulator): Charge debuff icon
- RandomlyAwesome: Helping me out with networking code
- Tyler Cook: Nemesis Amp Meter Design Feedback
- Mr. Bones: Reformation skin
- Bubbet: Fixing Melvin's health in multiplayer
- The Risk of Rain 2 Modding Server: Helping me stay sane through all kinds of other bugs I wouldn't have been able to solve on my own.

# Change Log
<details>
 <summary>1.2.1</summary>
- Magnetic Vortex
    -  Fixed a bug where the muzzle effect wouldn't disappear on firing
</details>

<details>
<summary>1.2.0</summary>

- Sorry it's been so long! School was really messing me up, and I only recently was able to get some real changes made. This mod definitely isn't abandoned, so don't worry; more updates to come!
- New Known Bugs
    - Amp will sometimes randomly become invincible in multiplayer; I actually think I fixed this issue, but since the occurrence is near random and there's no error in the log when it occurs it's hard to test. So, if you run into this problem, please let me know!
    - Amp will sometimes randomly switch skins mid-run
    - This has not yet become an issue, but if you're playing multiplayer while not being the host and you find that achievements aren't working, let me know!
    - If someone is using the red sprite skin in multiplayer, then every Amp will have their surge skill use the red VFX, even if they have the normal skin.
- General
    - Cleaned up the config file and removed unused entries
    - Fixed a glitch that caused a "bad statetype null" error upon cancelling Fulmination early
    - Grammar and wording adjustments for select screen tips & lore
    - Moved unlockables to use vanilla methods instead of R2API's UnlockableAPI; this has the unintended side effect of re-locking Amp's Red Sprite Skin if you've already unlocked it, but as said before you can always just head to the config file to re-unlock it.
        - Note: all new unlockables can be unlocked in config settings as well, if you're not up to the challenge
- Animations
    - Fixed Amp's ascending animation not looping
    - Fixed Amp's movement animations not showing up for clients
- Skins
    - Amp's "Reformation" Skin is now unlocked by beating the game on Typhoon or any higher difficulty
- Charge
    - Now creates floating electric orbs above an enemy's head that indicate how many stacks of charge they have. If they're too distracting, you can disable these in the config. 
    - No special VFX for electrified yet unfortunately; I would've put them in this patch, but didn't have enough time and I wanted to get this one out ASAP.
- Plasma Slash
    - Is now an unlockable
- Surge
    - Fixed a glitch where clients on a multiplayer server wouldn't be able to properly use this ability. It's still kind of laggy, but that'll be fixed in the next patch, which will hopefully complete Amp's multiplayer compatibility.
    - Fixed a glitch where the Surge exit explosion wouldn't appear in mutliplayer.
- Magnetic Vortex
    - Fixed a glitch where bright flashing purple "walls" would appear around the map upon usage (for real this time I think; if you still encounter this, please let me know!)
    - Fixed a glitch where the sound for the end explosion wouldn't play.
- Bulwark of Storms
    - Is now an unlockable

</details>

<details>
 <summary>1.1.1</summary>
 
- General
    - Removed a glitch where Pulse Leap would play an additional sound it wasn't supposed to
</details>

<details>
 <summary>1.1.0</summary>
 
- General
    - Amp's Mastery skin, Red Sprite, now has red lightning effects!
        - Also added a config so you can choose whether to have red lightning or the original blue
        - Let me know if the red Voltaic Bombardment effects are too intense; I'm a bit on the fence on whether or not they're fine as is, or if they need to be toned down a bit. Let me know how you feel about the other red vfx changes too!
    - I know I said this patch would include some extra VFX for charged & a magnetic vortex special effect, but I wanted to get this out first since school's taking up a lot of my time now & it may be a while before I get to that. Next patch for sure though, promise!
        - I'm also going to try to fix Surge not working properly in multiplayer next patch. Look forward to it!
- Plasma Slash
    - Amp now faces the direction of the cursor while using the ability
- Surge
    - Fixed effect glitching out upon running into enemies or the ground
</details>

<details>
 <summary>1.0.10</summary>
 
- 1.0.10
    - Removed wormhealth item from chests
</details>


<details>
 <summary>1.0.9</summary>

- General
    - SOUNDS! No more being deafened by a magnetic vortex halfway across the map, as sounds should all now have attentuation and not be as earrapey. If you still have issues with sounds being way too loud or sounds being heard from everywhere on the map, please let me know!!
        - Also added pitch randomization to a few sounds, should make them not as dull to hear over and over again.
    - Added new skin, "Reformation". Credit to Mr. Bones!
    - Next patch to address some more multiplayer bugs & add some extra VFX for charged & electrified, as well as a special charged effect for Magnetic Vortex.
- Charge
    - Explosion no longer applies shocked status effect
    - Explosion now applies a debuff called "electrified", which acts as a lingering charge debuff that allows Amp to continue to apply his secondary abilities' special effects.
- Modified Shockblade
    - Further increased range, now much more in line with vanilla melee survivors
    - Reduced damage from 160% to 150%
- Lorentz Cannon
    - Increased damage from 130% to 140%
    - Added projectile trails to improve visibility
- Magnetic Vortex
    - Increased projectile speed from 70 to 90
    - Increased projectile radius from 0.6 to 1.0
    - SHOULD HAVE fixed a bug where the VFX messed up and caused flashing purple walls to appear everywhere. Please let me know if you still encounter it!
    - Plasma Slash
    - Further increased grounded slash range
- Surge
    - Cancelling early now completely stops your momentum. Use this to precisely maneuver around attacks!
        - This functionality doesn't appear in multiplayer; to be fixed with next patch!
    - Adjusted hitbox position
- Pulse Leap
    - Sped up exit flip animation
    - Decreased explosion effect size
- Bulwark of Storms
    - Fixed Melvin's name/health not showing up correctly in multiplayer
        -Sidenote: if you find an item named WORMHEALTH please let me know.
</details>

<details>
 <summary>1.0.8</summary>
 

- Reworked item displays to be compatible with extra item mods; mod loading shouldn't get stuck at 100% anymore
</details>

<details>
 <summary>1.0.7</summary>

- Discovered incompatability with Extra Fireworks & updated README
</details>

<details>
 <summary>1.0.6</summary>

- General
    - Fixed bug where UnlockableAPI wouldn't load properly
    - Some more bug related and QOL changes to come soon, like making Lorentz Cannon's projectiles more visible & fixing a common bug where Amp's item displays cause loading to get stuck at 100% with certain mod combinations
- Modified Shockblade
    - Extended depth range from 7 to 10.5 units
    - Extended horizontal range from 6.6 to 7.5 units
    - Extended vertical range from 5 to 6 units
    - Buffed damage from 140% to 160%
    - Slashes now *guarantee* a proc of charge instead of having a 20% chance. This is a tentative change to make melee a more viable option.
- Plasma Slash
    - Fixed a bug where plasma slash could interrupt itself when multiple charges were present
    - Extended slash dimensional ranges: 
        - Depth: from 15 to 16.5 units
        - Horizontal: from 15 to 17 units
        - Vertical: from 6 to 9 units
    - Increased width of projectile, should be easier to hit now
- Lorentz Cannon
    - Increased strength of homing; should be much more consistent now
    - Increased cooldown from 1.5 to 3 seconds
    - Reduced bullet damage from 160% to 130%
    - Reduced proc coefficient per bullet from 1 to .7
- Magnetic Vortex
    - Fixed a bug where killing/damaging enemies wouldn't cause item procs
    - Radial damage & explosion now consistently apply in multiplayer
- Surge
    - Increased contact damage from 100% to 150%; again, a really tentative change
    - Increased cooldown from 8 to 10 seconds
- Pulse Leap
    - Fall damage removal now works in multiplayer
    - Fixed NRE spamming the console in multiplayer
- Fulmination
    - Reduced damage from 2400% to 2200%
- Voltaic Bombardment
    - Increased damage from 1000% to 1400%
    - Increased overcharge duration from 3 to 5 seconds

</details>

<details>
 <summary>1.0.0 -> 1.0.5</summary>

- READMEs are hard
</details>

