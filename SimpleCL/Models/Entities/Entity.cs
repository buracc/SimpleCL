using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using SimpleCL.Annotations;
using SimpleCL.Database;
using SimpleCL.Models.Coordinates;
using SimpleCL.Models.Entities.Fortress;
using SimpleCL.Models.Entities.Fortress.Structure;
using SimpleCL.Models.Entities.Mob;
using SimpleCL.Models.Entities.Pet;
using SimpleCL.Models.Entities.Teleporters;
using SimpleCL.Models.Exceptions;
using SimpleCL.Util.Extension;

namespace SimpleCL.Models.Entities
{
    public class Entity : ILocatable, IIdentifiable, INotifyPropertyChanged
    {
        #region Members

        private LocalPoint _localPoint;
        private string _name;

        #endregion

        #region Properties

        public readonly uint Id;
        public readonly string ServerName;
        public readonly byte TypeId1;
        public readonly byte TypeId2;
        public readonly byte TypeId3;
        public readonly byte TypeId4;
        
        public uint Uid { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        
        public LocalPoint LocalPoint
        {
            get => _localPoint;
            set
            {
                _localPoint = value;
                OnPropertyChanged(nameof(LocalPoint));
                OnPropertyChanged(nameof(WorldPoint));
                OnPropertyChanged(nameof(MapLocation));
            }
        }
        
        public WorldPoint WorldPoint => WorldPoint.FromLocal(LocalPoint);
        
        protected readonly NameValueCollection DatabaseData;

        #endregion

        #region UI Properties

        public Size MarkerSize { get; set; }
        public Point MapLocation
        {
            get
            {
                var point = Program.Gui.GetMap().GetPoint(WorldPoint);
                point.X -= MarkerSize.Width / 2;
                point.Y -= MarkerSize.Height / 2;
                return point;
            }
        }

        #endregion

        #region Constructor

        public Entity(uint id, string serverName, byte typeId1, byte typeId2, byte typeId3, byte typeId4, string name)
        {
            Id = id;
            ServerName = serverName;
            TypeId1 = typeId1;
            TypeId2 = typeId2;
            TypeId3 = typeId3;
            TypeId4 = typeId4;
            Name = name;
        }

        public Entity(uint id, QueryBuilder queryBuilder = null)
        {
            Id = id;

            switch (id)
            {
                case uint.MaxValue:
                    return;
                case > ushort.MaxValue:
                    throw new EntityParseException("Entity id longer than expected: " + id);
            }

            if ((DatabaseData = GameDatabase.Get.GetModel(id, queryBuilder)) != null)
            {
                TypeId1 = 1;
            }
            else if ((DatabaseData = GameDatabase.Get.GetItemData(id, queryBuilder)) != null)
            {
                TypeId1 = 3;
            }
            else
            {
                var teleLinks = GameDatabase.Get.GetTeleportLinks(id, queryBuilder);
                if (teleLinks.IsNotEmpty())
                {
                    DatabaseData = teleLinks[0];
                    TypeId1 = 4;
                }
            }

            if (DatabaseData != null)
            {
                var tid2 = TypeId1 == 3 ? "tid1" : "tid2";
                var tid3 = TypeId1 == 3 ? "tid2" : "tid3";
                var tid4 = TypeId1 == 3 ? "tid3" : "tid4";
                ServerName = DatabaseData["servername"];
                Name = this is Player ? "Weed" : DatabaseData["name"];
                TypeId2 = byte.Parse(DatabaseData[tid2]);
                TypeId3 = byte.Parse(DatabaseData[tid3]);
                TypeId4 = byte.Parse(DatabaseData[tid4]);
            }
            else
            {
                throw new EntityParseException("Failed to parse entity: " + id);
            }
        }

        #endregion

        #region Methods

        public static Entity FromId(uint id, QueryBuilder queryBuilder = null)
        {
            var entity = new Entity(id, queryBuilder);
            if (entity.IsSkillAoe())
            {
                return new SkillAoe(id);
            }

            if (entity.IsActor())
            {
                var actor = new Actor(id);
                if (actor.IsPlayer())
                {
                    return new Player(id);
                }

                if (actor.IsNpc())
                {
                    var npc = new Npc(id);
                    if (npc.IsMonster())
                    {
                        var monster = new Monster(id);
                        if (monster.IsFlower())
                        {
                            return new Flower(id);
                        }

                        if (monster.IsThiefCaravan())
                        {
                            return new ThiefCaravan(id);
                        }

                        if (monster.IsTraderCaravan())
                        {
                            return new TraderCaravan(id);
                        }

                        return monster;
                    }

                    if (npc.IsTalk())
                    {
                        return new TalkNpc(id);
                    }

                    if (npc.IsCos())
                    {
                        var cos = new Cos(id);
                        if (cos.IsHorse())
                        {
                            return new Horse(id);
                        }

                        if (cos.IsTransport())
                        {
                            return new Transport(id);
                        }

                        if (cos.IsAttackPet())
                        {
                            return new AttackPet(id);
                        }

                        if (cos.IsPickPet())
                        {
                            return new PickPet(id);
                        }

                        if (cos.IsGuildGuard())
                        {
                            return new GuildGuard(id);
                        }

                        if (cos.IsQuestPet())
                        {
                            return new QuestPet(id);
                        }

                        if (cos.IsFellowPet())
                        {
                            return new FellowPet(id);
                        }
                    }

                    if (npc.IsFortressCos())
                    {
                        var fortressCos = new FortressCos(id);
                        if (fortressCos.IsPatrolGuard())
                        {
                            return new FortressPatrolGuard(id);
                        }

                        if (fortressCos.IsFortressFlag())
                        {
                            return new FortressFlag(id);
                        }

                        if (fortressCos.IsFortressMonster())
                        {
                            return new FortressMonster(id);
                        }

                        if (fortressCos.IsDefenseGuard())
                        {
                            return new FortressDefenseGuard(id);
                        }
                    }

                    if (npc.IsFortressStructure())
                    {
                        var structure = new FortressStructure(id);
                        if (structure.IsHeart())
                        {
                            return new FortressHeart(id);
                        }

                        if (structure.IsTower())
                        {
                            return new FortressTower(id);
                        }

                        if (structure.IsGate())
                        {
                            return new FortressGate(id);
                        }

                        if (structure.IsDefenseCamp())
                        {
                            return new FortressDefenseCamp(id);
                        }

                        if (structure.IsCommandPost())
                        {
                            return new FortressCommandPost(id);
                        }

                        if (structure.IsObstacle())
                        {
                            return new FortressObstacle(id);
                        }
                    }
                }
            }

            if (entity.IsGroundItem())
            {
                return new GroundItem(id);
            }

            return entity.IsTeleport() ? new Teleport(id) : entity;
        }

        public bool IsSkillAoe()
        {
            return Id == uint.MaxValue;
        }

        public bool IsActor()
        {
            return TypeId1 == 1;
        }

        public bool IsGroundItem()
        {
            return TypeId1 == 3;
        }

        public bool IsTeleport()
        {
            return TypeId1 == 4;
        }

        public override bool Equals(object obj)
        {
            return obj is Entity other && other.Uid == Uid;
        }

        protected bool Equals(Entity other)
        {
            return Uid == other.Uid;
        }

        public override int GetHashCode()
        {
            return (int) Uid;
        }

        public override string ToString()
        {
            return GetType().Name + ": " + Name + " [" + Id + "] [" + Uid + "]";
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}