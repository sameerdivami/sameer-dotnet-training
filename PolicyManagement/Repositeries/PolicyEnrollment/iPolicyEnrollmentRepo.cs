using PolicyManagement.Entities;
using PolicyManagement.DTOs;
public interface IPolicyEnrollmentRepo
{
    Task<bool> EnrollUserInPolicyAsync(int userId, int policyId);

    Task<bool> UpdateEnrollmentAsync(int userId, int policyId, UserPolicy newStatus);

    Task<List<UserPolicyDetailsDto>> GetEnrollmentByIdAsync(int userId);
}