using System;
using System.Collections.Generic;
using System.Transactions;
using BornToMove.Business;
using BornToMove.DAL;

namespace BornToMove
{
    class Program
    {
        static void Main(string[] args)
        {
            MoveContext moveContext = new MoveContext();
            MoveCrud moveCrud = new MoveCrud(moveContext);
            BuMove buMove = new BuMove(moveCrud);

            buMove.CheckAndAddMovesIfEmptyDb();
            
            Console.WriteLine("Het is tijd om te bewegen!");
            Console.WriteLine("Wilt u een suggestie of zelf een oefening kiezen uit de lijst?");
            Console.WriteLine("Typ 'suggestie' of 'lijst' en druk op enter:");
            string answer;
            int moveId;
            int moveIndex;
            string input;
            do
            {
                answer = Console.ReadLine()?.ToLower();
                if (answer == "suggestie") 
                {
                    
                    Console.WriteLine("---------------");
                    Console.WriteLine("Er wordt een willekeurige oefening gekozen...");
                    Console.WriteLine("---------------");
                    var randomMove = buMove.GetRandomMove();
                    moveId = randomMove.Move.Id;
                    PrintResult(randomMove);
                    GiveReview(buMove, moveId);
                }
                else if (answer == "lijst")
                {
                    Console.WriteLine("---------------");
                    Console.WriteLine("Alle oefeningen worden opgehaald...");
                    var allMoves = buMove.GetAllMoves();
                    PrintResults(allMoves);
                    while (true)
                    { 
                        Console.WriteLine("Kies een oefening uit de lijst en geef hier het nummer in en druk op enter:");
                        Console.WriteLine("Als u een oefening wilt toevoegen, typ 0.");
                        input = Console.ReadLine();
                        if (int.TryParse(input, out moveIndex))
                        {
                            if (moveIndex == 0)
                            {
                                Console.WriteLine("Geef de naam van de oefening en druk op enter:");
                                string name = Console.ReadLine();
                                Console.WriteLine("Controleren of deze oefening nog niet bestaat in het systeem...");
                                var result = buMove.GetMoveByName(name);
                                if (result != null)
                                {
                                    Console.WriteLine("Deze oefening bestaat al.");
                                }
                                else 
                                {
                                    Console.WriteLine("Geef de omschrijving van de oefening en druk op enter:");
                                    string description = Console.ReadLine();
                                    buMove.SaveMove(name, description);
                                    Console.WriteLine("De oefening is succesvol aangemaakt.");
                                    Console.WriteLine("Naam: " + name + ", omschrijving: " + description);
                                    break;
                                }
                            } 
                            else 
                            {
                                if (moveIndex >= 1 && moveIndex <= allMoves.Count)
                                {
                                    moveId = allMoves[moveIndex  - 1].Move.Id;
                                    Console.WriteLine("---------------");
                                    Console.WriteLine("U koos de oefening met nummer: " + moveIndex + ". Deze oefening wordt opgehaald...");
                                    Console.WriteLine("---------------");
                                    var moveById = buMove.GetMoveById(moveId);
                                    if (moveById != null)
                                    {
                                        PrintResult(moveById);
                                        GiveReview(buMove, moveId);
                                    } 
                                    else 
                                    {   
                                        Console.WriteLine("Er ging iets mis.");
                                        continue;
                                    }
                                } else 
                                {
                                    Console.WriteLine("Er ging iets mis, kies een nummer uit de lijst.");
                                    continue;
                                }
                            break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Er ging iets mis, kies een nummer uit de lijst.");
                        }
                    }
                } 
                else {
                    Console.WriteLine("Typ 'suggestie' of 'lijst. En druk op enter.");
                } 

            } while (answer != "suggestie" && answer != "lijst");
            
            // Sluit de toepassing
            Console.WriteLine("Druk op een toets om af te sluiten...");
            Console.ReadKey();
        }

        static void PrintResults(List<MoveRating> moves)
        {
            for (int i = 0; i < moves.Count; i++)
            {
                Console.WriteLine($"Nummer: {i + 1}");
                PrintResult(moves[i]);
            }
        }

        static void PrintResult(MoveRating moveRating)
        {
            if (moveRating != null)
            {
                Console.WriteLine($"Naam: {moveRating.Move.Name}");
                Console.WriteLine($"Omschrijving: {moveRating.Move.Description}");
                Console.WriteLine($"Inspanningsniveau: {moveRating.Rating}");

                Console.WriteLine("---------------");
            }
            else
            {
                Console.WriteLine("Geen resultaten voor dit nummer.");
            }
        }

        static void GiveReview(BuMove buMove, int moveId)
        {
            Console.WriteLine("Geef alstublieft nog even uw mening over de oefening.");
            Console.WriteLine("Beoordeling, kies een getal van 1-5 en druk op enter:");
            int vote = int.Parse(Console.ReadLine());
            Console.WriteLine("Intensiteit, kies een getal van 1-5 en druk op enter:");
            int rating = int.Parse(Console.ReadLine());
            Console.WriteLine("U gaf deze oefening een " + vote + " en voor intensiteit een " + rating + ". Bedankt voor uw beoordeling!");
            buMove.AddMoveRating(moveId, rating, vote);
        }
    }
}
