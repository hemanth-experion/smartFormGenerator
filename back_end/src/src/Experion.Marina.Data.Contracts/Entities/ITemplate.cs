namespace Experion.Marina.Data.Contracts.Entities
{
    public interface ITemplate
    {
        long Id { get; set; }
        string TemplateName { get; set; }
        long UserId { get; set; }
    }
}
