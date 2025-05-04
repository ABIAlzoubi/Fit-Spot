namespace FitnessCenter.Models
{
    public class AdminDashDataClass
    {
        public decimal WorkoutId { get; set; }
        public string WorkoutName { get; set; }
        public decimal Duration { get; set; }
        public string Shift { get; set; }
        public int MembersCount { get; set; }
        public List<Member> RecentJoin { get; set; }
    }
}
