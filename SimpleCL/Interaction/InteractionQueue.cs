using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using SimpleCL.Interaction.Providers;
using SimpleCL.Models;
using SimpleCL.Models.Character;
using SimpleCL.Models.Coordinates;
using SimpleCL.Models.Entities;
using SimpleCL.SecurityApi;

namespace SimpleCL.Interaction
{
    public class InteractionQueue
    {
        private static InteractionQueue _instance;
        public static InteractionQueue Get => _instance ??= new InteractionQueue();

        public static readonly ConcurrentQueue<Packet> PacketQueue = new();
        public static bool AttackLoop;

        private InteractionQueue()
        {
            new Thread(() =>
            {
                while (true)
                {
                    var local = LocalPlayer.Get;

                    if (local == null || local.Uid == 0)
                    {
                        Thread.Sleep(1);
                        continue;
                    }

                    if (!AttackLoop)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }

                    var selectedEntity = Program.Gui.SelectedEntities.FirstOrDefault();
                    if (selectedEntity == null)
                    {
                        Thread.Sleep(1);
                        continue;
                    }

                    var attackableEntity = (ITargetable) Entities.AllEntities.Values
                        .OrderBy(x => ((ILocatable) x).WorldPoint.DistanceTo(LocalPlayer.Get.WorldPoint))
                        .FirstOrDefault(x =>
                        {
                            if (x is not ITargetable)
                            {
                                return false;
                            }

                            return selectedEntity switch
                            {
                                Player player => x is Player p && player.Uid == p.Uid,
                                Monster monster => x is Monster m && monster.Id == m.Id && m.Hp > 0,
                                _ => false
                            };
                        });

                    if (attackableEntity == null)
                    {
                        Thread.Sleep(1);
                        continue;
                    }

                    var selectedSkill = Program.Gui.SelectedSkills.FirstOrDefault(x => !x.IsOnCooldown());
                    if (selectedSkill == null)
                    {
                        Thread.Sleep(1);
                        continue;
                    }

                    attackableEntity.Attack(selectedSkill);
                    Thread.Sleep(1000);
                }
            }).Start();
        }

        public void StartLoop()
        {
            if (AttackLoop)
            {
                AttackLoop = false;
                return;
            }

            AttackLoop = true;
        }
    }
}