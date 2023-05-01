namespace API.Hubs;

public class EloSystem
{
    public static (int, int) CalculateNewRatings(int playerARating, int playerBRating, double resultPlayerA,
        double resultPlayerB, int k = 32)
    {
        // Обчислюємо очікувані ймовірності перемоги для кожного гравця
        var expectedPlayerAWinProbability = 1 / (1 + Math.Pow(10, (playerBRating - playerARating) / 400.0));
        var expectedPlayerBWinProbability = 1 / (1 + Math.Pow(10, (playerARating - playerBRating) / 400.0));

        // Оновлюємо рейтинги гравців на основі результатів гри
        var newPlayerARating = (int)Math.Round(playerARating + k * (resultPlayerA - expectedPlayerAWinProbability));
        var newPlayerBRating = (int)Math.Round(playerBRating + k * (resultPlayerB - expectedPlayerBWinProbability));

        // Обмеження на зміну рейтингу
        var deltaPlayerARating = Math.Abs(newPlayerARating - playerARating);
        var deltaPlayerBRating = Math.Abs(newPlayerBRating - playerBRating);
        if (deltaPlayerARating < 40)
            newPlayerARating = playerARating +
                               Math.Sign(resultPlayerA - expectedPlayerAWinProbability) * deltaPlayerARating;
        else
            newPlayerARating = playerARating + Math.Sign(resultPlayerA - expectedPlayerAWinProbability) * 40;
        if (deltaPlayerBRating < 40)
            newPlayerBRating = playerBRating +
                               Math.Sign(resultPlayerB - expectedPlayerBWinProbability) * deltaPlayerBRating;
        else
            newPlayerBRating = playerBRating + Math.Sign(resultPlayerB - expectedPlayerBWinProbability) * 40;

        return (newPlayerARating, newPlayerBRating);
    }
}