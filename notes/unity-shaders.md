## Shader Languages Unity Can use

### Shaderlab
 * Unity's own shader language
 * Required in all unity shaders

### Cg
 * Stands for "C for graphics"
 * Deprecated language
 * Can compile to Direct X or GLSL

### GLSL
 * OpenGL Shader Langauge
 * Unity advices that you only use GLSL if you are testing for OS X or Mobile devices. Because Unity will cross compile Cg/HLSL into GLSL when on the platforms that need it.
   * You can force Unity to cross complile glsl on all platforms with #pragma glsl

## Unity Shaders Overview

Shaders in Unity can be written in one of three different ways:

### Surface Shaders

Surface Shaders are your best option if your shader needs to be affected by lights and shadows. Surface shaders make it easy to write complex shaders in a compact way - it’s a higher level of abstraction for interaction with Unity’s lighting pipeline. Most surface shaders automatically support both forward and deferred lighting. You write surface shaders in a couple of lines of Cg/HLSL and a lot more code gets auto-generated from that.

Do not use surface shaders if your shader is not doing anything with lights. For Image Effects or many special-effect shaders, surface shaders are a suboptimal option, since they will do a bunch of lighting calculations for no good reason!

These shaders are expensive to render.

### Vertex and Fragment Shaders

Vertex and Fragment Shaders will be required, if your shader doesn’t need to interact with lighting, or if you need some very exotic effects that the surface shaders can’t handle. Shader programs written this way are the most flexible way to create the effect you need (even surface shaders are automatically converted to a bunch of vertex and fragment shaders), but that comes at a price: you have to write more code and it’s harder to make it interact with lighting. These shaders are written in Cg/HLSL as well.

These are the normal kind of shaders that I am familiar with for graphics programming.

### Fixed Function Shaders

Fixed Function Shaders need to be written for old hardware that doesn’t support programmable shaders. You will probably want to write fixed function shaders as an n-th fallback to your fancy fragment or surface shaders, to make sure your game still renders something sensible when run on old hardware or simpler mobile platforms. Fixed function shaders are entirely written in a language called ShaderLab, which is similar to Microsoft’s .FX files or NVIDIA’s CgFX.

## Vertex/Fragment shaders in Unity

When writing the vertex/fragment kind of shader in Unity there is an Interface part that handles everything that connects your shader to Unity. The Interface part also has Properties so you can change the shader from the unity UI.

And of course there will be a Vertex Shader part and a Fragment Shader part

## Precision of computations

When writing shaders in Cg/HLSL, there are three basic number types: float, half and fixed (as well as vector & matrix variants of them, e.g. half3 and float4x4):

 * float: high precision floating point. Generally 32 bits, just like float type in regular programming languages.
 * half: medium precision floating point. Generally 16 bits, with a range of –60000 to +60000 and 3.3 decimal digits of precision.
 * fixed: low precision fixed point. Generally 11 bits, with a range of –2.0 to +2.0 and 1/256th precision.
Use lowest precision that is possible; this is especially important on mobile platforms like iOS and Android. Good rules of thumb are:

For colors and unit length vectors, use fixed.
For others, use half if range and precision is fine; otherwise use float.
On mobile platforms, the key is to ensure as much as possible stays in low precision in the fragment shader. On most mobile GPUs, applying swizzles to low precision (fixed/lowp) types is costly; converting between fixed/lowp and higher precision types is quite costly as well.

## SubShader
Each Unity Shader will have a list of SubShaders and when the game runs Unity will pick one of the SubShaders to use based on which platform the game is currently running on (Android, Xbox, PC, etc).

## Pass
A SubShader can have multiple passes, so that the object the shader is on will render once for each pass in the SubShader.

## Shader Data
The vertex and frag shader data structs (input/output) need each variable to be annotated with a semantic (such as POSITION, TEXCOORD0, etc). There are also some premade shader data structs that Unity provides if you include UnityCG.cginc.
http://docs.unity3d.com/Manual/SL-VertexProgramInputs.html

## Image Effects Shaders
When using `Graphics.Blit()` on a camera to create an image effect the screen's image is passed into the shader as a texture named _MainTex.

## Game Time Inside Shader
To get the current time in a shader add `#include "UnityCG.cginc"` to the top of the shader and use the `_Time` variable. `_Time.x` is 1/20th of the current time and `_Time.y` is the actual game time.

## Texel Size
Here is how to find the size of 1 pixel for a texture inside a shader (so for a 1920x1080 resolution texture a pixel is (1/1920, 1/1080) in size). If the texture is named `_MainTex` then add another variable to the shader code called `float4 _MainTex_TexelSize;` which has the data.
The reason it is a float4 is because it has the following 4 values (1 / width, 1 / height, width, height).

## Dealing with Different Vertical Orientations Between OpenGL/DirectX
This unity page documents it pretty well: http://docs.unity3d.com/Manual/SL-PlatformDifferences.html
Basically my understanding is in certain situations (like when Anti Aliasing is used in unity) DirectX and OpenGL will have opposite ideas of which part of a texture is considered the top part. The solution is to use the snippet of code provided in the link above.
```GLSL
#if UNITY_UV_STARTS_AT_TOP
if (_MainTex_TexelSize.y < 0)
        uv.y = 1-uv.y;
#endif
```

## Making Transition Textures
When using my transition shader to create cool transitions on the camera. Keep these things in mind.
Blue controls when a pixel is shown by comparing itself to the Transition Material's `t` value.
Red and Green act as the final displacement vector at the end of the transition. That is when the Transition Material's `t` value is 1 then all the pixels will be fully displaced and when t is 0 then all the pixels will be where they started. So the Transition Material's t value interpolates each pixel between 0 displacement and full displacement. The 0 to 1 values of red and green get converted to -1 to 1 in the shader and act as a displacement vector in the texture's coordinate system.

Often you will have a blue gradient to signify a direction in which pixels should turn off. If you want it look like the pixels are sliding off then give the pixels a displacement in the opposite direction then the direction that the pixels are turning off. Because the displacement gives you where to sample from in the original unaltered image and we want to sample from pixels that have already been turned off (and not the pixels that will get turned off).

So for example if I want a horizontal left to right sliding screen wipe. Then:
	* All the pixels will have a green of 128 (aka 0 in the y direction)
	* All the pixels will have a red of 0 (aka -1 in the x direction), because when t is nearly 1 only the right most pixels will still be shown and those should look like the original pixels on the left side of the texture which are -1 away (since textures are 1 by 1 squares and the pixels we are samplying from are to the left of the pixels that are still visible).
	* The blue values of the image will be a gradient of 0 to 255 from left to right horizontally across the image.

Also note that the Transition Material's `Color` literally replaces pixels of the `Image Texture` so even if you give the `Color` a transparency you will not see the `Image Texture` underneath just whatever is behind the Material in the scene. If you want to actually see the `Image Texture` underneath (like for a fade effect) then you must set `Color Opacity` to less than 1 and then the shader will lerp between the Material's `Color` and the `Image Texture`'s color (you will also most likely want to turn off displacement and set the `Color`'s alpha to 1).
