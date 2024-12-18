class CommentCLI
{
    public static void CommentTweet(Tweet tweet)
    {
        Console.WriteLine("\nTryck Esc för att avbryta");
        Console.WriteLine("Skriv din kommentar:");

        string commentContent = Helpers.ReadUserInput();

        if (commentContent == null)
        {
            return;
        }

        CommentHandler.AddComment(commentContent, tweet);
    }

    public static void ShowComments(Tweet tweet, bool showIndex)
    {
        var ownComment = CommentHandler.GetOwnComments(tweet);

        foreach (var c in tweet.Comments)
        {
            var i = tweet.Comments.IndexOf(c);

            if (tweet.Author == UserCLI.loggedInUser.Username)
            {
                if (showIndex)
                {
                    Console.Write($"{i + 1}. ");
                }
            }
            else
            {
                if (showIndex && c.Author == UserCLI.loggedInUser.Username)
                {
                    i = ownComment.IndexOf(c);
                    Console.Write($"{i + 1}. ");
                }
            }

            Console.WriteLine(c.Author);
            Console.WriteLine(c.Content);
            Console.WriteLine(c.Timestamp.ToString("MM-dd HH:mm"));
            Console.WriteLine("----------------------");
        }
    }
    public static void RemoveComment(Tweet tweet)
    {
        Console.Clear();
        TweetCLI.ShowTweet(tweet, true);

        var ownComment = CommentHandler.GetOwnComments(tweet);

        Console.WriteLine("\nTryck Esc för att gå tillbaka");

        if (tweet.Author == UserCLI.loggedInUser.Username)
        {
            Console.WriteLine($"Välj vilken du vill radera (1-{tweet.Comments.Count})");
        }
        else
        {
            Console.WriteLine($"Välj vilken du vill radera (1-{ownComment.Count})");
        }

        int? index;
        while (true)
        {
            string choice = Helpers.ReadUserInput();

            if (choice == null)
            {
                return;
            }

            if (tweet.Author == UserCLI.loggedInUser.Username)
            {
                index = Helpers.CheckUserInput(tweet.Comments.Count, choice);

                if (index > 0 && index <= tweet.Comments.Count)
                {
                    break;
                }
            }
            else
            {
                index = Helpers.CheckUserInput(ownComment.Count, choice);

                if (index > 0 && index <= ownComment.Count)
                {
                    break;
                }
            }
        }

        var chosenComment = CommentHandler.GetCommentFromIndex((int)index, tweet, ownComment);

        Console.WriteLine("Du vill ta bort kommentaren:");
        Console.WriteLine("----------------------");
        Console.WriteLine(chosenComment.Author);
        Console.WriteLine(chosenComment.Content);
        Console.WriteLine(chosenComment.Timestamp.ToString("MM-dd HH:mm"));

        Console.WriteLine("\nTryck Enter för att radera");
        while (true)
        {
            var input = Console.ReadKey(true).Key;

            switch (input)
            {
                case ConsoleKey.Enter:
                    tweet.Comments.Remove(chosenComment);
                    return;

                case ConsoleKey.Escape:
                    return;

            }
        }
    }
}