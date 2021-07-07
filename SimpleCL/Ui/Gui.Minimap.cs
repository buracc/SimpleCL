﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using SimpleCL.Models.Character;
using SimpleCL.Models.Coordinates;
using SimpleCL.Models.Entities;
using SimpleCL.Models.Entities.Pet;
using SimpleCL.Models.Entities.Teleporters;
using SimpleCL.Ui.Components;
using SimpleCL.Util.Extension;
using Timer = System.Timers.Timer;

namespace SimpleCL.Ui
{
    partial class Gui
    {

        private void InitMapTimers()
        {
            var markerTimer = new Timer(100);
            markerTimer.Elapsed += (_, _) =>
            {
                RefreshMarkers();
            };
            
            markerTimer.Start();
            
            var mapTimer = new Timer(10000);
            mapTimer.Elapsed += (_, _) =>
            {
                if (_localPlayer.Uid == 0)
                {
                    return;
                }

                RefreshMap();
            };
            
            mapTimer.Start();
        }
        
        public void RefreshGui()
        {
            var local = _localPlayer;
            if (local == null)
            {
                return;
            }

            nameLabelValue.Text = local.Name;
            jobNameLabelValue.Text = local.JobName;

            Text = "SimpleCL (" + local.Uid + ")";

            hpProgressBar.Maximum = (int) local.MaxHp;
            mpProgressBar.Maximum = (int) local.MaxMp;
            hpProgressBar.Value = (int) local.MaxHp;
            mpProgressBar.Value = (int) local.MaxMp;

            expProgressBar.Value = (int) local.GetExpPercent();
            expProgressBar.CustomText = local.GetExpPercentDecimal().ToString("P", CultureInfo.CurrentCulture);
            jobExpProgressBar.Value = (int) local.GetJobExpPercent();
            jobExpProgressBar.CustomText =
                local.GetJobExpPercentDecimal().ToString("P", CultureInfo.CurrentCulture);

            levelLabelValue.Text = local.Level.ToString();
            jobLevelLabelValue.Text = local.JobLevel.ToString();
            spLabelValue.Text = local.Skillpoints.ToString("N0");
            goldLabelValue.Text = local.Gold.ToString("N0");

            var worldPoint = WorldPoint.FromLocal(local.LocalPoint);
            localCoordsLabelValue.Text = local.LocalPoint.ToString();
            worldCoordsLabelValue.Text = worldPoint.ToString();
            currLocalLabelValue.Text = local.LocalPoint.ToString();
            currWorldLabelValue.Text = worldPoint.ToString();
        }

        public void AddMinimapMarker(Entity entity)
        {
            var marker = new MapControl();
            _toolTip.SetToolTip(marker, entity.ToString());

            switch (entity)
            {
                case TalkNpc:
                    marker.Image = Properties.Resources.mm_sign_npc;
                    break;

                case Monster monster:
                    marker.Image = Properties.Resources.mm_sign_monster;
                    var monsterMenu = new ContextMenuStrip();
                    var monsterMenuitem = new ToolStripMenuItem
                    {
                        Text = "Attack",
                        Name = monster.ToString()
                    };

                    monsterMenuitem.Click += (_, _) =>
                    {
                        var skill = SelectedSkills.FirstOrDefault(x => !x.IsOnCooldown());
                        Log("Going to attack " + monster.Name + " with skill " + skill);
                        monster.Attack(skill);
                    };

                    monsterMenu.Items.Add(monsterMenuitem);

                    marker.ContextMenuStrip = monsterMenu;
                    break;

                case LocalPlayer:
                    marker.Image = Properties.Resources.mm_sign_character;
                    _toolTip.SetToolTip(marker, "We are here");

                    var localPlayerMenu = new ContextMenuStrip();
                    localPlayerMenu.MaximumSize = new Size(250, 400);

                    foreach (var skill in _localPlayer.Skills)
                    {
                        var menuitem = new ToolStripMenuItem
                        {
                            Text = skill.Name,
                            Name = skill.ToString()
                        };

                        menuitem.Click += (_, _) => skill.Cast();

                        localPlayerMenu.Items.Add(
                            menuitem
                        );
                    }

                    marker.ContextMenuStrip = localPlayerMenu;
                    break;

                case Player player:
                    if (player.Stall != null)
                    {
                        marker.Image = Properties.Resources.mm_sign_stall;
                        var stallMenu = new ContextMenuStrip();
                        var stallVisitItem = new ToolStripMenuItem
                        {
                            Text = "Open stall"
                        };

                        stallVisitItem.Click += (_, _) =>
                        {
                            Log("Opening " + player.Name + "'s stall");
                            player.Stall.Visit();
                        };
                        
                        var stallLeaveItem = new ToolStripMenuItem
                        {
                            Text = "Leave stall",
                        };

                        stallLeaveItem.Click += (_, _) =>
                        {
                            Log("Closing " + player.Name + "'s stall");
                            player.Stall.Leave();
                        };

                        stallMenu.Items.Add(stallVisitItem);
                        stallMenu.Items.Add(stallLeaveItem);
                        _toolTip.SetToolTip(marker, player.Stall.Title);
                        marker.ContextMenuStrip = stallMenu;
                        break;
                    }
                    
                    marker.Image = Properties.Resources.mm_sign_otherplayer;
                    
                    var playerMenu = new ContextMenuStrip();
                    var traceItem = new ToolStripMenuItem
                    {
                        Text = "Trace",
                    };

                    traceItem.Click += (_, _) =>
                    {
                        Log("Going to trace " + player.Name);
                        player.Trace();
                    };
                    
                    // todo: add casting buffs on player
                    // var buffItem = new ToolStripMenuItem
                    // {
                    //     Text = "Trace",
                    //     Name = player.ToString()
                    // };
                    //
                    // buffItem.Click += (_, _) =>
                    // {
                    //     Log("Going to trace " + player.Name);
                    //     player.Trace();
                    // };

                    playerMenu.Items.Add(traceItem);

                    marker.ContextMenuStrip = playerMenu;
                    break;

                case PickPet:
                case AttackPet:
                case FellowPet:
                case Horse:
                    marker.Image = Properties.Resources.mm_sign_animal;
                    break;

                case Teleport teleporter:
                    marker.Image = Properties.Resources.xy_gate;
                    if (teleporter.Links.IsNotEmpty())
                    {
                        var teleportMenu = new ContextMenuStrip();
                        foreach (var teleportLink in teleporter.Links)
                        {
                            var menuitem = new ToolStripMenuItem
                            {
                                Text = teleportLink.Name,
                                Tag = teleporter
                            };

                            menuitem.Click += (sender, args) => teleportLink.Teleport(teleporter);

                            teleportMenu.Items.Add(
                                menuitem
                            );
                        }

                        marker.ContextMenuStrip = teleportMenu;
                    }

                    break;
            }

            if (marker.Image == null)
            {
                return;
            }

            marker.Size = marker.Image.Size;

            var location = minimap.GetPoint(entity.WorldPoint);
            location.X -= marker.Image.Size.Width / 2;
            location.Y -= marker.Image.Size.Height / 2;
            marker.Location = location;

            marker.Tag = entity;

            minimap.AddMarker(entity.Uid, marker);
        }

        public void SetLocalPlayerMarkerAngle()
        {
            if (!minimap.Markers.ContainsKey(_localPlayer.Uid))
            {
                Console.WriteLine(_localPlayer.Uid);
                return;
            }

            var marker = minimap.Markers[_localPlayer.Uid];
            var image = Properties.Resources.mm_sign_character;
            var rotated = new Bitmap(image.Width, image.Height);
            var graphics = Graphics.FromImage(rotated);
            graphics.TranslateTransform((float) rotated.Width / 2, (float) rotated.Height / 2);
            graphics.RotateTransform(-_localPlayer.GetAngleDegrees());
            graphics.TranslateTransform(-(float) rotated.Width / 2, -(float) rotated.Height / 2);
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(image, new Point(0, 0));
            graphics.Dispose();
                
            marker.InvokeLater(() => { marker.Image = rotated; });
        }

        public void RefreshMap(bool force = false)
        {
            try
            {
                if (!mapPanel.Visible && !force)
                {
                    return;
                }

                minimap.SetView(_localPlayer.WorldPoint, force);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void ClearMarkers()
        {
            minimap.ClearMarkers();
        }
        
        public void ClearTiles()
        {
            minimap.RemoveTiles();
        }

        public void RefreshMarkers()
        {
            minimap.UpdateMarkerLocations();
        }

        public void RemoveMinimapMarker(uint uid)
        {
            minimap.RemoveMarker(uid);
        }
    }
}