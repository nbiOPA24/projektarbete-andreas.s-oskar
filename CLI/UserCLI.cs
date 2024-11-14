using System.Xml;

class UserCLI
{
    public static void Register()
    {
        Console.WriteLine("Registrering");
        Console.WriteLine("---------------------");
        Console.WriteLine("Tryck esc för att avbryta");

        string username;
        while (true)
        {
            Console.Write("Ange användarnamn: ");
            username = Helpers.ReadUserInput();
            
            if (string.IsNullOrWhiteSpace(username))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Användarnamn får inte vara tomt.");
                Console.ForegroundColor = ConsoleColor.White;
                continue;
            }

            if(UserHandler.IsUsernameAvailable(username))
            {
                break;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Användarnamn upptaget.");
                Console.ForegroundColor = ConsoleColor.White;
                continue;
            }
        }

        string password;  
        while (true)    
        {
            Console.Write("Ange lösenord: ");
            password = Helpers.ReadUserInput();

            if (string.IsNullOrWhiteSpace(password))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Lösenordet får inte vara tomt.");
                Console.ForegroundColor = ConsoleColor.White;
                continue;
            }

            Console.Write("Bekräfta lösenordet: ");
            string confirmPassword = Helpers.ReadUserInput();

            if (password != confirmPassword)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Lösenorden matchar inte. Försök igen!");
                Console.ForegroundColor = ConsoleColor.White;
                continue;
            }
            else
            {
                break;
            }
        }

        string name;
        while (true)
        {
            Console.Write("Ange förnamn och efternamn: ");
            name = Helpers.ReadString();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Namn får inte vara tomt.");
                Console.ForegroundColor = ConsoleColor.White;
                continue;
            }
            else
            {
                break;
            }
        }
        UserHandler.AddNewUser(username, password, name);
    }
    public static bool LogIn()
    {
        bool validUser = false;
        
        while (!validUser)
        {
            Console.WriteLine("-----Logga in-----");
            
            Console.Write("Ange användarnamn: ");
            string username = Helpers.ReadString();
            
            Console.Write("Ange Lössenord: ");
            string password = Helpers.ReadString();
            
            validUser = UserHandler.validLogIn(username, password);
        }
        
        return validUser;
    }
}