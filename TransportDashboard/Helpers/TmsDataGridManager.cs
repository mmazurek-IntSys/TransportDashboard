using System;
using System.Collections.Generic;
using System.Text;

namespace TransportDashboard.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    public class TmsDataGridManager<T>
    {
        private readonly DataGridView _grid;
        private readonly List<T> _data = new();

        public TmsDataGridManager(DataGridView grid)
        {
            _grid = grid ?? throw new ArgumentNullException(nameof(grid));
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            _grid.SuspendLayout();
            _grid.Dock = DockStyle.Fill;
            _grid.AutoGenerateColumns = true;
            _grid.ReadOnly = true;
            _grid.AllowUserToAddRows = false;
            _grid.AllowUserToDeleteRows = false;
            _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _grid.MultiSelect = false;
            _grid.BackgroundColor = Color.White;
            _grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _grid.EnableHeadersVisualStyles = true;
            _grid.RowHeadersVisible = false;
            _grid.SuspendLayout();
        }

        public void SetData(IEnumerable<T> data)
        {
            _data.Clear();
            if (data != null)
                _data.AddRange(data);

            _grid.SuspendLayout();
            _grid.DataSource = null;
            _grid.DataSource = _data;
            _grid.ResumeLayout();
        }

        public void RefreshAiScore(string columnName = "AiScore", double low = 0, double high = 100)
        {
            if (!_grid.Columns.Contains(columnName)) return;

            foreach (DataGridViewRow row in _grid.Rows)
            {
                if (row.Cells[columnName].Value != null && double.TryParse(row.Cells[columnName].Value.ToString(), out double score))
                {
                    row.Cells[columnName].Style.BackColor = GetHeatColor(score, low, high);
                }
            }
        }

        private Color GetHeatColor(double value, double min, double max)
        {
            // green -> yellow -> red
            double ratio = (value - min) / (max - min);
            ratio = Math.Min(Math.Max(ratio, 0), 1);

            int r = (int)(255 * ratio);
            int g = (int)(255 * (1 - ratio));
            return Color.FromArgb(r, g, 0);
        }
    }
}
