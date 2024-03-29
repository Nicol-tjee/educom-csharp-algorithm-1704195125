﻿using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using BornToMove.DAL;
using System.Linq;

namespace BornToMove.Business
{
    public class BuMove
    {
        private MoveCrud MoveCrud;

        public BuMove(MoveCrud moveCrud)
        {
            MoveCrud = moveCrud;
        }

        public void CheckAndAddMovesIfEmptyDb()
        {
            if (!MoveCrud.IsAnyMove())
            {
                MoveCrud.CreateMove(new Move {Name = "Push up", Description = "Ga horizontaal liggen op teentoppen en handen. Laat het lijf langzaam zakken tot de neus de grond bijna raakt. Duw het lijf terug nu omhoog tot de ellebogen bijna gestrekt zijn. Vervolgens weer laten zakken. Doe dit 20 keer zonder tussenpauzes."});
                MoveCrud.CreateMove(new Move {Name = "Planking", Description = "Ga horizontaal liggen op teentoppen en onderarmen. Houdt deze positie 1 minuut vast."});
                MoveCrud.CreateMove(new Move {Name = "Squat", Description = "Ga staan met gestrekte armen. Zak door de knieën tot de billen de grond bijna raken. Ga weer volledig gestrekt staan. Herhaal dit 20 keer zonder tussenpauzes."});
            }
        }

        //Willekeurige Move 
        public MoveRating GetRandomMove()
        {
            return MoveCrud.ReadRandomMove();
        }

        //Lijst met alle moves
        public List<MoveRating> GetAllMoves()
        {
            return MoveCrud.ReadAllMoves();
        }

        //Move by id
        public MoveRating GetMoveById(int moveId)
        {
            return MoveCrud.ReadMoveById(moveId);
        }

        //Move by name
        public Move? GetMoveByName(string name)
        {
            return MoveCrud.ReadMoveByName(name);
        }

        //Move controleren en opslaan
        public void SaveMove(string name, string description)
        {
            Move move = new Move()
            {
                Name = name, 
                Description = description, 
            };
            MoveCrud.CreateMove(move);
        }

        //Move controleren en updaten
        public void UpdateMove(Move move)       
        {
            MoveCrud.UpdateMoveById(move);
        }

        //Rating toevoegen
        public void AddMoveRating(int moveId, int rating, int vote)
        {
            var move = MoveCrud.ReadMoveById(moveId);
            if (move != null)
            {
                MoveRating moveRating = new MoveRating(move.Move, rating, vote);
                MoveCrud.CreateMoveRating(moveRating);
            }
            else 
            {
                Console.WriteLine("Helaas is deze oefening niet gevonden.");
            }
        }
        public double CalcAvgMoveRating(List<MoveRating> moveRatings)
        {
            if (moveRatings.Any())
            {
                return moveRatings.Average(mr => mr.Rating);
            }
            return 0;
        }
    }
}