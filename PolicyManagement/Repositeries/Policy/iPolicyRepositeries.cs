using PolicyManagement.DTOs;
using PolicyManagement.Entities;  
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PolicyManagement.Repositories
{
    public interface IPolicyRepository
    {
        Task<IEnumerable<ReturnPolicyDto>> GetAllPoliciesAsync();
        Task<ReturnPolicyDto> GetPolicyByIdAsync(int id);
        Task<IEnumerable<ReturnPolicyDto>> SearchPoliciesByAmountAsync(int minAmount, int maxAmount);
        Task<IEnumerable<ReturnPolicyDto>> GetPoliciesByStatusAsync(bool isActive);

        Task<ReturnPolicyDto> CreatePolicyAsync(CreatePolicyDto policy);

        Task<Policy?> UpdatePolicyAsync(int id, Policy policy);

        Task<bool> DeletePolicyAsync(int id);
    }
}