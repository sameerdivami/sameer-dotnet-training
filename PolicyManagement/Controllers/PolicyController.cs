using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PolicyManagement.Services;
using PolicyManagement.Entities;
using policyManagementApp.Filters;
using Microsoft.AspNetCore.Authorization;
using PolicyManagement.DTOs;
namespace policyManagementApp
{
    [ApiController]
    [ServiceFilter(typeof(GlobalResponseFilter))]
    [ServiceFilter(typeof(ResponseTimeFilter))]
    [Route("api/policies")]
    public class PolicyController : ControllerBase
    {

        private readonly IPolicyService policyService;
        public PolicyController(IPolicyService _policyService)
        {
            this.policyService = _policyService;
        }
        [HttpGet(Name = "GetPolicies")]
        public async Task<IActionResult> GetAllPolicies()
        {
            // await Task.Delay(10000, HttpContext.RequestAborted); 
            var policies = await policyService.GetAllPoliciesAsync();
            return Ok(policies);
        }

        [HttpGet("{id}", Name = "GetPolicyById")]
        public async Task<IActionResult> GetPolicyById(int id)
        {
            var policy = await policyService.GetPolicyByIdAsync(id);
            if (policy == null)
            {
                return NotFound(new { message = $"Policy with ID {id} not found" });
            }
            return Ok(policy);
        }

        [HttpGet("search", Name = "SearchPolicies")]
        public async Task<IActionResult> SearchPolicies([FromQuery] int minAmount, [FromQuery] int maxAmount)
        {
            var policies = await policyService.SearchPoliciesByAmountAsync(minAmount, maxAmount);
            return Ok(policies);
        }

        [HttpGet("status", Name = "GetPoliciesByStatus")]
        public async Task<IActionResult> GetPoliciesByStatus([FromQuery] bool isActive)
        {
            var policies = await policyService.GetPoliciesByStatusAsync(isActive);
            return Ok(policies);
        }

        // Test endpoints for middleware exception handling
        [HttpGet("test-null-ref", Name = "TestNullReference")]
        public IActionResult TestNullReference()
        {
            string? nullString = null;
            return Ok(nullString.Length); // Throws NullReferenceException
        }

        [HttpGet("test-arg-null", Name = "TestArgumentNull")]
        public IActionResult TestArgumentNull()
        {
            throw new ArgumentNullException("testParam", "This is a test ArgumentNullException");
        }

        [HttpGet("test-key-not-found", Name = "TestKeyNotFound")]
        public IActionResult TestKeyNotFound()
        {
            throw new KeyNotFoundException("Test key not found exception");
        }

        [HttpGet("test-timeout", Name = "TestTimeout")]
        public IActionResult TestTimeout()
        {
            throw new TimeoutException("Test timeout exception");
        }

        [HttpGet("test-unauthorized", Name = "TestUnauthorized")]
        public IActionResult TestUnauthorized()
        {
            throw new UnauthorizedAccessException("Test unauthorized access exception");
        }

        [HttpGet("test-http-request", Name = "TestHttpRequest")]
        public async Task<IActionResult> TestHttpRequest()
        {
            using var httpClient = new HttpClient();
            // This should fail and throw HttpRequestException
            var response = await httpClient.GetAsync("http://localhost:9999/nonexistent");
            return Ok();
        }

        [HttpGet("test-io", Name = "TestIOException")]
        public IActionResult TestIOException()
        {
            throw new IOException("Test IO exception");
        }

        [HttpGet("test-invalid-op-jwt", Name = "TestInvalidOperationJWT")]
        public IActionResult TestInvalidOperationJWT()
        {
            throw new InvalidOperationException("JWT authentication failed");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(Name = "CreatePolicy")]
        public async Task<IActionResult> CreatePolicy([FromBody] CreatePolicyDto policy)
        {
            if (policy == null)
            {
                return BadRequest(new { message = "Policy data is required" });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(modalField => modalField.Value?.Errors.Count > 0)
                    .Select(modalField => new
                    {
                        Field = modalField.Key,
                        Errors = modalField.Value?.Errors.Select(e => e.ErrorMessage).ToList() ?? new List<string>()
                    })
                    .ToList();

                return BadRequest(new
                {
                    message = "Validation failed",
                    errors = errors
                });
            }

            var createdPolicy = await policyService.CreatePolicyAsync(policy);
            return CreatedAtRoute("GetPolicyById", new { createdPolicy });
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}", Name = "UpdatePolicy")]
        public async Task<IActionResult> UpdatePolicy(int id, [FromBody] Policy policy)
        {
            if (policy == null || id != policy.Id)
            {
                return BadRequest(new { message = "Invalid policy data" });
            }

            var updatedPolicy = await policyService.UpdatePolicyAsync(id, policy);
            if (updatedPolicy == null)
            {
                return NotFound(new { message = $"Policy with ID {id} not found" });
            }

            return Ok(updatedPolicy);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}", Name = "DeletePolicy")]
        public async Task<IActionResult> DeletePolicy(int id)
        {
            var deleted = await policyService.DeletePolicyAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = $"Policy with ID {id} not found" });
            }

            return NoContent();
        }
    }
}