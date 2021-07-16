using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using SimpleCL.Models.Character;
using SimpleCL.Models.Entities;
using SimpleCL.Models.Entities.Npcs;
using SimpleCL.Models.Entities.Pets;
using SimpleCL.Models.Entities.Teleporters;
using SimpleCL.Ui.Components;
using SimpleCL.Util.Extension;
using Timer = System.Timers.Timer;

namespace SimpleCL.Ui
{
    partial class Gui
    {
        private void InitMap()
        {
            var mapTimer = new Timer(10000);
            mapTimer.Elapsed += (_, _) =>
            {
                if (_localPlayer.Uid == 0)
                {
                    return;
                }

                RefreshMap(true);
            };
            
            mapTimer.Start();

            mapVisibleCheckbox.CheckedChanged += (_, _) => minimap.Visible = mapVisibleCheckbox.Checked;
        }

        public void AddMinimapMarker(Entity entity)
        {
            var marker = new MapControl();
            _toolTip.SetToolTip(marker, entity.ToString());

            switch (entity)
            {
                case Shop shop:
                    marker.Image = (Bitmap) Properties.Resources.ResourceManager.GetObject(shop.MapIcon);
                    var shopMenu = new ContextMenuStrip();
                    var shopMenuItem = new ToolStripMenuItem
                    {
                        Text = "Open shop"
                    };

                    shopMenuItem.Click += (_, _) =>
                    {
                        Log($"Opening shop [{shop.Name}]");
                        shop.Select();
                    };

                    shopMenu.Items.Add(shopMenuItem);
                    marker.ContextMenuStrip = shopMenu;
                    break;
                
                case TalkNpc talkNpc:
                    marker.Image = (Bitmap) Properties.Resources.ResourceManager.GetObject(talkNpc.MapIcon);
                    break;

                case Monster monster:
                    marker.Image = Properties.Resources.mm_sign_monster;
                    var monsterMenu = new ContextMenuStrip();
                    monsterMenu.MaximumSize = new Size(250, 400);
                    
                    var monsterMenuitem = new ToolStripMenuItem
                    {
                        Text = "Attack"
                    };

                    monsterMenuitem.Click += (_, _) =>
                    {
                        var skill = SelectedSkills.FirstOrDefault(x => !x.IsOnCooldown());
                        Log($"Attacking [{monster.Name}] with skill [{skill?.Name}]");
                        monster.Attack(skill);
                    };
                    
                    var castItem = new ToolStripMenuItem
                    {
                        Text = "Cast"
                    };
                    
                    foreach (var skill in _localPlayer.Skills.Where(x => x.IsAttackSkill() && x.Targeted))
                    {
                        var menuitem = new ToolStripMenuItem
                        {
                            Text = skill.Name
                        };

                        menuitem.Click += (_, _) =>
                        {
                            monster.Attack(skill);
                            Log($"Attacking monster [{monster.Name}] with skill [{skill.Name}]");
                        };

                        castItem.DropDownItems.Add(
                            menuitem
                        );
                    }

                    monsterMenu.Items.Add(monsterMenuitem);
                    monsterMenu.Items.Add(castItem);

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
                            Text = skill.Name
                        };

                        menuitem.Click += (_, _) =>
                        {
                            skill.Cast();
                        };

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
                            player.Stall.Visit();
                            Log($"Opening [{player.Name}]'s stall");
                        };
                        
                        var stallLeaveItem = new ToolStripMenuItem
                        {
                            Text = "Leave stall",
                        };

                        stallLeaveItem.Click += (_, _) =>
                        {
                            player.Stall.Leave();
                            Log($"Closing [{player.Name}]'s stall");
                        };

                        stallMenu.Items.Add(stallVisitItem);
                        stallMenu.Items.Add(stallLeaveItem);
                        _toolTip.SetToolTip(marker, player.Stall.Title);
                        marker.ContextMenuStrip = stallMenu;
                        break;
                    }

                    if (player.LifeState == Actor.Health.LifeState.Dead)
                    {
                        marker.Image = Properties.Resources.mm_sign_skull;
                        marker.Image = Properties.Resources.mm_sign_skull;
                    
                        var resSkills = _localPlayer.Skills.Where(x => x.IsResSkill()).ToList();
                        if (resSkills.Any())
                        {
                            var resMenu = new ContextMenuStrip();
                            var resSubMenu = new ToolStripMenuItem("Resurrect");
                        
                            foreach (var resSkill in resSkills)
                            {
                                var resItem = new ToolStripMenuItem
                                {
                                    Text = resSkill.Name
                                };

                                resItem.Click += (_, _) =>
                                {
                                    player.Attack(resSkill);
                                    Log($"Ressing [{player.Name}] with skill [{resSkill.Name}]");
                                };

                                resSubMenu.DropDownItems.Add(
                                    resItem
                                );
                            }

                            marker.ContextMenuStrip = resMenu;
                        }
                        else
                        {
                            marker.ContextMenuStrip = null;
                        }
                        
                        break;
                    }
                    
                    marker.Image = Properties.Resources.mm_sign_otherplayer;
                    
                    var playerMenu = new ContextMenuStrip();
                    var traceItem = new ToolStripMenuItem
                    {
                        Text = "Trace"
                    };

                    traceItem.Click += (_, _) =>
                    {
                        player.Trace();
                        Log($"Going to trace [{player.Name}]");
                    };
                    
                    var buffItem = new ToolStripMenuItem
                    {
                        Text = "Buff"
                    };
                    
                    foreach (var skill in _localPlayer.Skills.Where(x => !x.IsAttackSkill() && x.Targeted))
                    {
                        var menuitem = new ToolStripMenuItem
                        {
                            Text = skill.Name
                        };

                        menuitem.Click += (_, _) =>
                        {
                            player.Attack(skill);
                            Log($"Casting buff [{skill.Name}] on [{player.Name}]");
                        };

                        buffItem.DropDownItems.Add(
                            menuitem
                        );
                    }
                    
                    var attackItem = new ToolStripMenuItem
                    {
                        Text = "Attack"
                    };
                    
                    foreach (var skill in _localPlayer.Skills.Where(x => x.IsAttackSkill() && x.Targeted))
                    {
                        var menuitem = new ToolStripMenuItem
                        {
                            Text = skill.Name
                        };

                        menuitem.Click += (_, _) =>
                        {
                            player.Attack(skill);
                            Log($"Attacking [{player.Name}] with skill [{skill.Name}]");
                        };

                        attackItem.DropDownItems.Add(
                            menuitem
                        );
                    }
                    
                    playerMenu.Items.Add(attackItem);
                    playerMenu.Items.Add(buffItem);
                    playerMenu.Items.Add(traceItem);

                    marker.ContextMenuStrip = playerMenu;
                    break;

                case CharacterPet pet:
                    marker.Image = Properties.Resources.mm_sign_animal;
                    
                    if (pet.OwnerUid != _localPlayer.Uid)
                    {
                        break;
                    }
                    
                    var summonMenu = new ContextMenuStrip();
                    var terminateItem = new ToolStripMenuItem("Unsummon");
                    
                    terminateItem.Click += (_, _) =>
                    {
                        pet.Unsummon();
                        Log($"Recalling pet [{pet.Name}]");
                    };
                    
                    summonMenu.Items.Add(terminateItem);
                    marker.ContextMenuStrip = summonMenu;
                    break;
                
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

                            menuitem.Click += (_, _) =>
                            {
                                teleportLink.Teleport(teleporter);
                                Log($"Teleporting to [{teleportLink.Name}]");
                            };

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
            entity.MarkerSize = marker.Image.Size;
            marker.DataBindings.Add("Location", entity, "MapLocation");

            marker.Tag = entity;

            minimap.AddMarker(entity.Uid, marker);
        }

        public void RefreshPlayerMarker(uint uid)
        {
            minimap.UpdatePlayerMarker(uid);
        }

        public void SetLocalPlayerMarkerAngle()
        {
            if (!minimap.Markers.ContainsKey(_localPlayer.Uid))
            {
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

        public void RemoveMinimapMarker(uint uid)
        {
            minimap.RemoveMarker(uid);
        }
    }
}