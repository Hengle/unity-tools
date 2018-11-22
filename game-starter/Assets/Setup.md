## General
Project Settings > Editor
  Visible Meta Files
  Force Text
  
## Tilemap
More complicated brushes and tiles (like anmiated tiles and prefab brush) are
found here:
https://github.com/Unity-Technologies/2d-extras


For some tiles and brushes (for example animated tiles and prefab brushes)
you need to create new assets (right click create in project window) and
configure the tile or brush before you can use it in the tilemap.

## 3D Lighting Instructions
  Use Legacy Diffuse for most 3D materials.
  Have a scene lighting of ambient color and make it #D9D9D9 (which is white #FFFFFF at %85 intensity)
  Have an all white #FFFFFF directional light with effectively %15 intensity on the ground.
  To figure out the directional light intensity use the following:
    cos(angle) * intensity = 0.15
    intensity = 0.15 / cos(angle)
  Where angle is the the angle between light direction and Down direction

## Post Processing

`Window` > `Package Manager`
Click `All` tab and look for `Post-processing` and install it

Add a `Post Processing Layer` to the `Main Camera`
Select the `PostProcessing` layer in the component and give the camera the `PostProcessing` layer
Add a `Post Processing Volume` to the `Main Camera`
Enable `Is Global` in the `Post Processing Volume`
Right click in the `Project` tab and create a `Post-processing Profile`
Drop the `Post-processing Profile` in the `Post Processing Volume`

### Bloom (Glowing effect)

Click `Add effect...` in your `Post-processing Profile`
Check `Intensity` and set it some thing higher than 0 like 3

Click on the `Material` you want to be glowing.
Check `Emission`
Click on the color and set the `Intensity` to something that looks right (I've set it to 1 in the past) 

## Quick Outline Package
https://assetstore.unity.com/packages/tools/particles-effects/quick-outline-115488

## iOS Device Testing
https://unity3d.com/learn/tutorials/topics/mobile-touch/building-your-unity-game-ios-device-testing

## My older Graphics settings
  Project Settings > Quality
    Anti Aliasing 4x Multi Sampling
    Shadow Projection Close Fit

  Window > Lighting > Settings > Scene Tab (unnecessary if using a scene from the starter)
    Ambient Source Color
    Ambient Color B2B2B2

## Things to try:

  Edit > Project Settings > Player > Other Settings > Color Space > Linear

  Project Settings > Quality
    Anti Aliasing Disabled (use post processing instead)
    
  Camera Component
    Allow HDR turned on
    Allow MSAA turned off
    
  Post Procesing Profile
    Enable Color Gradiant
      Tone Mapper Filmic
      Post Exposure 0.6
    Enable Eye Adaption
      Go to light settings and change Ambient Light Intensity to 2
      Change your main Directional Light Intensity to 7
    Enable Motion Blur
    Enable Vignette
      Reduce intensity to 0.37
    Enable Ambient Occulusion
    Enable Bloom
      Radius 2.5
      Intensity 0.1
    Enable Antialiasing 
      Method > Temporal Anti-aliasing
      
## Knockback 
For characters moving with AddForce() using a simple Impulse for knockback works but is imperfect. 
* The impulse needs to be a much higher number than the sustained force for movement. For example:
  * Player
    * accel = 50f
    * stoppingDrag = 25f
    * maxSpeed = 6f
  * knockbackAccel = 600f
* The impulse moves the character more when player is moving then when the player is stationary (since the player turns off drag during movement)

For higher quality knockbacks try the following:
    Apply the knockback as a sustained force (instead of Impulse) which is applied every fixed update for a set duration.
    During the time it is applied the character should have its stoppingDrag off or it should be added to the player's velocity if I'm manually setting velocity for character movement.