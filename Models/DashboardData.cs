﻿namespace IM.Models
{
    public class DashboardData
    {
        public KpiData KpiData { get; set; }
        public KpiData OverallData { get; set; }
        public object MostAssignedToUsersData { get; set; }
        public List<Incident> LastFive { get; set; }
        public List<Incident> Oldest5 { get; set; }
    }
}
