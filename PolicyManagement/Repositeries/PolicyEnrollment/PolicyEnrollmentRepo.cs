using PolicyManagementApp.Data;
using PolicyManagement.Entities;
using Microsoft.EntityFrameworkCore;
using PolicyManagement.DTOs;
public class PolicyEnrollmentRepo : IPolicyEnrollmentRepo
{
    private readonly AppDbContext _context;

    public PolicyEnrollmentRepo(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> EnrollUserInPolicyAsync(int userId, int policyId)
    {
        var user = await _context.Users.FindAsync(userId);
        var policy = await _context.Policies.FindAsync(policyId);

        if (user == null || policy == null)
        {
            return false; 
        }

        var enrollment = new UserPolicy
        {
            UserId = userId,
            PolicyId = policyId,
            RequestedAt = DateTime.UtcNow
        };

        _context.UserPolicies.Add(enrollment);
        await _context.SaveChangesAsync();

        return true; 
    }

    public async Task<bool> UpdateEnrollmentAsync(int userId, int policyId, UserPolicy newStatus)
    {
        var enrollment = await _context.UserPolicies
            .FirstOrDefaultAsync(up => up.UserId == userId && up.PolicyId == policyId);

        if (enrollment == null)
        {
            return false; 
        }

        enrollment.Status = newStatus.Status;
        enrollment.ApprovedAt = DateTime.UtcNow;

        _context.UserPolicies.Update(enrollment);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<UserPolicyDetailsDto>> GetEnrollmentByIdAsync(int userId)
{
    return await _context.UserPolicies
        .Include(up => up.Policy)
        .Where(up => up.UserId == userId)
        .Select(up => new UserPolicyDetailsDto
        {
            PolicyName = up.Policy!.PolicyName,
            PremiumAmount = up.Policy.PremiumAmount,
            Description = up.Policy.Description,
            IsActive = up.Policy.IsActive,
            Status = up.Status
        })
        .ToListAsync();
}
}