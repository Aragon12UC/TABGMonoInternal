using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace ExampleAssembly
{
    public class Cheat : MonoBehaviour
    {
        private int mainWID = 1024;
        private Rect mainWRect = new Rect(5f, 5f, 300f, 150f);

        private bool magicBullet;
        private bool godmode;
        private bool drawMenu = true;

        private float lastCacheTime = Time.time + 5f;
        private float lastItemCache = Time.time + 1f;

        public static Player[] players;
        public static Pickup[] droppedItems;
        public static Car[] vehicles;


        public void Keyhandler() {
            if (!Input.anyKey || !Input.anyKeyDown) {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Insert)) {
                drawMenu = !drawMenu;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) && magicBullet) {
                if (players.Length > 0) {
                    foreach (Player player in players) {
                        if (player != Player.localPlayer && player != null) {
                            foreach (ProjectileHit proj in FindObjectsOfType<ProjectileHit>()) {
                                player.m_playerDeath.TakeDamage(proj.transform.position, new Vector3());
                            }
                        }
                    }
                }
            }
        }

        public void Supergun(ref Gun gun) {
            gun.bullets = 65535;
            gun.bulletsInMag = 65535;
            gun.extraSpread = 0f;
            gun.hasFullAuto = true;
            gun.hipSpreadValue = 0f;
            gun.projectileRecoilMultiplier = 0f;
            gun.rateOfFire = 0.025f;
            gun.currentFireMode = 2; // Full auto.

            Destroy(gun.GetComponent<Recoil>());
        }

        public void Update() {
            Keyhandler();

            if (godmode) {
                if (Player.localPlayer != null) {
                    Player.localPlayer.stats.regenerationAdd = 3000f;
                    Player.localPlayer.stats.extraJumps = 1000;
                }
            }

            if (Time.time >= lastCacheTime) {
                lastCacheTime = Time.time + 5f;

                players = FindObjectsOfType<Player>();
                vehicles = FindObjectsOfType<Car>();

                ESP.mainCam = Camera.main;
            }

            if (Time.time >= lastItemCache) {
                lastItemCache = Time.time + 1f;

                droppedItems = FindObjectsOfType<Pickup>();
            }
        }

        public void OnGUI() {
            if (drawMenu) {
                mainWRect = GUILayout.Window(mainWID, mainWRect, MainWindow, "Main");
            }
        }

        private void MainWindow(int id) {
            GUILayout.BeginHorizontal();
            {
                magicBullet = GUILayout.Toggle(magicBullet, "Magic Bullet");
                godmode = GUILayout.Toggle(godmode, "Godmode");
            }
            GUILayout.EndHorizontal();

            if (Player.localPlayer != null) {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label($"Added Speed {Mathf.Floor(Player.localPlayer.stats.movementSpeedAdd)}");
                    Player.localPlayer.stats.movementSpeedAdd = GUILayout.HorizontalSlider(Player.localPlayer.stats.movementSpeedAdd, 0f, 30f);
                }
                GUILayout.EndHorizontal();

                if (GUILayout.Button("Superguns")) {
                    Gun rightGun = Player.localPlayer?.m_weaponHandler?.rightWeapon?.gun;
                    Gun leftGun = Player.localPlayer?.m_weaponHandler?.leftWeapon?.gun;

                    if (!rightGun) {
                        return;
                    }

                    Supergun(ref rightGun);

                    if (!leftGun) {
                        return;
                    }

                    Supergun(ref leftGun);
                }

                if (GUILayout.Button("Chams")) {
                    ESP.DoChams();
                }
            }
            
            GUILayout.Space(20f);

            GUILayout.BeginVertical("ESP", GUI.skin.box);
            {
                GUILayout.Space(20f);

                GUILayout.BeginHorizontal();
                {
                    ESP.crosshair = GUILayout.Toggle(ESP.crosshair, "Crosshair");
                    ESP.item = GUILayout.Toggle(ESP.item, "Item");
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    ESP.playerBox = GUILayout.Toggle(ESP.playerBox, "Player Box");
                    ESP.vehicle = GUILayout.Toggle(ESP.vehicle, "Vehicle");
                }
                GUILayout.EndHorizontal();

                ESP.playerName = GUILayout.Toggle(ESP.playerName, "Player Name");
            }
            GUILayout.EndVertical();

            GUI.DragWindow();
        }

        private string MakeEnable(string label, bool toggle) {
            string status = toggle ? "<color=green>ON</color>" : "<color=red>OFF</color>";
            return $"{label} {status}";
        }
    }
}
