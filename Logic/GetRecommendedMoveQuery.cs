using Domain;
using Infrastructure.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class GetRecommendedMoveQuery : Query<long, CharacterBattleStateDto>
    {
        public GetRecommendedMoveQuery(IUnitOfWork uow) : base(uow)
        {
        }

        private const float CriticalHealthPercentage = 0.25f;
        private const float AggressiveAttackProbability = 0.7f;

        protected override async Task<long> ExecuteQuery(CharacterBattleStateDto? parameters)
        {
            if(parameters == null)
                return -1;

            bool isAggressive = Random.Shared.NextSingle() < AggressiveAttackProbability;
            bool isLowHP = parameters.CurrentHealth / (float)parameters.MaxHealth < CriticalHealthPercentage;

            var allMoves = await _uow.CharacterMoveRepository.Query()
                .Where(cm => cm.CharacterId == parameters.Id)
                .Include(cm => cm.Move).Select(cm => cm.Move)
                .ToListAsync();

            if(allMoves.Count == 0)
                return -1;

            long fallbackMoveId = allMoves[0].Id;

            var affordableMoves = allMoves.Where(m => CanAffordMove(m, parameters)).ToList();
            int randomIndex = Random.Shared.Next(0, affordableMoves.Count);

            if(affordableMoves.Count > 0)
            {
                fallbackMoveId = affordableMoves[randomIndex].Id;
            }

            if(isLowHP)
            {
                allMoves = allMoves.Where(m => m.SelfHealingAmount > 0 && CanAffordMove(m, parameters)).ToList();
            }
            else if(isAggressive)
            {
                allMoves = allMoves.Where(m => m.Damage > 0 && CanAffordMove(m, parameters)).ToList();
            }
            else
            {
                allMoves = allMoves.Where(m => m.Damage == 0 && m.SelfHealingAmount == 0 && CanAffordMove(m, parameters)).ToList();
            }

            if(allMoves.Count > 0)
            {
                randomIndex = Random.Shared.Next(0, allMoves.Count);
                return allMoves[randomIndex].Id;
            }

            return fallbackMoveId;
        }



        bool CanAffordMove(Move m, CharacterBattleStateDto parameters) => m.ManaCost <= parameters.CurrentMana && m.HealthCost < parameters.CurrentHealth;


    }
}
