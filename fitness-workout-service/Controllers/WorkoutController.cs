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
        public IActionResult CreateWorkout([FromBody] ReqWorkoutDto workoutReq)
        {
            try
            {
                if (workoutReq == null)
                    return BadRequest(new
                    {
                        status = "Failed",
                        message = "Requset not valid"
                    });

                var isWorkoutExist = _workoutRep.GetWorkouts()
                    .Where(u => u.WorkoutName.Trim().ToLower() == workoutReq.WorkoutName.Trim().ToLower())
                    .FirstOrDefault();

                if (isWorkoutExist != null)
                {
                    return Ok(new
                    {
                        status = "Success",
                        message = "Workout already exists",
                        data = isWorkoutExist
                    });
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
                    return StatusCode(500, new
                    {
                        status = "Failed",
                        message = "Something went wrong adding workout"
                    });
                }

                return Ok(new
                {
                    status = "Success",
                    message = "Workout Successfully created",
                    data = workout
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    status = "Failed",
                    message = e.Message,
                    InnerException = e.InnerException.Message
                });
            }
        }

        [HttpPut("{workoutId}")]
        public IActionResult UpdateWorkout(int workoutId, [FromBody] ReqWorkoutDto workoutReq)
        {
            try
            {
                if (workoutReq == null)
                    return BadRequest(new
                    {
                        status = "Failed",
                        message = "Requset not valid"
                    });

                var isWorkoutExist = _workoutRep.GetWorkouts()
                        .Where(u => u.WorkoutID == workoutId)
                        .FirstOrDefault();

                if (isWorkoutExist == null)
                    return Ok(new
                    {
                        status = "Success",
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
                    return StatusCode(500, new
                    {
                        status = "Failed",
                        message = "Something went wrong when updating workout"
                    });
                }

                return Ok(new
                {
                    status = "Success",
                    message = "Workout Successfully updated",
                    data = updatedWorkout
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    status = "Failed",
                    message = e.Message,
                    InnerException = e.InnerException.Message
                });
            }
        }

        [HttpDelete("{workoutId}")]
        public IActionResult DeleteWorkout(int workoutId)
        {
            try
            {
                var isWorkoutExist = _workoutRep.GetWorkouts()
                    .Where(u => u.WorkoutID == workoutId)
                    .FirstOrDefault();

                if (isWorkoutExist == null)
                    return Ok(new
                    {
                        status = "Success",
                        message = "Workout not found!"
                    });

                if (!_workoutRep.DeleteWorkout(isWorkoutExist))
                {
                    return StatusCode(500, new
                    {
                        status = "Failed",
                        message = "Something went wrong deleting workout!"
                    });
                }

                return Ok(new
                {
                    status = "Success",
                    message = "Workout Successfully Deleted"
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    status = "Failed",
                    message = e.Message,
                    InnerException = e.InnerException.Message
                });
            }
        }

        [HttpGet]
        public IActionResult GetWorkouts()
        {
            try
            {
                var allWorkouts = _workoutRep.GetWorkouts();

                if (allWorkouts.Count <= 0)
                {
                    return Ok(new
                    {
                        status = "Success",
                        message = "Workout is empty!",
                        dat = allWorkouts
                    });
                }

                return Ok(new
                {
                    status = "Success",
                    message = "All Workout Successfully fetched",
                    data = allWorkouts
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    status = "Failed",
                    message = e.Message,
                    InnerException = e.InnerException.Message
                });
            }
        }

        [HttpGet("{workoutId}")]
        public IActionResult GetWorkout(int workoutId)
        {
            try
            {
                var workout = _workoutRep.GetWorkout(workoutId);

                if (workout == null)
                {
                    return Ok(new
                    {
                        status = "Succes",
                        message = "Workout not found!",
                        data = workout
                    });
                }

                return Ok(new
                {
                    status = "Success",
                    message = "Workout Successfully fetched",
                    data = workout
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    status = "Failed",
                    message = e.Message,
                    InnerException = e.InnerException.Message
                });
            }
        }
    }
}
