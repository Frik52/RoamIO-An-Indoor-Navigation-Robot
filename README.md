# 🤖Autonomous Indoor Robot Navigation Using A Search with Dynamic Obstacle Avoidance


This project is a Unity-based robot navigation system with **LiDAR obstacle detection** and a **dynamic camera controller** that switches between **top-down** and **follow-behind** views.

---

## 🚀 Features
- **LiDAR Sensor**:
  - Simulates 360° ray-based obstacle detection.
  - Dynamically replans the path when an obstacle is detected.
- **Camera Controller**:
  - Top-down overview when idle.
  - Smooth follow camera directly behind the robot when moving.
- **Robot Controller**:
  - Handles goal setting, pathfinding, and movement.
- **Modular Design**:
  - Easy to extend with new sensors or AI logic.

---

## 📂 Project Structure
/Assets
/Scripts
├── LidarSensor.cs
├── RobotController.cs
├── CameraController.cs
/Scenes
├── MainScene.unity

yaml
Copy
Edit

---

## 🛠 Requirements
- **Unity**: `2022.3.x` (LTS recommended)
- **.NET Runtime**: Managed by Unity
- **Packages**:
  - AI Navigation (for pathfinding / NavMesh)
  - Cinemachine (optional, for advanced camera controls)

See [`requirements.txt`](requirements.txt) for details.

---

## ▶️ How to Run
1. Clone the repository:
   ```bash
   git clone https://github.com/Frik52/RoamIO-An-Indoor-Navigation-Robot.git
Open the project in Unity Hub.

Load the MainScene.unity scene.

Press Play ▶️ in the Unity Editor.

🎮 Controls
Robot moves automatically towards assigned goals.

Camera switches modes automatically:

Top-down when robot is idle.

Follow-behind when robot moves.



🤝 Contributing
Pull requests are welcome! For major changes, please open an issue first to discuss your ideas.

📜 License
MIT License © 2025
