using System;
using System.Text.Json;

namespace TrainManager_Suelen_Vicente
{
    public static class Utils
    {
        public static string convertNumberIntoAlphabet(int number)
        {
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var letter = "";

            if (number >= alphabet.Length)
                letter += alphabet[number / alphabet.Length - 1];

            letter += alphabet[number % alphabet.Length];

            return letter;

        }

        public static int convertLetterIntoNumber(char c)
        {
            return ((int)char.ToUpper(c)) - 64;
        }

        public static bool isValidJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return false;

            json = json.Trim();

            //always will be a list of seats, even if they were all empty
            if ((json.StartsWith("[") && json.EndsWith("]")))
            {
                try
                {
                    var obj = JsonDocument.Parse(json);
                    return true;
                }
                catch (JsonException jex)
                {
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static string howToUse()
        {
            return "Welcome to Train Ticket Manager! \n" +
                " Here are some instructions on how to use it:\n" +
                "Obs about the seats' status: \n" +
                "    a - Selected seats will be marked in blue with a * on the side;\n" +
                "    b - All the available seats are green with no additional text markings;\n" +
                "    c - Occupied seats are marked in red with square brackets around it;\n" +
                "\n" +
                "Manage Seats Tab\n" +
                "1 - First select a seat on the chart;\n" +
                "2 - Fill the passenger name on the right;\n" +
                "3 - Click on 'Add Passenger' button to confirm;\n" +
                "4 - This seat will be marked as occupied;\n" +
                "5 - To remove a passenger, select a seat on the chart and click on 'Remove' button;\n" +
                "6 - After removing a passenger, the seat will be marked as available;\n" +
                "7 - Seat status will be displayed in the fields below the buttons for every seat selected on the chart;\n" +
                "\n" +
                "Search Tab\n" +
                "1 - You can start searching by typing a passenger name or a seat;\n" +
                "2 - When clicking on 'Search' button all the info will be displayed in the fields below the buttons\n" +
                "3 - The searched seat will be marked in blue and with a * marking on it;\n" +
                "4 - After research you can remove the passenger and make the seat available again;\n" +
                "5 - Clicking on the seat on the chart will also return the seat status and passenger name;\n" +
                "6 - After searching, click on 'Clear' button to clear previous results and search again;\n" +
                "\n" +
                "Saving Files\n" +
                "1 - Files are always saved on the app current domain;\n" +
                "2 - Everytime you change the file in the dropbox, the file is saved;\n" +
                "3 - You can save the file pressing 'Save' button;\n" +
                "4 - To save a file with a different name, press 'Save New' button;\n" +
                "5 - To clear all seat plan press 'Delete All'\n" +
                "6 - To start a new file, press 'Delete All' on the current file and then 'Save New'\n" +
                "\n"+
                "To see this message again click on 'How To Use?' button on the top of the page;";
        }
    }
}
