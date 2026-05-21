using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace TransportDashboard.Forms
{
    public partial class MainForm : Form
    {
        private Panel top;
        private Label status;
        private SplitContainer split;

        private DataGridView dgvLoads;
        private DataGridView dgvVehicles;
        private DataGridView dgvMatch;

        private PictureBox map;
        private Bitmap mapImg;     

        private System.Windows.Forms.Timer timer;

        private List<Vehicle> vehicles = new();
        private List<TransLoads> loads = new();
        private List<MatchResult> matchResults = new();

        private bool aiRunning = false;
        private DateTime lastSound = DateTime.MinValue;
        [DllImport("shell32.dll")]
        private static extern int SetCurrentProcessExplicitAppUserModelID(string AppID);

        private readonly Dictionary<string, Bitmap> truckIcons = new();

        public MainForm()
        {
            BuildUI();
            InitData();
            InitMap();
            LoadTruckIcons();
            InitTimer();
            RunAI();
            SetCurrentProcessExplicitAppUserModelID("TransportDashboard.Dispatcher");
        }

        // ================= UI =================

        private void BuildUI()
        {
            split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 800
            };

            Controls.Add(split);

            // LEFT = MAP
            map = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            split.Panel1.Controls.Add(map);

            // RIGHT = TABS
            var tabs = new TabControl
            {
                Dock = DockStyle.Fill
            };

            split.Panel2.Controls.Clear();
            split.Panel2.Controls.Add(tabs);

            // GRIDY
            dgvLoads = CreateGrid();
            dgvVehicles = CreateGrid();
            dgvMatch = CreateGrid();

            tabs.TabPages.Add(CreateTab("Loads", dgvLoads));
            tabs.TabPages.Add(CreateTab("Vehicles", dgvVehicles));
            tabs.TabPages.Add(CreateTab("AI Match", dgvMatch));
        }
        private TabPage CreateTab(string name, DataGridView grid)
        {
            var tab = new TabPage(name);

            grid.Dock = DockStyle.Fill;
            tab.Controls.Add(grid);

            return tab;
        }

        private DataGridView CreateGrid()
        {
            return new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false
            };
        }

        private TabPage Tab(string name, Control c)
        {
            var t = new TabPage(name);
            t.Controls.Add(c);
            return t;
        }

        // ================= INIT DATA =================
        private void LoadTruckIcons()
        {
            foreach (var v in vehicles)
            {
                string path = Path.Combine(
                    Application.StartupPath,
                    "icons",
                    $"truck_{v.Id.ToLower()}.png"
                );

                if (File.Exists(path))
                {
                    try
                    {
                        truckIcons[v.Id] = new Bitmap(path);
                    }
                    catch
                    {
                        // fallback – ignorujemy błędny plik
                    }
                }
            }
        }
        private void InitData()
        {
            vehicles = new List<Vehicle>
            {
                new Vehicle("T1", 100, 120),
                new Vehicle("T2", 300, 200),
                new Vehicle("T3", 500, 250)
            };

            loads = new List<TransLoads>
            {
                new TransLoads { Id = 1, Weight = 1200, Price = 5000 },
                new TransLoads { Id = 2, Weight = 800, Price = 3000 }
            };

            RefreshGrids();
        }

        private void InitMap()
        {
            string path = System.IO.Path.Combine(Application.StartupPath, "assets", "offline_map.png");

            if (System.IO.File.Exists(path))
                mapImg = new Bitmap(path);
        }

        private void InitTimer()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += (s, e) => Tick();
            timer.Start();
        }

        // ================= MAIN LOOP =================

        private void Tick()
        {
            MoveVehicles();
            RunAI();
            DrawMap();
            RefreshVehicleGrid();
        }

        // ================= VEHICLES =================

        private void MoveVehicles()
        {
            foreach (var v in vehicles)
            {
                v.X += v.Dx;
                v.Y += v.Dy;

                if (v.X < 0 || v.X > map.Width) v.Dx *= -1;
                if (v.Y < 0 || v.Y > map.Height) v.Dy *= -1;
            }
        }

        // ================= AI =================

        private void RunAI()
        {
            if (aiRunning) return;
            aiRunning = true;

            matchResults.Clear();

            foreach (var load in loads)
            {
                var best = vehicles
                    .OrderByDescending(v => Score(load, v))
                    .FirstOrDefault();

                if (best == null) continue;

                double score = Score(load, best);

                matchResults.Add(new MatchResult
                {
                    LoadId = load.Id,
                    TruckId = best.Id,
                    Score = score
                });

                // 🔥 THROTTLE SOUND (fix loop)
                if (score > 80 && (DateTime.Now - lastSound).TotalSeconds > 15)
                {
                    SystemSounds.Exclamation.Play();
                    lastSound = DateTime.Now;
                }
            }

            RefreshMatchGrid();

            aiRunning = false;
        }

        private double Score(TransLoads l, Vehicle v)
        {
            double cost = l.Weight * 1.4;
            double margin = (l.Price - cost) / cost * 100;
            double dist = Math.Abs(v.X - 200) + Math.Abs(v.Y - 200);

            return margin * 1.5 - dist * 0.1 + 50;
        }

        // ================= GRID =================

        private void RefreshGrids()
        {
            dgvLoads.DataSource = loads;
            RefreshVehicleGrid();
            RefreshMatchGrid();
        }

        private void RefreshVehicleGrid()
        {
            dgvVehicles.DataSource = vehicles.Select(v => new
            {
                v.Id,
                v.X,
                v.Y
            }).ToList();
        }

        private void RefreshMatchGrid()
        {
            dgvMatch.DataSource = null;
            dgvMatch.DataSource = matchResults;

            if (dgvMatch.DataSource == null || dgvMatch.Rows.Count == 0)
                return;

             

            foreach (DataGridViewRow row in dgvMatch.Rows.Cast<DataGridViewRow>().Where(r => !r.IsNewRow))
            {
                if (row.Cells["Score"].Value == null) continue;

                double score = Convert.ToDouble(row.Cells["Score"].Value);

                if (score > 80)
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                else if (score > 50)
                    row.DefaultCellStyle.BackColor = Color.LightYellow;
                else
                    row.DefaultCellStyle.BackColor = Color.LightCoral;
            }
        }

        // ================= MAP =================

        private void DrawMap()
        {
            if (map.Width <= 0) return;

            var bmp = new Bitmap(map.Width, map.Height);
            using var g = Graphics.FromImage(bmp);

            g.DrawImage(mapImg, 0, 0, map.Width, map.Height);

            foreach (var v in vehicles)
            {
                // ===== ICON FIRST =====
                if (truckIcons.TryGetValue(v.Id, out var icon))
                {
                    g.DrawImage(
                        icon,
                        (float)(v.X - icon.Width / 2),
                        (float)(v.Y - icon.Height / 2),
                        icon.Width,
                        icon.Height
                    );
                }
                else
                {
                    // fallback
                    g.FillEllipse(Brushes.Red, (float)v.X, (float)v.Y, 12, 12);
                }

                // label
                g.DrawString(
                    v.Id,
                    Font,
                    Brushes.Black,
                    (float)v.X + 5,
                    (float)v.Y + 5
                );
            }

            map.Image?.Dispose();
            map.Image = bmp;
        }

        // ================= MODELS =================

        class TransLoads
        {
            public int Id { get; set; }
            public double Weight { get; set; }
            public double Price { get; set; }
        }

        class Vehicle
        {
            public string Id;
            public float X, Y;
            public float Dx = 2;
            public float Dy = 1;

            public Vehicle(string id, float x, float y)
            {
                Id = id;
                X = x;
                Y = y;
            }
        }

        class MatchResult
        {
            public int LoadId { get; set; }
            public string TruckId { get; set; }
            public double Score { get; set; }
        }
    }
}