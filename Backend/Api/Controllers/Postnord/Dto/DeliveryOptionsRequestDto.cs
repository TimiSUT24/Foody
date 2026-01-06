namespace Api.Controllers.Postnord.Dto
{
    public record DeliveryOptionsRequestDto
    {
        public RecipientDto Recipient { get; set; } = null!;
    }
}
