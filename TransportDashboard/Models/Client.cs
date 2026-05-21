using System;
using System.Collections.Generic;
using System.Text;

namespace TransportDashboard.Models;

public class Client
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public ICollection<FreightHistory> FreightHistories { get; set; }
        = new List<FreightHistory>();
}