Project 1 - COMP30019 - Graphics and Interaction

Landscape

The terrain is built using the diamond square algorithm. The program takes in 3 public variables, the height, the dimensions and the 
size of the terrain. The height determines the range of y values that the diamond square algorithm can set for the terrain. The 
dimensions controls the number of squares that the diamond square algorithm will take in (i.e. 4x4). The size variable will determine 
the total width and length of the terrain. 

Once these values have been set the program will generate the terrain using 2 loops. The first loop will create the triangles used
by the mesh. This is done by offsetting the x and y values by half the size of the terrain (so it is generated in the middle of the 
world space). The loop determines each vertice's x and y value based on the size of the terrain and the dimensions.

The second part of the program generates the y values of the vertices based on the diamond square algorithm. The algorithm implementation
was referenced from https://www.youtube.com/watch?v=1HV8GbFnCik&t=539s however certain parts were altered to work for our specific 
implementation. The algorithm works by setting the y values of the initial corners of the plane to random values. It then uses a for loop
which will incrementally reduce the square size and increase the number of squares ( increasing the number of iterations so it can set 
the y values of the inner squares ), running the diamond square algorithm on each iteration. 


Camera Motion

The camera motion is controlled using the a,s,w,d keys and is updated by adding the camera position to the transform.position 
depending on which direction the user is moving. The camera's direction can also be controlled using the mouse which uses a Quaternion 
generated using the x and y values of the mouse and adding that to the transform.rotation. Collision detection is handled in a discrete
method whereby if the user gets too close to the terrain (unity colliders overlap) onTriggerEnter is called which will increase the 
camera's y value as to avoid the collision. If the user attempts to exit the map, a function moveBackIntoGame is called which will
automatically push the user back into the game field. 


Surface Properties

The water is build using Displacement mapping by controlling the movement of the vertices. Random salts are added into the formula to
 show some randomness of the water wave. Properties such as wave speed wave height can be adjusted to have performance ideally.

The Light is using PhongShader to achieve realistic light reflection, as the sun rotates it will show different reflect from the landscape
and the water. 
The speed of the sun can adjust using time multiplier. The Light will follow Sunrise Sunset time to perform individual behaviour.

By adjusting the diffuse reflection and specular reflection according to the light factor from the sun rotation pass to the shader.
