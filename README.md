# 🍬 Drop Jelly
**Drop Jelly** is a satisfying and vibrant mobile puzzle game where players drop colorful jelly blocks into the grid to solve visual sorting challenges. Inspired by match-and-organize mechanics, the game combines simple interaction with progressively tricky puzzles that test spatial thinking and planning.

This project is a Unity-powered hobby clone created for educational and portfolio purposes, based on the mobile game genre of visually appealing block-sorting games.
📱 [Original Game](https://play.google.com/store/apps/details?id=games.supermesh.dropjellies&hl=en)

---

##🎯 Project Goals

- Recreate the core mechanics of games like Drop Jelly
- Improve Unity skills by handling multiple inner objects within a parent block
- Develop a clean and scalable input system with touch and swipe support
- Practice animation using DOTween for responsive and juicy movement
- Build a foundation for potential content expansion (e.g. levels, effects)

---

##🎮 Gameplay Overview
- Each JellyBlock contains 1–4 InnerPieces, each with its own color.
- Players must drop these blocks into grid slots so that matching colors align.
- When enough same-colored pieces are connected, they merge or disappear.
- Plan drops to prevent grid overflow and optimize for chain reactions.
  
---

## 🛠️ Features

- 🎨 Custom grid-based board logic
- 🧩 Multiple piece configurations (1x1, 2x1, 2x2)
- 🧠 Faced piece detection system for match validation
- ⏭️ Fixed **Next/Prev** buttons for navigating levels
- 📦 Clean folder structure and modular codebase
  
---

##🧠 System Design
- InnerPiece & JellyBlock Architecture:
Each JellyBlock acts as a parent, holding multiple InnerPieces. InnerPieces are responsible for color and merging logic, while the JellyBlock handles movement and animation.

- Touch-Based Input with Swipe Detection:
A threshold-based swipe input system ensures accurate drop direction and intuitive mobile interaction.

- Grid System:
Cells can check for occupancy, manage contained blocks, and validate if a drop is allowed.

- Animations:
All drops, merges, and clears use DOTween animations.

---

##📈 Level Progression (Manual)
- While an automated target system and level-based difficulty scaling are not yet included, players can switch between scenes manually using the Next and Prev buttons.
- This provides a basic way to test level designs across different setups.

---

##🎨 Technologies Used
- Unity (C#)
- DOTween – for movement and UI animations
- ScriptableObject – for organizing block and level data
- Unity UI System – Canvas-based layout and controls
- Custom InputManager – for swipe and tap input

---

##📌 Notes
- This project is a non-commercial clone created purely for learning and showcasing development skills.
- All code, systems, and mechanics are implemented from scratch.
- The game was inspired by modern mobile puzzle games but includes original architecture and gameplay logic.

---

**Fatih Bozkurt**  
🎮 Unity Game Developer  
🌍 Focused on puzzle, hybrid-casual mobile games  
📫 [LinkedIn](https://www.linkedin.com/in/fatih-bozkurt-9bb915212) | [GitHub](https://github.com/fatihhbozkurtt)





