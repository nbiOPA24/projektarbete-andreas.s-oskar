﻿using System.ComponentModel.Design;
using System.Dynamic;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Microsoft.VisualBasic;
using System.Text;

class Program
{
    public static void Main(string[] args)
    {   
        UserHandler.users = JsonSerializer.Deserialize<List<User>>(File.ReadAllText("Users.json"));
        TweetHandler.tweets = JsonSerializer.Deserialize<List<Tweet>>(File.ReadAllText("Tweets.json"));
        var options = new JsonSerializerOptions{WriteIndented = true}; 
        bool validUser = false;
          
        while (!validUser)
        {
            Console.WriteLine("Välkommen till Shitter");
            Console.WriteLine("1. Registrera");
            Console.WriteLine("2. Logga in");
            int choice = int.Parse(Console.ReadLine());
            
            switch (choice)
            {
                case 1:
                    UserHandler.Register();
                    File.WriteAllText("Users.json", JsonSerializer.Serialize(UserHandler.users, options));
                    break;
                
                case 2:
                    validUser = UserHandler.Login();
                    break;
            }
        }

        while (validUser)
        {
            TweetHandler.SortTweets();
            Console.WriteLine("----Shitter----");
            
            TweetHandler.ShowTweets(TweetHandler.tweets);
            
            Console.WriteLine("1. Tweet 2. Profil 3. Sök");
            
            int choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                TweetHandler.MakeTweet();
                break; 

                case 2:
                UserHandler.ShowUserProfile();
                break; 

                case 3:
                break;
            }
            
        }

        
    }
}

class User
{
    public string Username {get; set;}
    public string Password {get; set;}
    public string Name {get; set;}
    public List<User> Followers {get; set;} = new List<User>();
    public List<User> Following {get; set;} = new List<User>();
    public List<Tweet> OwnTweets {get; set;} = new List<Tweet>();
    public User(string username, string password, string name)
    {
        Username = username;
        Password = password;
        Name = name;
    }
}

static class UserHandler
{
    static string userName;
    static string password;
    public static string loggedInUser;
    public static List<User> users = new List<User>();
    static public void Register()
    {
        Console.Write("Ange användarnamn: ");
        userName = Console.ReadLine();
        Console.Write("Ange lössenord: ");
        password = Console.ReadLine();
        Console.Write("Ange förnamn och efternamn: ");
        string name = Console.ReadLine();
        users.Add(new User(userName, password, name));
    }
    static public bool Login()
    {
        bool validUser = false;
                    
        while (!validUser)
        {
            Console.Write("Ange användarnamn: ");
            userName = Console.ReadLine();
            Console.Write("Ange lössenord: ");
            password = Console.ReadLine();
            for (int i = 0; i < users.Count; i++)
            {
                if (!userName.Equals(users[i].Username))
                {
                    Console.WriteLine("Kan inte hitta ett konto med det användarnamn.");
                }
                else if (!password.Equals(users[i].Password))
                {
                    Console.WriteLine("Du har anget fel lössenord!");
                }
                else if (userName.Equals(users[i].Username) && password.Equals(users[i].Password))
                {
                    loggedInUser = userName;
                    return true;
                }
            }
        
        }
        return false;
    }
    public static void ShowUserProfile()
    {   
        Console.Clear();
        User loggedInUser = users.FirstOrDefault(u => u.Username == UserHandler.loggedInUser);
        Console.WriteLine(loggedInUser.Name);
        Console.WriteLine($"@{loggedInUser.Username}");
        Console.WriteLine($"Följare {loggedInUser.Followers.Count}\tFöljer {loggedInUser.Following.Count}");
        Console.WriteLine("----------------------");
        TweetHandler.ShowTweets(loggedInUser.OwnTweets);
        
    }
}
class Tweet 
{
    public string Content {get; set;}
    public string Author {get; set;}
    public DateTime Date {get; set;}

    public Tweet (string content, string author, DateTime date)
    {
        Content = content; 
        Author = author; 
        Date = date; 
    }
    
}

static class TweetHandler
{
    static public List<Tweet> tweets= new List<Tweet> ();

    public static void MakeTweet()
    {
        Console.Write("Skriv din tweet: ");
        string tweetContent = Console.ReadLine();
        
        var tweet = new Tweet(tweetContent, UserHandler.loggedInUser, DateTime.Now);
        
        tweets.Add(tweet);
        
        User loggedInUser = UserHandler.users.FirstOrDefault(u => u.Username == UserHandler.loggedInUser);
        loggedInUser.OwnTweets.Add(tweet);
        
        var options = new JsonSerializerOptions{WriteIndented = true}; 
        File.WriteAllText("Users.json", JsonSerializer.Serialize(UserHandler.users, options));
        File.WriteAllText("Tweets.json",JsonSerializer.Serialize(tweets, options)); 
    }
    public static void SortTweets()
    {
        for (int i = 0; i < tweets.Count; i++)
        {
            for (int j = 0; j < tweets.Count -1; j++)
            {
                if (tweets[j].Date < tweets[j + 1].Date) 
                {
                    var temp = tweets[j];
                    tweets[j] = tweets[j + 1];
                    tweets[j + 1] = temp;
                }
            }
        }
    }
    public static void ShowTweets(List<Tweet> tweet)
    {
            foreach(Tweet t in tweet)
            {
                Console.WriteLine(t.Author);
                Console.WriteLine(t.Content);
                Console.WriteLine(t.Date.ToString("MM-dd HH:mm"));
            }
    }
}