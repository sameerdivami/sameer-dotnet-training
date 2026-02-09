using PolicyManagement.DTOs;
using PolicyManagement.Entities;
namespace PolicyManagement.Services
{
    public class PolicyEnrollment : IPolicyEnrollment
    {
        private readonly IPolicyEnrollmentRepo iPolicyEnrollmentRepo;

        public PolicyEnrollment(IPolicyEnrollmentRepo iPolicyEnrollmentRepo)
        {
            this.iPolicyEnrollmentRepo = iPolicyEnrollmentRepo;
        }

        public async Task<bool> EnrollUserInPolicyAsync(int userId, int policyId)
        {
            return await iPolicyEnrollmentRepo.EnrollUserInPolicyAsync(userId, policyId);
        }

        public async Task<bool> UpdateEnrollmentAsync(int userId, int policyId, UserPolicy newStatus)
        {
            return await iPolicyEnrollmentRepo.UpdateEnrollmentAsync(userId, policyId, newStatus);
        }

        public async Task<List<UserPolicyDetailsDto>> GetEnrollmentByIdAsync(int userId)
        {
            return await iPolicyEnrollmentRepo.GetEnrollmentByIdAsync(userId);
        }

    }
}