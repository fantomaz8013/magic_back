namespace Magic.Domain;

public interface IHasBlocked
{
    bool IsBlocked { get; set; }

    DateTime? BlockedDate { get; set; }
}