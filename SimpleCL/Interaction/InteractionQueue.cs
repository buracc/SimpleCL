using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SimpleCL.Interaction.Providers;
using SimpleCL.Models.Character;
using SimpleCL.Models.Coordinates;
using SimpleCL.Models.Entities;
using SimpleCL.SecurityApi;

namespace SimpleCL.Interaction
{
    public class InteractionQueue
    {
        private static InteractionQueue _instance;
        public static InteractionQueue Get => _instance ?? (_instance = new InteractionQueue());
        
        public static readonly ConcurrentQueue<Packet> PacketQueue = new ConcurrentQueue<Packet>();
        public static bool Looping;
        private readonly Thread _loopThread;

        private InteractionQueue()
        {
            _loopThread = new Thread(() =>
            {
                while (Looping)
                {
                    var selectedEntity = Program.Gui.SelectedEntities.FirstOrDefault();
                    if (selectedEntity == null)
                    {
                        continue;
                    }
                    
                    var attackableEntity = Entities.TargetableEntities
                        .OrderBy(x => ((ILocatable) x).WorldPoint.DistanceTo(LocalPlayer.Get.WorldPoint))
                        .FirstOrDefault(x =>
                    {
                        switch (selectedEntity)
                        {
                            case Player player:
                                return x is Player p && player.Uid == p.Uid;
                            case Monster monster:
                                return x is Monster m && monster.Id == m.Id;
                            default:
                                return false;
                        }
                    });

                    if (attackableEntity == null)
                    {
                        continue;
                    }
            
                    var selectedSkill = Program.Gui.SelectedSkills.FirstOrDefault(x => !x.IsOnCooldown());
                    if (selectedSkill == null)
                    {
                        Thread.Sleep(500);
                        continue;
                    }

                    Console.WriteLine("attacking: " + attackableEntity + " with " + selectedSkill);
                
                    attackableEntity.Cast(selectedSkill);
                    Thread.Sleep(500);
                }
            });
        }
        
        public void StartLoop()
        {
            if (Looping)
            {
                Looping = false;
                return;
            }
            
            Looping = true;
            _loopThread.Start();
        }
    }
}