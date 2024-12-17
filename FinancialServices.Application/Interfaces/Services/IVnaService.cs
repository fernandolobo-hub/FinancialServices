using PublicBonds.Application.DTOs.Response;
using PublicBonds.Domain.Entities;
using PublicBonds.Domain.Enums;
using PublicBonds.Domain.RequestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicBonds.Application.Interfaces.Services
{
    public interface IVnaService
    {
        Task<List<VnaResponseDto>> GetVnasAsync(VnaFilterRequest request);

        Task<VnaResponseDto> GetMostRecentVnaAsync(DateTime referenceDate, IndexerEnum indexer);
    }
}
