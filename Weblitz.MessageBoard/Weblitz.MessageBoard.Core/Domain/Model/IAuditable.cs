namespace Weblitz.MessageBoard.Core.Domain.Model
{
    public interface IAuditable
    {
        AuditInfo AuditInfo { get; }
    }
}