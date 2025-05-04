namespace FitnessCenter.Models
{
    public class MemberWorkoutClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string WorkoutName { get; set; }
        public decimal? WorkoutPrice { get; set; }
        public DateTime? JoinDate { get;set; }
        
    }
}
