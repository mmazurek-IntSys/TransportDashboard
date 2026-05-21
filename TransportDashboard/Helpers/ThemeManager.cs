namespace TransportDashboard.Helpers;

public static class ThemeManager
{
    public static void ApplyDarkTheme(Control parent)
    {
        parent.BackColor = Color.FromArgb(32, 32, 32);

        parent.ForeColor = Color.White;

        foreach (Control control in parent.Controls)
        {
            switch (control)
            {
                case Button btn:
                    btn.BackColor = Color.FromArgb(45, 45, 48);
                    btn.ForeColor = Color.White;
                    btn.FlatStyle = FlatStyle.Flat;
                    break;

                case DataGridView dgv:
                    ApplyDarkGrid(dgv);
                    break;

                case Panel p:
                    p.BackColor = Color.FromArgb(37, 37, 38);
                    break;
            }

            ApplyDarkTheme(control);
        }
    }

    private static void ApplyDarkGrid(
        DataGridView dgv)
    {
        dgv.BackgroundColor =
            Color.FromArgb(30, 30, 30);

        dgv.GridColor =
            Color.FromArgb(60, 60, 60);

        dgv.DefaultCellStyle.BackColor =
            Color.FromArgb(45, 45, 48);

        dgv.DefaultCellStyle.ForeColor =
            Color.White;

        dgv.ColumnHeadersDefaultCellStyle.BackColor =
            Color.FromArgb(28, 28, 28);

        dgv.ColumnHeadersDefaultCellStyle.ForeColor =
            Color.White;

        dgv.EnableHeadersVisualStyles = false;
    }
}