namespace FitnessCenter.Models
{
    public class TrainerIndexInfoClass
    {
        public int id { get; set; }
        public string WorkoutName { get; set; }
        public decimal? WorkoutDurashion { get; set; }
        public string WorkoutShift { get; set; }
        public string WorkoutImage { get; set; }
        public List<Member> MembersInClass { get; set; }
    }
}
