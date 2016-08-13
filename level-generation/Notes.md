## Basic 3D Terrain Generation

One way to create random terrain is to combine multiple height samples into one final height. Each sample represents a different level of detail (for example Hills, Boulders, Pebbles).
Each "sample" can be called an octave.

A common source for the sample data is from perlin noise.

Lacunarity controls the change (usually increase) in frequency of the octaves. The higher the frequency the more detail a sample will have (I assume because frequency represents the frequency of details).
Persistance controls the change (usually decrease) in amplitude of the octaves. The lower the amplitude the less affect it will have on the final combined sample.

Lacunarity > 1
1 > Persistence > 0

Example Values
Lacunarity = 2
Persistence = 1/2


Octave 0
	frequency = lacunarity ^ 0
	amplitude = persistance ^ 0

Octave 1
	frequency = lacunarity ^ 1
	amplitude = persistance ^ 1

Octave 2
	frequency = lacunarity ^ 2
	amplitude = persistance ^ 2