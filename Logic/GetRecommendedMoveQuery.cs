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

        private const float CriticalHealthPercentage = 0.2f;
        private const float AggressiveAttackProbability = 0.7f;

        protected override async Task<long> ExecuteQuery(CharacterBattleStateDto? parameters)
        {
            if(parameters == null)
                return -1;

            bool isAggressive = Random.Shared.NextSingle() < AggressiveAttackProbability;
            bool isLowHP = parameters.CurrentHealth / (float)parameters.MaxHealth < CriticalHealthPercentage;

            var moves = await _uow.CharacterMoveRepository.Query()
                .Where(cm => cm.CharacterId == parameters.Id)
                .Include(cm => cm.Move).Select(cm => cm.Move)
                .ToListAsync();

            if(moves.Count == 0)
                return -1;

            long defaultMoveId = moves[0].Id;

            if(isLowHP)
            {
                moves = moves.Where(m => m.SelfHealingAmount > 0 && CanAffordMove(m, parameters)).ToList();
            }
            else if(isAggressive)
            {
                moves = moves.Where(m => m.Damage > 0 && CanAffordMove(m, parameters)).ToList();
            }
            else
            {
                moves = moves.Where(m => m.Damage == 0 && CanAffordMove(m, parameters)).ToList();
            }

            if(moves.Count > 0) return moves[0].Id;

            return defaultMoveId;
        }



        bool CanAffordMove(Move m, CharacterBattleStateDto parameters) => m.ManaCost <= parameters.CurrentMana && m.HealthCost < parameters.CurrentHealth;


    }
}
