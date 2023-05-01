using MediatR;

public class UpdateRatingCommand : IRequest
{
    public string UserId { get; set; }
    public int NewEloRating { get; set; }
}