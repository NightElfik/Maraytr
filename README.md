
Maraytr - Marek's ray-tracer
=========

Maraytr is a ray-tracer written with high focus on algorithms and data structures rather than on performance.
I decided to make it as revision and deeper understanding of theory and math behind ray-tracing.
And also because ray-tracing is fun!
The core was written in four days and without third-party libraries.
The scene is represented as CSG and supported primitives are sphere, cube, and plane.
It is possible to do basic boolean operations like union, intersection, subtraction, or xor.

**Project page**: http://www.marekfiser.com/Projects/Maraytr

**License**: Public domain, see LICENSE.txt for details.

Features
--------

* Extensible design.
* CSG scene represencation.
  * Three primitive shapes: sphere, cube and half-space (thick plane).
  * Support for affine tranformations.
  * Support for boolean operations: union, intersection, and subtraction.
* Supersampling for smoother images.
* Point and are lights (soft shadows).
* Phong lighting model.
* Procedural textures (checkers, stripes).
* Perspective camera.

Images
------

![Test scene](http://www.marekfiser.com/Img/640/480/Img/Projects/Maraytr/Dice.png)

![Test scene](http://www.marekfiser.com/Img/640/480/Img/Projects/Maraytr/4thDay.png)
