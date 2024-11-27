using PublicBonds.Application.Interfaces.Repositories;
using PublicBonds.Domain.Entities;


namespace PublicBonds.Application.Persistance
{
    public static class BondTypeCaching
    {
        private static List<BondType>? _bondTypes;

        public static List<BondType>? BondTypes
        {
            get { return _bondTypes; }
        }

        public static void Initialize(IBondTypeRepository bondTypeRepository)
        {
            _bondTypes ??= bondTypeRepository.GetAllAsync().Result.ToList();
        }

        public static BondType GetBondTypeByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidBondTypeNameException("Nome do bond não pode ser nulo ou vazio.");
            }

            var bondType = _bondTypes?.FirstOrDefault(x => x.Name == name);

            if (bondType == null)
            {
                throw new InvalidBondTypeNameException($"BondType com o nome '{name}' não encontrado.");
            }

            return bondType;
        }
    }

    public class InvalidBondTypeNameException : Exception
    {
        public InvalidBondTypeNameException(string message) : base(message) { }
    }
}
