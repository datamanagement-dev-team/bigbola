namespace BlueBrown.BigBola.Application.Entities
{
    public record WalletAction
    {
        public int Id { get; init; }
        public decimal Tax { get; init; }
        public DateTime Date { get; init; }
        public string Type { get; init; } = string.Empty;
        public decimal Profit { get; init; }
        public decimal Investment { get; init; }
        public decimal NetPayment { get; init; }
        public Player Player { get; init; }
    }
}
