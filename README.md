# SpatialOrientationPractice
 A Unity Project for practicing the Spatial Orientation Test commonly found in Pilot and ATC assessments. After finishing the core functionality, being able to complete questions, may extend (eg: adding an answer checker, or adding complexity to questions that you would not find in any of the assessments) 

The Spatial Orientation Test consists of questions where there are 2 compasses - a gyrocompass (GYRO) and a radiocompass (RBI). 
The GYRO represents the direction a plane is travelling, with the compass rotating around a representation of the plane such that the top of the compass is always the direction the plane is travelling.
The RBI shows the direction of a nondirectional radar beacon to the plane using an arrow, and is relative to the direction of the plane (eg: or a plane travelling true west, the compass North of the RBI is true west). 

The questions in a SOT require that you determine the direction of travel of the plane (easy part), as well as the location of the plane relative to the beacon (harder part). 
You select the plane's direction from a set of 8 symbols (cardinals and secondary cardinal directions).
You select the plane's location by placing the selected plane symbol in one of 8 squares around the beacon.
