using fitness_db.Interfaces;
using fitness_db.Models;
using fitness_workout_service.Dto.Req;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fitness_workout_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutController : Controller
    {
        private readonly IWorkoutRepository _workoutRep;
        public WorkoutController(IWorkoutRepository workoutRepository)
        {
            _workoutRep = workoutRepository;
        }

        [HttpPost]
        public IActionResult CreateWorkout([FromBody]  ReqWorkoutDto workoutReq)
        {
            try
            {
                if (workoutReq == null)
                    return BadRequest(new
                    {
                        status = "failed",
                        message = "Requset not valid"
                    });

                var isWorkoutExist = _workoutRep.GetWorkouts()
                    .Where(u => u.WorkoutName.Trim().ToLower() == workoutReq.WorkoutName.Trim().ToLower())
                    .FirstOrDefault();

                if (isWorkoutExist != null)
                {
                    ModelState.AddModelError("", "Workout already exists");
                    if (!ModelState.IsValid)
                    {
                        var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                        return BadRequest(new
                        {
                            status = "failed",
                            errors = errors
                        });
                    }
                }

                var workout = new Workout
                {
                    WorkoutName = workoutReq.WorkoutName,
                    Description = workoutReq.Description,
                    Duration = workoutReq.Duration,
                    CaloriesBurned = workoutReq.CaloriesBurned,
                };

                if (!_workoutRep.CreateWorkout(workout))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }

                return Ok( new { 
                    status = "success",
                    message = "Workout Successfully created"
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    status = "failed",
                    message = e.Message
                });
                throw;
            }
        }

        [HttpPut("{workoutId}")]
        public IActionResult UpdateWorkout(int workoutId, [FromBody] ReqWorkoutDto workoutReq)
        {
            if (workoutReq == null)
                return BadRequest(new
                {
                    status = "failed",
                    message = "Requset not valid"
                });

            var isWorkoutExist = _workoutRep.GetWorkouts()
                    .Where(u => u.WorkoutID == workoutId)
                    .FirstOrDefault();

            if (isWorkoutExist == null)
                return NotFound(new
                {
                    status = "failed",
                    message = "Workout not found!"
                });


            isWorkoutExist.WorkoutID = workoutId;
            isWorkoutExist.WorkoutName = workoutReq.WorkoutName;
            isWorkoutExist.Description = workoutReq.Description;
            isWorkoutExist.Duration = workoutReq.Duration;
            isWorkoutExist.CaloriesBurned = workoutReq.CaloriesBurned;

            var updatedWorkout = _workoutRep.UpdateWorkout(isWorkoutExist);
            if (updatedWorkout == null)
            {
                ModelState.AddModelError("", "Something went wrong updating workout");
                return StatusCode(500, new
                {
                    status = "failed",
                    message = "Something went wrong updating workout"
                });
            }

            return Ok(new
            {
                status = "success",
                message = "Workout Successfully updated",
                data = updatedWorkout
            });
        }

        [HttpDelete("{workoutId}")]
        public IActionResult DeleteWorkout(int workoutId)
        {
            var isWorkoutExist = _workoutRep.GetWorkouts()
                    .Where(u => u.WorkoutID == workoutId)
                    .FirstOrDefault();

            if (isWorkoutExist == null)
                return NotFound(new
                {
                    status = "failed",
                    message = "Workout not found!"
                });

            if (!_workoutRep.DeleteWorkout(isWorkoutExist))
            {
                return BadRequest(new
                {
                    status = "failed",
                    message = "Something went wrong deleting workout!"
                });
            }

            return Ok(new
            {
                status = "success",
                message = "Workout Successfully Deleted"
            });
        }

        [HttpGet]
        public IActionResult GetWorkouts()
        {
            var allWorkouts = _workoutRep.GetWorkouts();

            if (allWorkouts.Count <= 0)
            {
                return NotFound(new
                {
                    status = "failed",
                    message = "Workout is empty!"
                });
            }

            return Ok(new
            {
                status = "success",
                message = "All Workout Successfully fetched",
                data = allWorkouts
            });
        }

        [HttpGet("{workoutId}")]
        public IActionResult GetWorkout(int workoutId)
        {
            var workout = _workoutRep.GetWorkout(workoutId);

            if (workout == null)
            {
                return NotFound(new
                {
                    status = "failed",
                    message = "Workout not found!"
                });
            }

            return Ok(new
            {
                status = "success",
                message = "Workout Successfully fetched",
                data = workout
            });
        }
    }
}
