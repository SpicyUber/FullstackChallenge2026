using Domain;
using Infrastructure.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Shared.DataTransferObjects;
using Shared.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class GetEncounterMapQuery : Query<EncounterTreeNodeDto, object>
    {
        public GetEncounterMapQuery(IUnitOfWork uow) : base(uow)
        {
        }

        protected override async Task<EncounterTreeNodeDto> ExecuteQuery(object? parameters)
        {
            var encounters =
                 await _uow.EncounterRepository.Query()
                .Include(encounter => encounter.Item)
                .Include(encounter => encounter.Effect)
                .Include(en => en.Enemy).ThenInclude(character => character.Moves)
                .ThenInclude(characterMove => characterMove.Move).ThenInclude(move => move.Effect)
                .ToListAsync();

            encounters = encounters.OrderBy(e => e.Difficulty).ThenBy(e => e.Id).ToList();

            List<EncounterDto> encounterList = new();

            foreach(var encounter in encounters)
            {
                ItemDto item = GetItemFromEncounter(encounter);
                EffectDto effect = GetEffectFromEncounter(encounter);
                CharacterDto enemy = GetCharacterFromEncounter(encounter);

                encounterList.Add(new(
                    encounter.Id, encounter.Gold,
                    encounter.Xp,
                    encounter.Difficulty,
                    enemy,
                    item,
                    effect));
            }

            EncounterTreeNodeDto root = CreateBinaryTree(0,encounterList);

            return root;
        }

        private EncounterTreeNodeDto CreateBinaryTree(int index, List<EncounterDto> encounterList)
        {
            if(index >= encounterList.Count) return null;
            return new EncounterTreeNodeDto(encounterList[index],CreateBinaryTree(index*2+1,encounterList), CreateBinaryTree(index * 2 + 2, encounterList));
        }

        private CharacterDto GetCharacterFromEncounter(Encounter encounter)
        {
            var enemy = encounter.Enemy;

            List<MoveDto> enemyMoves = new();

            foreach(var characterMove in enemy.Moves)
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

                enemyMoves.Add(moveDto);
            }


            return new CharacterDto(
                enemy.Id,
                enemy.Name,
                enemy.Health,
                enemy.Mana,
                enemy.Attack,
                enemy.Defense,
                enemy.Magic,
                enemy.Type,
                enemyMoves

                );
        }

        private EffectDto GetEffectFromMove(Move move)
        {
            if(move == null) return null;
            var effect = move.Effect;

            if(effect == null) return null;
            return new EffectDto(effect.Id, effect.Amount, effect.IsDebuff, effect.Duration, effect.Type);
        }

        private EffectDto GetEffectFromEncounter(Encounter encounter)
        {
            if(encounter == null || encounter.Effect == null) return null;

            var effect = encounter.Effect;

            return new EffectDto(effect.Id, effect.Amount, effect.IsDebuff, effect.Duration, effect.Type);
        }

        private ItemDto GetItemFromEncounter(Encounter encounter)
        {
            if(encounter == null || encounter.Item == null) return null;

            var item = encounter.Item;

            return new ItemDto(item.Id,
                item.Name,
                item.Price,
                item.Type,
                item.AttackDelta,
                item.DefenseDelta,
                item.HealthDelta,
                item.ManaDelta,
                item.MagicDelta);
        }
    }
}
