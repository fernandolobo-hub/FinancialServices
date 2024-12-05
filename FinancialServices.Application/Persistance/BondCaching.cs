using PublicBonds.Application.Interfaces.Repositories;
using PublicBonds.Domain.Entities;
using PublicBonds.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicBonds.Application.Persistance
{
    public static class BondCaching
    {
        private static List<Bond>? _bonds;

        public static List<Bond>? Bonds
        {
            get { return _bonds; }
        }

        public static void Initialize(IBondRepository bondRepository)
        {
            _bonds ??= bondRepository.GetAllAsync().Result.ToList();
        }

        public static Bond GetBondByNameAndMaturityDate(string name, DateTime maturityDate)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidBondTypeNameException("Nome do bond não pode ser nulo ou vazio.");
            }

            var bond = _bonds?.FirstOrDefault(x => x.Name == name && x.MaturityDate == maturityDate);

            if (bond == null)
            {
                throw new InvalidBondTypeNameException($"BondType com o nome '{name}' não encontrado.");
            }

            return bond;
        }
    }
}
