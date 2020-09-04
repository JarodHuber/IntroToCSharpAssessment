﻿/*******************************************************************************************
*
*   raylib [core] example - Basic window
*
*   Welcome to raylib!
*
*   To test examples, just press F6 and execute raylib_compile_execute script
*   Note that compiled executable is placed in the same folder as .c file
*
*   You can find all basic examples on C:\raylib\raylib\examples folder or
*   raylib official webpage: www.raylib.com
*
*   Enjoy using raylib. :)
*
*   This example has been created using raylib 1.0 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2013-2016 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using Raylib_cs;
using System.Numerics;
using static raygamecsharp.Objects;
using static Raylib_cs.Color;
using static Raylib_cs.Raylib;
using CRPGThing;

namespace raygamecsharp
{
    public class Window
    {
        enum CombatPhase { START, PLAYERATTACK, PAUSE, ENEMYATTACK, END }
        public const int screenWidth = 800;
        public const int screenHeight = 450;

        public static Timer attackTimer = new Timer(20);

        CombatPhase stage = CombatPhase.START;

        Player curPlayer;
        Monster curMonster;

        public bool WindowHidden { get { return IsWindowHidden(); } }

        public Window()
        {
            InitWindow(screenWidth, screenHeight, "Combat");

            SetTargetFPS(60);

            InitializeCharacters();

            HideWindow();
        }

        public void StartAttack(Player player, Monster monster)
        {
            stage = CombatPhase.START;
            curPlayer = player;
            curMonster = monster;

            curPlayer.currentWeapon.weaponAttack.Start();
            curMonster.enemyAttack.Start();

            attackTimer.Reset(7);
            UnhideWindow();
        }

        void EndAttack()
        {
            curPlayer = null;
            curMonster = null;
            HideWindow();
        }

        public void Run()
        {
            BeginDrawing();
            ClearBackground(BLACK);
            switch (stage)
            {
                case CombatPhase.START:
                    DrawText("Loading", 300, 200, 40, RAYWHITE);
                    if (attackTimer.Check())
                        stage = CombatPhase.PLAYERATTACK;
                    break;
                case CombatPhase.PLAYERATTACK:
                    PlayerAttack();
                    break;
                case CombatPhase.PAUSE:
                    DrawText("Press Enter to continue", 170, 50, 40, RAYWHITE);
                    if (IsKeyPressed(KeyboardKey.KEY_ENTER))
                        stage = CombatPhase.ENEMYATTACK;
                    break;
                case CombatPhase.ENEMYATTACK:
                    EnemyAttack();
                    break;
                case CombatPhase.END:
                    EndAttack();
                    break;
            }
            EndDrawing();
        }

        void PlayerAttack()
        {
            if (attackTimer.Check())
            {
                stage = CombatPhase.PAUSE;
                return;
            }
            curPlayer.currentWeapon.weaponAttack.Update();
        }
        void EnemyAttack()
        {
            if (attackTimer.Check())
            {
                stage = CombatPhase.END;
                return;
            }
            curMonster.enemyAttack.Update();
        }

        public void Close()
        {
            CloseWindow();
        }
    }
}