using FluentValidation;
using PublicBonds.Application.DTOs.Response;
using PublicBonds.Application.Interfaces.Repositories;
using PublicBonds.Application.Interfaces.Services;
using PublicBonds.Domain.Entities;
using PublicBonds.Domain.Enums;
using PublicBonds.Domain.Exceptions.Request;
using PublicBonds.Domain.RequestObjects;
using System;


namespace PublicBonds.Application.Services
{
    public class VnaService : IVnaService
    {

        private readonly IVnaRepositoy _vnaRepository;
        private readonly IValidator<VnaFilterRequest> _vnaRequestValidator;

        public VnaService(IVnaRepositoy vnaRepository,
            IValidator<VnaFilterRequest> vnaRequestValidator)
        {
            _vnaRepository = vnaRepository;
            _vnaRequestValidator = vnaRequestValidator;
        }

        public async Task<VnaResponseDto> GetMostRecentVnaAsync(DateTime referenceDate, IndexerEnum indexer)
        {
            var mostRecentVna = await _vnaRepository.GetMostRecentVnaAsync(referenceDate, indexer);

            return VnaResponseDto.FromVna(mostRecentVna);
        }

        public async Task<List<VnaResponseDto>> GetVnasAsync(VnaFilterRequest request)
        {
            try
            {
                var validationResult = _vnaRequestValidator.Validate(request);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors
                        .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                        .ToList();

                    throw new VnaRequestValidationException("Request Validation Error", errors);
                }

                List<VnaResponseDto> vnaResponseDtos = new List<VnaResponseDto>();

                var indexerEnum = (IndexerEnum)Enum.Parse(typeof(IndexerEnum), request.Indexer, ignoreCase: true);
                var vnas = await _vnaRepository.GetVnasAsync(request.StartDate, request.EndDate, indexerEnum);
                foreach (var vna in vnas)
                {
                    var dto = VnaResponseDto.FromVna(vna);
                    vnaResponseDtos.Add(dto);
                }
                return vnaResponseDtos;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving vna's: " + ex.Message);
                throw;
            }
        }
    }
}
