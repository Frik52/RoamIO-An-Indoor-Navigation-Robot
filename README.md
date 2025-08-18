# RoamIO
 => Overview

This project is a robot navigation simulation built in Unity with support for both 2D and 3D environments.
The robot autonomously navigates from a start point to a user-selected destination using the A* pathfinding algorithm, while avoiding static and dynamic obstacles.

Features include interactive destination setting, real-time obstacle avoidance, and a smooth camera system that follows the robot in motion.


==> Key Features

   => Autonomous Robot Navigation (2D + 3D modes)
    
   => A* Pathfinding with obstacle avoidance
    
   => Interactive target selection via clicks/taps
    
   => Smart Camera: overhead → follow → return
    
   => Static & Dynamic Obstacles (walls, NPCs, doors)
    
   => Indoor 3D environment + Top-down 2D grid
   

🖥==> Demo Workflow

   => Launch simulation → Camera starts overhead.
    
   => User clicks a target location.
    
   => Robot calculates shortest path using A*.
    
   => Robot moves, avoiding obstacles in real time.
    
   => Camera follows robot → returns to overhead when idle.
   

==> Tech Stack

   => Engine: Unity 2021+
    
   => Language: C#
    
   => Algorithm: A* Pathfinding
    
   => Physics: Unity Physics (collisions + obstacle detection)
   

==> Assets:

  => 2D sprites (tiles, robot, walls)
  
  => 3D prefabs (robot, furniture, walls, doors)
  

==> Installation & Setup

 => Clone the repository:
  
 => git clone https://github.com/Frik52/RoamIO-An-Indoor-Navigation-Robot.git
 => cd RoamIO-An-Indoor-Navigation-Robot


-> Open in Unity Hub (Unity 2021 or newer).

-> Load the Main Scene (2D or 3D).

-> Press ▶️ Play to start simulation.


==> Future Improvements

=? Multi-robot navigation & collision avoidance

=> Path smoothing with Bezier curves
 
=> VR/AR support
 
=> Machine Learning-based navigation
 
=> Custom map editor
