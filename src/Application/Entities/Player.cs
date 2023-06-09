namespace BlueBrown.BigBola.Application.Entities
{
    public record Player
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string SecondLastName { get; init; } = string.Empty;
        public DateTime Birthdate { get; init; }
        public string Nationality { get; init; } = string.Empty;
        public int TypeDocument { get; init; }
        public long IdDocument { get; init; }
        public string Rfc { get; init; } = string.Empty;
        public string Curp { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Street { get; init; } = string.Empty;
        public string Neighborhood { get; init; } = string.Empty;
        public int ExternalNo { get; init; }
        public int InternalNo { get; init; }
        public string City { get; init; } = string.Empty;
        public string County { get; init; } = string.Empty;
        public string State { get; init; } = string.Empty;
        public string Country { get; init; } = string.Empty;
        public int ZipCode { get; init; }
    }
}
