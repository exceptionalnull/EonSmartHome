namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    public class SmartHomeScheduleRule
    {
        public int StimeOpt { get; set; }
        public int[] Wday { get; set; }
        public int Smin { get; set; }
        public int Enable { get; set; }
        public int Repeat { get; set; }
        public int EtimeOpt { get; set; }
        public string Name { get; set; }
        public int Eact { get; set; }
        public int Month { get; set; }
        public int Sact { get; set; }
        public int Year { get; set; }
        public int Longitude { get; set; }
        public int Day { get; set; }
        public int Force { get; set; }
        public int Latitude { get; set; }
        public int Emin { get; set; }
    }
}