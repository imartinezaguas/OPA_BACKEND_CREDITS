using System.Threading.Tasks;
using Opa.Credits.Domain.Entities;

namespace Opa.Credits.Application.Interfaces;

public interface IAssociateRepository
{
    Task<Associate> GetByIdentificationAsync(string identification);
    Task AddAsync(Associate associate);
}
