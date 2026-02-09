using PolicyManagement.Entities; 
using System.Collections.Generic;
using System.Linq;
using PolicyManagementApp.Data;  
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using PolicyManagement.DTOs;

namespace PolicyManagement.Repositories
{
    public class PolicyRepository : IPolicyRepository
    {
        private readonly AppDbContext _dbContext;
        
        public PolicyRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ReturnPolicyDto>> GetAllPoliciesAsync()
        {
            return await _dbContext.Policies
                .Select(p => new ReturnPolicyDto
                {
                    Name = p.PolicyName,
                    PremiumAmount = p.PremiumAmount,
                    Description = p.Description ?? string.Empty,
                    IsActive = p.IsActive
                })
                .ToListAsync();
        }

        public async Task<ReturnPolicyDto> GetPolicyByIdAsync(int id)
        {
            var policy = await _dbContext.Policies.FindAsync(id);
            if (policy == null)
            {
                throw new KeyNotFoundException($"Policy with ID {id} not found.");
            }

            return new ReturnPolicyDto
            {
                Name = policy.PolicyName,
                PremiumAmount = policy.PremiumAmount,
                Description = policy.Description ?? string.Empty,
                IsActive = policy.IsActive
            };
        }

        public async Task<IEnumerable<ReturnPolicyDto>> SearchPoliciesByAmountAsync(int minAmount, int maxAmount)
        {
            return await _dbContext.Policies
                .Where(p => p.PremiumAmount >= minAmount && p.PremiumAmount <= maxAmount)
                .Select(p => new ReturnPolicyDto
                {
                    Name = p.PolicyName,
                    PremiumAmount = p.PremiumAmount,
                    Description = p.Description ?? string.Empty,
                    IsActive = p.IsActive
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ReturnPolicyDto>> GetPoliciesByStatusAsync(bool isActive)
        {
            return await _dbContext.Policies
                .Where(p => p.IsActive == isActive)
                .Select(p => new ReturnPolicyDto
                {
                    Name = p.PolicyName,
                    PremiumAmount = p.PremiumAmount,
                    Description = p.Description ?? string.Empty,
                    IsActive = p.IsActive
                })
                .ToListAsync();
        }

        public async Task<ReturnPolicyDto> CreatePolicyAsync(CreatePolicyDto policyDto)
        {
            var policy = new Policy
            {
                PolicyName = policyDto.Name,
                PremiumAmount = policyDto.PremiumAmount,
                Description = policyDto.Description,
                IsActive = policyDto.IsActive
            };

            _dbContext.Policies.Add(policy);
            await _dbContext.SaveChangesAsync();

            return new ReturnPolicyDto
            {
                Name = policy.PolicyName,
                PremiumAmount = policy.PremiumAmount,
                Description = policy.Description ?? string.Empty,
                IsActive = policy.IsActive
            };
        }

        public async Task<Policy?> UpdatePolicyAsync(int id, Policy policy)
        {
            var existingPolicy = await _dbContext.Policies.FindAsync(id);
            if (existingPolicy == null)
            {
                return null;
            }

            existingPolicy.PolicyName = policy.PolicyName;
            existingPolicy.PremiumAmount = policy.PremiumAmount;
            existingPolicy.Description = policy.Description;
            existingPolicy.IsActive = policy.IsActive;

            _dbContext.Policies.Update(existingPolicy);
            await _dbContext.SaveChangesAsync();
            return existingPolicy;
        }   

        public async Task<bool> DeletePolicyAsync(int id)
        {
            var existingPolicy = await _dbContext.Policies.FindAsync(id);
            if (existingPolicy == null)
            {
                return false;
            }

            _dbContext.Policies.Remove(existingPolicy);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}