using Shared.DataTransferObjects;
using Shared.Enumerators;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Network), typeof(BackgroundVisibilityController), typeof(BattleLog))]
public class GameManager : SingletonPersistent<GameManager>
{
    [SerializeField] private Network _network;
    [SerializeField] private BackgroundVisibilityController _bgController;

    [SerializeField] private BattleLog _battleLog;

    [SerializeField] private float _delayBetweenMoveAndEffectInSeconds;
    [SerializeField] private float _delayBetweenEffectAndNextTurnInSeconds;

    public int HeroGold { get; private set; }
    public int HeroLevel { get; private set; }

    public int XpToNextLevel { get; private set; } = 100;

    public int HeroAttackLevel { get; private set; }
    public int HeroDefenseLevel { get; private set; }

    public int HeroMagicLevel { get; private set; }

    public int HeroHealthLevel { get; private set; }
    public int HeroManaLevel { get; private set; }

    public List<MoveDto> HeroLearnedMoves { get; private set; } = new();
    public List<ItemDto> HeroOwnedItems { get; private set; } = new();

    public TurnState BattleTurnState => _battleTurnState;

    private EncounterTreeNodeDto _encounterTreeRoot { get; set; }
    private EncounterTreeNodeDto _encounterTreeCurrentNode { get; set; }

    private TurnState _battleTurnState = TurnState.HERO;

    public long RecommendedCharacterMoveId { get; private set; } = -1;

    private void Start() => OpenMainMenu();

    private void OpenMainMenu()
    {
        bool doesSaveExist = SaveManager.Instance.HasSave();

        UIManager.Instance.OpenMenu<MainMenuData>(new(doesSaveExist));
    }

    public void StartNewGame()
    {
        StartCoroutine(StartNewGameRoutine());
    }

    public void PlayMove(DamageContext damageContext, FXContext fxContext)
    {
        StartCoroutine(PlayMoveRoutine(damageContext, fxContext));
    }

    private IEnumerator PlayMoveRoutine(DamageContext damageContext, FXContext fxContext)
    {
        TurnState attackIsFromTurn = _battleTurnState;

        _battleTurnState = TurnState.WAITING;

        _battleLog.Log(new(damageContext.SenderName, damageContext.MoveName));
        yield return new WaitForSeconds(_delayBetweenMoveAndEffectInSeconds);

        ActivateFX(fxContext);
        yield return new WaitForSeconds(_delayBetweenEffectAndNextTurnInSeconds);

        if(attackIsFromTurn == TurnState.HERO)
        {
            BattleEvents.InvokeMonsterWasAttacked(damageContext);
            _battleTurnState = TurnState.MONSTER;
        }
        else
        {
            BattleEvents.InvokeHeroWasAttacked(damageContext);
            _battleTurnState = TurnState.HERO;
        }

        BattleEvents.InvokeTurnStarted();

    }

    public void ActivateFX(FXContext fXContext)
    {
        //TODO
    }

    private IEnumerator StartNewGameRoutine()
    {
        yield return _network.GetHeroes();
        if(!_network.LastRequestSuccess) yield break;

        var hero = _network.Heroes[0];
        GameEvents.InvokeHeroSelected(hero);

        yield return _network.GetEncounterTree();
        if(!_network.LastRequestSuccess) yield break;

        _encounterTreeRoot = _network.EncounterTree;
        _encounterTreeCurrentNode = _encounterTreeRoot;

        GameEvents.InvokeEncounterSelected(_encounterTreeCurrentNode.Encounter.Enemy);

        _bgController.ShowBackgroundFromDifficulty(_encounterTreeCurrentNode.Encounter.Difficulty);

        BattleEvents.InvokeBattleStarted();
    }

    public IEnumerator GetRecommendedMoveCoroutine(CharacterBattleStateDto dto)
    {
        yield return _network.GetMove(dto);

        if(_network.LastRequestSuccess)
        {
            RecommendedCharacterMoveId = _network.RecommendedMoveId;
        }

        else RecommendedCharacterMoveId = -1;
    }
}
