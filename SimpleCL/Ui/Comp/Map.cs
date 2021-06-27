using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SimpleCL.Interaction;
using SimpleCL.Interaction.Pathing;
using SimpleCL.Model.Coord;
using SimpleCL.Model.Entity;

namespace SimpleCL.Ui.Comp
{
    public class Map : Panel
    {

        private readonly Dictionary<string, MapTile> _mapSectors = new Dictionary<string, MapTile>();
        private readonly Dictionary<uint, MapControl> _markers = new Dictionary<uint, MapControl>();

        private WorldPoint _mapCenter;
        private string _filePath;
        private byte _zoom;
        private Size _tileSize;
        private int _tileCount;
        
        public Size TileSize => _tileSize;
        public int TileCount => _tileCount;
        public WorldPoint MapCenter => _mapCenter;

        public Map()
        {
            // Initialize
            base.DoubleBuffered = true;
            _mapCenter = new WorldPoint(0, 0);
            _zoom = 0;
            base.Size = new Size(762, 250);
            _tileSize = new Size((int) Math.Round(Width / (2.0 * _zoom + 1), MidpointRounding.AwayFromZero),
                (int) Math.Round(Height / (2.0 * _zoom + 1), MidpointRounding.AwayFromZero));
            _tileCount = 2 * _zoom + 3;

            SelectMapLayer(_mapCenter.Region);
            UpdateTiles();
        }

        public new Size Size
        {
            get => base.Size;
            set
            {
                base.Size = value;
                _tileSize =
                    new Size((int) Math.Round(Width / (2.0 * _zoom + 1), MidpointRounding.AwayFromZero),
                        (int) Math.Round(Height / (2.0 * _zoom + 1), MidpointRounding.AwayFromZero));
                RemoveTiles();
                UpdateTiles();
                UpdateMarkerLocations();
            }
        }
        
        public byte Zoom
        {
            get => _zoom;
            set
            {
                if (value != _zoom)
                {
                    _zoom = value;
                    _tileSize =
                        new Size((int) Math.Round(Width / (2.0 * _zoom + 1), MidpointRounding.AwayFromZero),
                            (int) Math.Round(Height / (2.0 * _zoom + 1), MidpointRounding.AwayFromZero));
                    _tileCount = 2 * _zoom + 3;
                    RemoveTiles();
                    UpdateTiles();
                    UpdateMarkerLocations();
                }
            }
        }

        private void SelectMapLayer(ushort region)
        {
            switch (region)
            {
                case 32769:
                    if (_mapCenter.Z < 115)
                        _filePath = "Minimap/d/dh_a01_floor01_{0}x{1}.jpg";
                    else if (_mapCenter.Z < 230)
                        _filePath = "Minimap/d/dh_a01_floor02_{0}x{1}.jpg";
                    else if (_mapCenter.Z < 345)
                        _filePath = "Minimap/d/dh_a01_floor03_{0}x{1}.jpg";
                    else
                        _filePath = "Minimap/d/dh_a01_floor04_{0}x{1}.jpg";
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
                    if (_mapCenter.Z < 115)
                        _filePath = "Minimap/d/fort_dungeon01_{0}x{1}.jpg";
                    else if (_mapCenter.Z < 230)
                        _filePath = "Minimap/d/fort_dungeon02_{0}x{1}.jpg";
                    else if (_mapCenter.Z < 345)
                        _filePath = "Minimap/d/fort_dungeon03_{0}x{1}.jpg";
                    else
                        _filePath = "Minimap/d/fort_dungeon04_{0}x{1}.jpg";
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
            int tileAvg = _tileCount / 2;
            int relativePosX = (int) Math.Round((_mapCenter.X % 192) * _tileSize.Width / 192.0 +
                                                (_mapCenter.X < 0 ? _tileSize.Width : 0));
            int relativePosY = (int) Math.Round((_mapCenter.Y % 192) * _tileSize.Height / 192.0 +
                                                (_mapCenter.Y < 0 ? _tileSize.Height : 0));
            int marginX = (int) Math.Round(_tileSize.Width / 2.0 - _tileSize.Width - relativePosX);
            int marginY = (int) Math.Round(_tileSize.Height / 2.0 - _tileSize.Height * 2 + relativePosY);

            int i = 0;
            for (int sectorY = tileAvg + _mapCenter.YSector; sectorY >= -tileAvg + _mapCenter.YSector; sectorY--)
            {
                int j = 0;
                for (int sectorX = -tileAvg + _mapCenter.XSector;
                    sectorX <= tileAvg + _mapCenter.XSector;
                    sectorX++)
                {
                    string path = string.Format(_filePath, sectorX, sectorY);
                    Point sectorLocation =
                        new Point(j * _tileSize.Width + marginX, i * _tileSize.Height + marginY);

                    if (_mapSectors.TryGetValue(path, out var sector))
                    {
                        if (sector.Location.X != sectorLocation.X || sector.Location.Y != sectorLocation.Y)
                            sector.Location = sectorLocation;
                        if (_tileSize.Width != sector.Size.Width || _tileSize.Height != sector.Size.Height)
                            sector.Size = _tileSize;
                        sector.Visible = true;
                    }
                    else
                    {
                        sector = new MapTile(sectorX, sectorY)
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
        }
        
        public void ClearCache()
        {
            int minAvg = _tileCount / 2;
            int ySectorMin = -minAvg + MapCenter.YSector;
            int ySectorMax = -minAvg + MapCenter.YSector;
            int xSectorMin = -minAvg + MapCenter.XSector;
            int xSectorMax = -minAvg + MapCenter.XSector;

            List<string> deleteCache = new List<string>();
            foreach (MapTile tile in _mapSectors.Values)
            {
                if (tile.SectorX < xSectorMin || tile.SectorX > xSectorMax
                                              || tile.SectorY < ySectorMin || tile.SectorY > ySectorMax)
                {
                    deleteCache.Add(tile.Name);
                }
            }

            if (deleteCache.Count > 0)
            {
                for (int i = 0; i < deleteCache.Count; i++)
                {
                    Controls.RemoveByKey(deleteCache[i]);
                    _mapSectors.Remove(deleteCache[i]);
                }
            }
        }
        private void ClearTiles()
        {
            int minAvg = _tileCount / 2;

            int ySectorMin = -minAvg + MapCenter.YSector;
            int ySectorMax = -minAvg + MapCenter.YSector;
            int xSectorMin = -minAvg + MapCenter.XSector;
            int xSectorMax = -minAvg + MapCenter.XSector;

            foreach (MapTile tile in _mapSectors.Values)
            {
                if (tile.SectorX < xSectorMin || tile.SectorX > xSectorMax
                                              || tile.SectorY < ySectorMin || tile.SectorY > ySectorMax)
                {
                    tile.Visible = false;
                }
            }
        }

        private void RemoveTiles()
        {
            foreach (MapTile tile in _mapSectors.Values)
            {
                Controls.RemoveByKey(tile.Name);
            }

            _mapSectors.Clear();
        }

        public void SetView(WorldPoint viewPoint)
        {
            if (!_mapCenter.Equals(viewPoint))
            {
                if (_mapCenter.Region != viewPoint.Region && viewPoint.InCave())
                {
                    SelectMapLayer(viewPoint.Region);
                    RemoveTiles();
                    _mapCenter = viewPoint;
                }
                else
                {
                    _mapCenter = viewPoint;
                    ClearTiles();
                }

                UpdateTiles();
            }

            UpdateMarkerLocations();
        }
        public Point GetPoint(WorldPoint coords)
        {
            int tileAvg = _tileCount / 2;

            return new Point
            {
                X = (int) Math.Round((coords.X - _mapCenter.X) / (192.0 / _tileSize.Width) +
                    _tileSize.Width * tileAvg - _tileSize.Width / 2.0),
                Y = (int) Math.Round((coords.Y - _mapCenter.Y) / (192.0 / _tileSize.Height) * (-1) +
                    _tileSize.Height * tileAvg - _tileSize.Height / 2.0)
            };
        }

        public LocalPoint GetCoord(Point point)
        {
            int tileAvg = _tileCount / 2;
            float coordX = (point.X + _tileSize.Width / 2.0f - _tileSize.Width * tileAvg) * 192 / _tileSize.Width +
                           _mapCenter.X;
            float coordY =
                (point.Y + _tileSize.Height / 2.0f - _tileSize.Height * tileAvg) * 192 / _tileSize.Height * (-1) +
                _mapCenter.Y;

            return MapCenter.InCave()
                ? new LocalPoint(MapCenter.Region, coordX, MapCenter.Z, coordY)
                : LocalPoint.FromWorld(new WorldPoint(coordX, coordY));
        }
        public void AddMarker(uint uniqueId, MapControl marker)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() =>
                {
                    marker.Name = Name + "_" + uniqueId;
                    Controls.Add(marker);
                    Controls.SetChildIndex(marker, 1);
                    _markers[uniqueId] = marker;
                }));
            }
            else
            {
                marker.Name = Name + "_" + uniqueId;
                Controls.Add(marker);
                Controls.SetChildIndex(marker, 1);
                _markers[uniqueId] = marker;
            }
        }

        public void RemoveMarker(uint uniqueId)
        {
            MapControl marker = _markers[uniqueId];
            if (marker != null)
            {
                Controls.RemoveByKey(marker.Name);
                _markers.Remove(uniqueId);
            }
        }

        public void ClearMarkers()
        {
            for (int i = 0; i < _markers.Count; i++)
            {
                Controls.RemoveByKey(_markers.Values.ToList()[i].Name);
            }

            _markers.Clear();
        }

        public void UpdateMarkerLocations()
        {
            double aX = _tileSize.Width * (_zoom + (3 / 2)) - _tileSize.Width / 2.0;
            double aY = _tileSize.Height * (_zoom + (3 / 2)) - _tileSize.Height / 2.0;
            double bX = (192.0 / _tileSize.Width);
            double bY = (192.0 / _tileSize.Height);

            foreach (KeyValuePair<uint,MapControl> keyValuePair in _markers)
            {
                var uid = keyValuePair.Key;
                var marker = keyValuePair.Value;

                WorldPoint coord = ((Entity) marker.Tag).WorldPoint;
                Point location = new Point((int) Math.Round((coord.X - _mapCenter.X) / bX + aX),
                    (int) Math.Round((coord.Y - _mapCenter.Y) / bY * (-1) + aY));
                
                location.X -= marker.Image.Size.Width / 2;
                location.Y -= marker.Image.Size.Height / 2;
                
                if (marker.Location.X != location.X && marker.Location.Y != location.Y)
                {
                    marker.Location = location;
                }
            }
        }

        private void MapClicked(object sender, MouseEventArgs e)
        {
            MapTile t = (MapTile) sender;
            var clickPoint = new Point(t.Location.X + e.Location.X, t.Location.Y + e.Location.Y);
            Console.WriteLine("clicked on: " + clickPoint);
            var coord = GetCoord(new Point(t.Location.X + e.Location.X, t.Location.Y + e.Location.Y));
            Console.WriteLine("Going to move to: " + coord);
            InteractionQueue.PacketQueue.Enqueue(Movement.WalkTo(coord));
            Program.Gui.Log("Moving to [" + coord + "]");
        }
    }
}