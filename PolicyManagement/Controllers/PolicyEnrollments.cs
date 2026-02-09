using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PolicyManagement.Entities;
namespace PolicyManagementApp.Controllers
{
    [ApiController]
    [Route("api/policyEnrollment")]
    public class EnrollementController : ControllerBase
    {
        private readonly IPolicyEnrollment policyEnrollmentService;
        public EnrollementController(IPolicyEnrollment _policyEnrollmentService)
        {
            this.policyEnrollmentService = _policyEnrollmentService;
        }
        [HttpPost]
        public async Task<IActionResult> EnrollUserInPolicy([FromBody] UserPolicy enrollment)
        {
            if (enrollment == null)
            {
                return BadRequest(new { message = "Enrollment data is required" });
            }

            var result = await policyEnrollmentService.EnrollUserInPolicyAsync(enrollment.UserId, enrollment.PolicyId);
            if (result)
            {
                return Ok(new { message = "User enrolled in policy successfully" });
            }
            else
            {
                return StatusCode(500, new { message = "Failed to enroll user in policy" });
            }
        }

        [Authorize(Roles = "Admin")]

        [HttpPut("{userId}/{policyId}", Name = "UpdateEnrollment")]
        public async Task<IActionResult> UpdateEnrollment(int userId, int policyId, [FromBody] UserPolicy enrollment)
        {     if (enrollment == null)
            {
                return BadRequest(new { message = "Enrollment data is required" });
            }

            var result = await policyEnrollmentService.UpdateEnrollmentAsync(userId, policyId, enrollment);
            if (result)
            {
                return Ok(new { message = "Enrollment updated successfully" });
            }
            else
            {
                return StatusCode(500, new { message = "Failed to update enrollment" });
            }
        }

        [HttpGet("{id}", Name = "GetEnrollmentById")]
        public async Task<IActionResult> GetEnrollmentById(int userId)
        {
            var enrollment = await policyEnrollmentService.GetEnrollmentByIdAsync(userId);
            if (enrollment == null)
            {
                return NotFound(new { message = $"Enrollment not found" });
            }
            return Ok(enrollment);
        }
    }

}