using Domain;
using Infrastructure.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class GetAllHeroesQuery : Query<List<CharacterDto>, object>
    {
        public GetAllHeroesQuery(IUnitOfWork uow) : base(uow)
        {
        }

        protected override async Task<List<CharacterDto>> ExecuteQuery(object parameters)
        {
            var heroes = await _uow.CharacterRepository.Query()
            .Where(c => c.IsHero)
            .Include(c => c.Moves)
            .ThenInclude(cm => cm.Move)
            .ThenInclude(m => m.Effect)
            .ToListAsync();

            List<CharacterDto> result = new();

            foreach(var hero in heroes)
            {
                List<MoveDto> moves = new List<MoveDto>();

                foreach(var characterMove in hero.Moves)
                {
                    var move = characterMove.Move;
                    var effectDto = GetEffectFromMove(move);

                    var moveDto = new MoveDto(
                        move.Id,
                        move.Name,
                        move.DamageType,
                        move.Damage,
                        move.SelfHealingAmount,
                        move.HealthCost,
                        move.ManaCost,
                        move.Element,
                        effectDto,
                        move.isVFXSelfCast,
                        move.VFXType,
                        move.SFXType);

                    moves.Add(moveDto);
                }

                result.Add(new CharacterDto(
                    hero.Id,
                    hero.Name,
                    hero.Health,
                    hero.Mana,
                    hero.Attack,
                    hero.Defense,
                    hero.Magic,
                    hero.Type,
                    moves
                    ));
            }

            return result;
        }

        private EffectDto GetEffectFromMove(Move move)
        {
            if(move == null) return null;
            var effect = move.Effect;

            if(effect == null) return null;
            return new EffectDto(effect.Id, effect.Amount, effect.IsDebuff, effect.Duration, effect.Type);
        }
    }
}
