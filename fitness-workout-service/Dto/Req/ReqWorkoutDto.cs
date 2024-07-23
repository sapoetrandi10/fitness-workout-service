namespace fitness_workout_service.Dto.Req
{
    public class ReqWorkoutDto
    {
        public int WorkoutID { get; set; }
        public string WorkoutName { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; } = 0;
        public float CaloriesBurned { get; set; } = 0;
    }
}
