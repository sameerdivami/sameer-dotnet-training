using PolicyManagement.DTOs;
using PolicyManagement.Entities;
using PolicyManagement.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PolicyManagement.Services
{
    public class PolicyService : IPolicyService 
    {
        private readonly IPolicyRepository _policyRepository;
        
        public PolicyService(IPolicyRepository policyRepository) 
        {
            _policyRepository = policyRepository;
        }

        public async Task<IEnumerable<ReturnPolicyDto>> GetAllPoliciesAsync() 
        {
            return await _policyRepository.GetAllPoliciesAsync();
        }

        public async Task<ReturnPolicyDto?> GetPolicyByIdAsync(int id) 
        {
            return await _policyRepository.GetPolicyByIdAsync(id);
        }

        public async Task<IEnumerable<ReturnPolicyDto>> SearchPoliciesByAmountAsync(int minAmount, int maxAmount) 
        {
            return await _policyRepository.SearchPoliciesByAmountAsync(minAmount, maxAmount);
        }

        public async Task<IEnumerable<ReturnPolicyDto>> GetPoliciesByStatusAsync(bool isActive) 
        {
            return await _policyRepository.GetPoliciesByStatusAsync(isActive);
        }

        public async Task<ReturnPolicyDto> CreatePolicyAsync(CreatePolicyDto policy)
        {
            return await _policyRepository.CreatePolicyAsync(policy);
        }

        public async Task<Policy?> UpdatePolicyAsync(int id, Policy policy)
        {
            return await _policyRepository.UpdatePolicyAsync(id, policy);
        }

        public async Task<bool> DeletePolicyAsync(int id)
        {
            var existingPolicy = await _policyRepository.GetPolicyByIdAsync(id);
            if (existingPolicy == null)
            {
                return false;
            }
            return await _policyRepository.DeletePolicyAsync(id);
        }
    }
}