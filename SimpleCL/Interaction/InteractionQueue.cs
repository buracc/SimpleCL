using System.Collections.Concurrent;
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