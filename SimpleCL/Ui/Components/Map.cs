using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SimpleCL.Interaction.Pathing;
using SimpleCL.Models.Coordinates;
using SimpleCL.Models.Entities;
using SimpleCL.Util.Extension;

namespace SimpleCL.Ui.Components
{
    public class Map : Panel
    {
        private readonly Dictionary<string, MapTile> _mapSectors = new();
        public readonly Dictionary<uint, MapControl> Markers = new();

        private string _filePath;
        private Size _tileSize;
        private int _tileCount;

        public Size TileSize => _tileSize;
        public int TileCount => _tileCount;
        public WorldPoint MapCenter { get; private set; }

        private byte _zoom;

        public Map()
        {
            base.DoubleBuffered = true;
            MapCenter = new WorldPoint(0, 0);
            Zoom = 0;
            base.Size = new Size(600, 600);
            _tileSize = new Size((int) Math.Round(Width / (2.0 * _zoom + 1), MidpointRounding.AwayFromZero),
                (int) Math.Round(Height / (2.0 * _zoom + 1), MidpointRounding.AwayFromZero));
            _tileCount = 2 * _zoom + 3;

            MouseWheel += (sender, args) =>
            {
                var change = args.Delta;
                Zoom = change > 0 ? (byte) 0 : (byte) 1;
            };

            SelectMapLayer(MapCenter.Region);
            UpdateTiles();
        }

        public new Size Size
        {
            get => base.Size;
            set
            {
                base.Size = value;
                _tileSize = new Size((int) Math.Round(Width / (2.0 * _zoom + 1), MidpointRounding.AwayFromZero),
                    (int) Math.Round(Height / (2.0 * _zoom + 1), MidpointRounding.AwayFromZero));
                _tileCount = 2 * _zoom + 3;
                RemoveTiles();
                UpdateTiles();
                UpdateMarkerLocations();
            }
        }

        public byte Zoom
        {
            set
            {
                if (_zoom.Equals(value))
                {
                    return;
                }

                _zoom = value;
                _tileSize = new Size((int) Math.Round(Width / (2.0 * _zoom + 1), MidpointRounding.AwayFromZero),
                    (int) Math.Round(Height / (2.0 * _zoom + 1), MidpointRounding.AwayFromZero));
                _tileCount = 2 * _zoom + 3;

                RemoveTiles();
                UpdateTiles();
                UpdateMarkerLocations();
            }
        }

        private void SelectMapLayer(ushort region)
        {
            switch (region)
            {
                case 32769:
                    _filePath = MapCenter.Z switch
                    {
                        < 115 => "Minimap/d/dh_a01_floor01_{0}x{1}.jpg",
                        < 230 => "Minimap/d/dh_a01_floor02_{0}x{1}.jpg",
                        < 345 => "Minimap/d/dh_a01_floor03_{0}x{1}.jpg",
                        _ => "Minimap/d/dh_a01_floor04_{0}x{1}.jpg"
                    };
                    break;
                case 32770:
                    _filePath = "Minimap/d/qt_a01_floor06_{0}x{1}.jpg";
                    break;
                case 32771:
                    _filePath = "Minimap/d/qt_a01_floor05_{0}x{1}.jpg";
                    break;
                case 32772:
                    _filePath = "Minimap/d/qt_a01_floor04_{0}x{1}.jpg";
                    break;
                case 32773:
                    _filePath = "Minimap/d/qt_a01_floor03_{0}x{1}.jpg";
                    break;
                case 32774:
                    _filePath = "Minimap/d/qt_a01_floor02_{0}x{1}.jpg";
                    break;
                case 32775:
                    _filePath = "Minimap/d/qt_a01_floor01_{0}x{1}.jpg";
                    break;
                case 32778:
                case 32779:
                case 32780:
                case 32781:
                case 32782:
                case 32784:
                    _filePath = "Minimap/d/RN_SD_EGYPT1_01_{0}x{1}.jpg";
                    break;
                case 32783:
                    _filePath = "Minimap/d/RN_SD_EGYPT1_02_{0}x{1}.jpg";
                    break;
                case 32785:
                    _filePath = MapCenter.Z switch
                    {
                        < 115 => "Minimap/d/fort_dungeon01_{0}x{1}.jpg",
                        < 230 => "Minimap/d/fort_dungeon02_{0}x{1}.jpg",
                        < 345 => "Minimap/d/fort_dungeon03_{0}x{1}.jpg",
                        _ => "Minimap/d/fort_dungeon04_{0}x{1}.jpg"
                    };
                    break;
                case 32786:
                    _filePath = "Minimap/d/flame_dungeon01_{0}x{1}.jpg";
                    break;
                case 32787:
                    _filePath = "Minimap/d/RN_JUPITER_02_{0}x{1}.jpg";
                    break;
                case 32788:
                    _filePath = "Minimap/d/RN_JUPITER_03_{0}x{1}.jpg";
                    break;
                case 32789:
                    _filePath = "Minimap/d/RN_JUPITER_04_{0}x{1}.jpg";
                    break;
                case 32790:
                    _filePath = "Minimap/d/RN_JUPITER_01_{0}x{1}.jpg";
                    break;
                case 32793:
                    _filePath = "Minimap/d/RN_ARABIA_FIELD_02_BOSS_{0}x{1}.jpg";
                    break;
                default:
                    _filePath = "Minimap/{0}x{1}.jpg";
                    break;
            }
        }

        public void UpdateTiles()
        {
            this.InvokeLater(() =>
            {
                var tileAvg = _tileCount / 2;
                var relativePosX = (int) Math.Round(MapCenter.X % 192 * _tileSize.Width / 192.0 +
                                                    (MapCenter.X < 0 ? _tileSize.Width : 0));
                var relativePosY = (int) Math.Round(MapCenter.Y % 192 * _tileSize.Height / 192.0 +
                                                    (MapCenter.Y < 0 ? _tileSize.Height : 0));
                var marginX = (int) Math.Round(_tileSize.Width / 2.0 - _tileSize.Width - relativePosX);
                var marginY = (int) Math.Round(_tileSize.Height / 2.0 - _tileSize.Height * 2 + relativePosY);

                var i = 0;
                for (var sectorY = tileAvg + MapCenter.YSector; sectorY >= -tileAvg + MapCenter.YSector; sectorY--)
                {
                    var j = 0;
                    for (var sectorX = -tileAvg + MapCenter.XSector;
                        sectorX <= tileAvg + MapCenter.XSector;
                        sectorX++)
                    {
                        var path = string.Format(_filePath, sectorX, sectorY);
                        var sectorLocation =
                            new Point(j * _tileSize.Width + marginX, i * _tileSize.Height + marginY);

                        if (_mapSectors.TryGetValue(path, out var sector))
                        {
                            if (sector.Location.X != sectorLocation.X || sector.Location.Y != sectorLocation.Y)
                            {
                                sector.Location = sectorLocation;
                            }

                            if (_tileSize.Width != sector.Size.Width || _tileSize.Height != sector.Size.Height)
                            {
                                sector.Size = _tileSize;
                            }

                            sector.Visible = true;
                        }
                        else
                        {
                            var x = sectorX;
                            var y = sectorY;
                            sector = new MapTile(x, y)
                            {
                                Name = path, Size = _tileSize, Location = sectorLocation
                            };

                            sector.MouseClick += MapClicked;

                            sector.LoadAsyncTile(path, _tileSize);
                            _mapSectors[path] = sector;
                            Controls.Add(sector);
                            sector.SendToBack();
                        }

                        j++;
                    }

                    i++;
                }
            });
        }

        public void ClearTiles()
        {
            this.InvokeLater(() =>
            {
                var minAvg = _tileCount / 2;

                var ySectorMin = -minAvg + MapCenter.YSector;
                var ySectorMax = minAvg + MapCenter.YSector;
                var xSectorMin = -minAvg + MapCenter.XSector;
                var xSectorMax = minAvg + MapCenter.XSector;

                foreach (var tile in _mapSectors.Values.Where(tile =>
                    tile.SectorX < xSectorMin || tile.SectorX > xSectorMax || tile.SectorY < ySectorMin ||
                    tile.SectorY > ySectorMax))
                {
                    tile.Visible = false;
                }
            });
        }

        public void RemoveTiles()
        {
            this.InvokeLater(() =>
            {
                foreach (var tile in _mapSectors.Values)
                {
                    Controls.RemoveByKey(tile.Name);
                }

                _mapSectors.Clear();
            });
        }

        public void SetView(WorldPoint viewPoint, bool force = false)
        {
            if (!MapCenter.Equals(viewPoint) || force)
            {
                if (!MapCenter.Region.Equals(viewPoint.Region) && viewPoint.InCave())
                {
                    SelectMapLayer(viewPoint.Region);
                    RemoveTiles();
                }
                else
                {
                    MapCenter = viewPoint;
                    ClearTiles();
                }

                UpdateTiles();
            }

            UpdateMarkerLocations();
        }

        public Point GetPoint(WorldPoint coords)
        {
            var tileAvg = _tileCount / 2;

            return new Point
            {
                X = (int) Math.Round((coords.X - MapCenter.X) / (192.0 / _tileSize.Width) +
                    _tileSize.Width * tileAvg - _tileSize.Width / 2.0),
                Y = (int) Math.Round((coords.Y - MapCenter.Y) / (192.0 / _tileSize.Height) * (-1) +
                    _tileSize.Height * tileAvg - _tileSize.Height / 2.0)
            };
        }

        public LocalPoint GetCoord(Point point)
        {
            var tileAvg = _tileCount / 2;
            var coordX =
                (point.X + _tileSize.Width / 2.0f - _tileSize.Width * tileAvg) * 192 / _tileSize.Width +
                MapCenter.X;
            var coordY =
                (point.Y + _tileSize.Height / 2.0f - _tileSize.Height * tileAvg) * 192 / _tileSize.Height * -1 +
                MapCenter.Y;

            return MapCenter.InCave()
                ? new LocalPoint(MapCenter.Region, coordX, MapCenter.Z, coordY)
                : LocalPoint.FromWorld(new WorldPoint(coordX, coordY));
        }

        public void UpdatePlayerMarker(uint uid)
        {
            this.InvokeLater(() =>
            {
                if (!Markers.TryGetValue(uid, out var marker) || marker.Tag is not Player player)
                {
                    return;
                }

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
                        Program.Gui.Log("Opening " + player.Name + "'s stall");
                        player.Stall.Visit();
                    };

                    var stallLeaveItem = new ToolStripMenuItem
                    {
                        Text = "Leave stall",
                    };

                    stallLeaveItem.Click += (_, _) =>
                    {
                        Program.Gui.Log("Closing " + player.Name + "'s stall");
                        player.Stall.Leave();
                    };

                    stallMenu.Items.Add(stallVisitItem);
                    stallMenu.Items.Add(stallLeaveItem);
                    new ToolTip().SetToolTip(marker, player.Stall.Title);
                    marker.ContextMenuStrip = stallMenu;
                }
                else
                {
                    marker.Image = Properties.Resources.mm_sign_otherplayer;

                    var playerMenu = new ContextMenuStrip();
                    var traceItem = new ToolStripMenuItem
                    {
                        Text = "Trace",
                    };

                    traceItem.Click += (_, _) =>
                    {
                        Program.Gui.Log("Going to trace " + player.Name);
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
                }
            });
        }

        public void AddMarker(uint uid, MapControl marker)
        {
            marker.Name = Name + "_" + uid;
            this.InvokeLater(() =>
            {
                Controls.Add(marker);
                Controls.SetChildIndex(marker, 1);
                Markers[uid] = marker;
            });
        }

        public void ClearMarkers()
        {
            this.InvokeLater(() =>
            {
                foreach (var keyValuePair in Markers.Where(
                    keyValuePair => Controls.ContainsKey(keyValuePair.Value.Name)))
                {
                    Controls.RemoveByKey(keyValuePair.Value.Name);
                }
            });

            Markers.Clear();
        }

        public void RemoveMarker(uint uniqueId)
        {
            if (!Markers.ContainsKey(uniqueId))
            {
                return;
            }

            var marker = Markers[uniqueId];
            this.InvokeLater(() => { Controls.RemoveByKey(marker.Name); });

            Markers.Remove(uniqueId);
        }

        public void UpdateMarkerLocations()
        {
            var aX = _tileSize.Width * (_zoom + 3 / 2) - _tileSize.Width / 2.0;
            var aY = _tileSize.Height * (_zoom + 3 / 2) - _tileSize.Height / 2.0;
            var bX = 192.0 / _tileSize.Width;
            var bY = 192.0 / _tileSize.Height;

            this.InvokeLater(() =>
            {
                foreach (var marker in Markers.Values)
                {
                    var coord = ((ILocatable) marker.Tag).WorldPoint;
                    var location = new Point((int) Math.Round((coord.X - MapCenter.X) / bX + aX),
                        (int) Math.Round((coord.Y - MapCenter.Y) / bY * -1 + aY));
                    var imageSize = marker.Image.Size;

                    location.X -= imageSize.Width / 2;
                    location.Y -= imageSize.Height / 2;

                    if (!marker.Location.X.Equals(location.X) && !marker.Location.Y.Equals(location.Y))
                    {
                        marker.Location = location;
                    }
                }
            });
        }

        private void MapClicked(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            var t = (MapTile) sender;
            var clickPoint = new Point(t.Location.X + e.Location.X, t.Location.Y + e.Location.Y);
            var coord = GetCoord(clickPoint);
            var world = WorldPoint.FromLocal(coord);
            Movement.WalkTo(coord);
            Program.Gui.Log("Moving to [" + world + "]");
        }
    }
}