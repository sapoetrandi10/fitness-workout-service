using fitness_db.Data;
using fitness_db.Interfaces;
using fitness_db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fitness_db.Repositories
{
    public class WorkoutRepository : IWorkoutRepository
    {
        private FitnessContext _fitnessCtx;
        public WorkoutRepository(FitnessContext context)
        {
            _fitnessCtx = context;
        }

        public bool CreateWorkout(Workout Workout)
        {
            _fitnessCtx.workouts.Add(Workout);
            var saved = _fitnessCtx.SaveChanges();

            return saved > 0 ? true : false;
        }

        public Workout UpdateWorkout(Workout Workout)
        {
            var updated = _fitnessCtx.SaveChanges();

            if (updated < 0)
            {
                return Workout = null;
            }

            return Workout;
        }


        public bool DeleteWorkout(Workout Workout)
        {
            _fitnessCtx.Remove(Workout);
            var saved = _fitnessCtx.SaveChanges();

            return saved > 0 ? true : false;
        }

        public ICollection<Workout> GetWorkouts()
        {
            return _fitnessCtx.workouts.ToList();
        }

        public Workout GetWorkout(int workoutId)
        {
            var workout = _fitnessCtx.workouts.Find(workoutId);
            if (workout == null)
            {
                return null;
            }
            return workout;
        }
    }
}
