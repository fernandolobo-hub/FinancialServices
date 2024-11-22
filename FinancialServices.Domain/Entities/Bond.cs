using FinancialServices.Domain.Entities;

public class Bond : Entity, IEquatable<Bond>
{
    public BondType Type { get; set; }
    public DateTime MaturityDate { get; set; }
    public string Name
    {
        get
        {
            return $"{Type.Name} {MaturityDate.Year}";
        }
    }

    public Bond()
    {
        
    }

    public Bond(BondType type, DateTime maturityDate)
    {
        Type = type;
        MaturityDate = maturityDate;
    }

    public bool Equals(Bond? other)
    {
        if(other is null)
            return false;
        if(this.MaturityDate == other.MaturityDate && this.Type.Id == other.Type.Id)
            return true;
        return false;
    }
}
