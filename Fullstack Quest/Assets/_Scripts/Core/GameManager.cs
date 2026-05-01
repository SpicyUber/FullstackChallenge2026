using DG.Tweening;
using Shared.DataTransferObjects;
using Shared.Enumerators;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Network), typeof(BackgroundVisibilityController), typeof(BattleLog))]
public class GameManager : SingletonPersistent<GameManager>
{
    [Header("Dependencies")]
    [SerializeField] private Network _network;
    [SerializeField] private Hero _hero;

    [SerializeField] private BackgroundVisibilityController _bgController;
    [SerializeField] private BattleLog _battleLog;

    [Header("Battle Timing")]
    [SerializeField] private float _delayBetweenMoveAndEffectInSeconds;
    [SerializeField] private float _delayBetweenEffectAndNextTurnInSeconds;

    [Header("Move FX")]
    [SerializeField] private Transform _fxTransform;
    [SerializeField] private SpriteRenderer _fxRenderer;
    [SerializeField] private Animator _fxAnimator;

    [Header("No damage FX")]
    [SerializeField] public ParticleSystem _blockedParticle;

    public int HeroAttackLevel { get => _hero.AttackLevel; set => _hero.AttackLevel = value; }
    public int HeroDefenseLevel { get => _hero.DefenseLevel; set => _hero.DefenseLevel = value; }

    public int HeroMagicLevel { get => _hero.MagicLevel; set => _hero.MagicLevel = value; }

    public int HeroHealthLevel { get => _hero.HealthLevel; set => _hero.HealthLevel = value; }
    public int HeroManaLevel { get => _hero.ManaLevel; set => _hero.ManaLevel = value; }

    public List<MoveDto> HeroLearnedMoves { get => _hero.LearnedMoves; private set => _hero.LearnedMoves = value; }
    public List<ItemDto> HeroOwnedItems { get => _hero.OwnedItems; private set => _hero.OwnedItems = value; }

    public int LevelUpStatTokens { get; set; } = 0;

    public int Gold { get; private set; }

    public int XpToNextLevel { get; private set; } = 10;

    public TurnState BattleTurnState => _battleTurnState;
    public long RecommendedCharacterMoveId { get; private set; } = -1;

    public float DelayBetweenMoveAndNextTurnInSeconds
    => _delayBetweenMoveAndEffectInSeconds
    + _delayBetweenEffectAndNextTurnInSeconds;

    private EncounterTreeNodeDto _encounterTreeRoot { get; set; }
    private EncounterTreeNodeDto _encounterTreeCurrentNode { get; set; }

    private TurnState _battleTurnState = TurnState.HERO;

    private const int NextLevelThresholdMultiplier = 2;

    public void StartNewGame()
    {
        StartCoroutine(StartNewGameRoutine());
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

    public void PlayMove(DamageContext damageContext, FXContext fxContext)
    {
        StartCoroutine(PlayMoveRoutine(damageContext, fxContext));
    }

    private void ActivateFX(FXContext fxContext)
    {
        _fxRenderer.enabled = true;
        _fxRenderer.flipX = fxContext.FlipSprite;

        PlayMoveSFX(fxContext.SFXType);
        PlayMoveAnimation(fxContext.VFXType);

        ColorMove(fxContext.Element);
        ApplyMoveMovement(fxContext);
    }

    private void ApplyMoveMovement(FXContext fxContext)
    {
        if(fxContext.VFXType == MoveVFXType.PROJECTILE)
            _fxTransform.DOMoveX(-fxContext.CastPosition.x, _delayBetweenEffectAndNextTurnInSeconds);
        else
            _fxTransform.position
                = new Vector3(((fxContext.IsSelfCast) ? 1 : -1) * fxContext.CastPosition.x, fxContext.CastPosition.y, fxContext.CastPosition.z);
    }

    private void ColorMove(Element element) => _fxRenderer.color = LookupDictionaries.Instance.GetColor(element);

    private void PlayMoveAnimation(MoveVFXType vfxType) => _fxAnimator.Play(vfxType.ToString());

    private void PlayMoveSFX(MoveSFXType sfxType) => AudioSource.PlayClipAtPoint(LookupDictionaries.Instance.GetSound(sfxType), Camera.main.transform.position);

    private void Start() => OpenMainMenu();

    private void OpenMainMenu()
    {
        bool doesSaveExist = SaveManager.Instance.HasSave();

        UIManager.Instance.OpenMenu<MainMenuData>(new(doesSaveExist));
    }

    private void OnGameOver()
    {
        _battleTurnState = TurnState.WAITING;
        _bgController.HideBackgrounds();
        _bgController.StopBGMusic();

        UIManager.Instance.OpenMenu<MessageBoxData>(
        new(
            new string[] { "Game over." },
            () => UIManager.Instance.OpenMenu<MainMenuData>(new(false))
        ));
    }

    private void OnBattleWon()
    {
        _battleTurnState = TurnState.WAITING;
        List<string> battleReport = CollectRewardsAndGetBattleReport();
        _battleTurnState = TurnState.WAITING;
        _bgController.HideBackgrounds();
        _bgController.StopBGMusic();

        UIManager.Instance.OpenMenu<MessageBoxData>(
        new(
        battleReport.ToArray(),
        GoToShop
        ));
    }

    private void GoToShop()
    {
        _bgController.HideBackgrounds();
        _bgController.StopBGMusic();
        StartCoroutine(ShopRoutine());
    }

    private IEnumerator ShopRoutine()
    {
        yield return _network.GetShop();
        if(!_network.LastRequestSuccess) yield break;

        UIManager.Instance.OpenMenu<ShopMenuData>(
            new(
            Gold,
            LevelUpStatTokens,
            _network.Shop
            ));
    }

    public bool TryBuy(ItemDto item)
    {
        if(item.Price > Gold) return false;

        HeroOwnedItems.Add(item);
        Gold -= item.Price;
        return true;
    }

    public void GoToOverview() => UIManager.Instance.OpenMenu<OverviewMenuData>(new(_hero, _encounterTreeCurrentNode));


    public void GoToTextBattle(bool goesToRightNode)
    {
        StartEncounter(goesToRightNode ? _encounterTreeCurrentNode.Right : _encounterTreeCurrentNode.Left);
    }

    public void PlayBlockedParticle(Vector3 worldPosition)
    {
        _blockedParticle.transform.position = worldPosition;
        _blockedParticle.gameObject.SetActive(true);
        _blockedParticle.Play();
    }

    private List<string> CollectRewardsAndGetBattleReport()
    {
        _battleTurnState = TurnState.WAITING;
        List<string> battleReport = new();

        MoveDto learnedMove = CollectAndGetLearnedMove(_encounterTreeCurrentNode);

        int levelXp = _encounterTreeCurrentNode.Encounter.Xp;

        battleReport.Add($"Congrats! You've learned the move {learnedMove.Name}.\n"
            + $"You've gained {levelXp}XP and leveled up {TryLevelUpHero(levelXp)} times");

        string itemsAndGoldReport = "";
        ItemDto item = CollectAndGetLevelItem(_encounterTreeCurrentNode);

        if(item != null)
        {
            itemsAndGoldReport += $"Lucky! The enemy dropped a {item.Name}.\n";
        }

        Gold += _encounterTreeCurrentNode.Encounter.Gold;
        itemsAndGoldReport += $"The enemy had {_encounterTreeCurrentNode.Encounter.Gold} gold on them.";

        battleReport.Add(itemsAndGoldReport);

        return battleReport;
    }

    private int TryLevelUpHero(int xp)
    {
        int levelUps = 0;

        while(xp >= XpToNextLevel)
        {
            levelUps++;

            var formerXpToNextLevel = XpToNextLevel;
            xp -= XpToNextLevel;

            XpToNextLevel = formerXpToNextLevel * NextLevelThresholdMultiplier;
            LevelUpStatTokens++;
        }

        XpToNextLevel -= xp;
        xp = 0;

        return levelUps;
    }

    private ItemDto CollectAndGetLevelItem(EncounterTreeNodeDto encounterTreeCurrentNode)
    {
        var item = encounterTreeCurrentNode.Encounter.Item;

        if(item != null)
            HeroOwnedItems.Add(item);

        return item;
    }

    private MoveDto CollectAndGetLearnedMove(EncounterTreeNodeDto encounterTreeCurrentNode)
    {
        var enemyMoves = encounterTreeCurrentNode.Encounter.Enemy.Moves;
        var learnedMove = enemyMoves[Random.Range(0, enemyMoves.Count)];

        HeroLearnedMoves.Add(learnedMove);

        return learnedMove;
    }

    private IEnumerator PlayMoveRoutine(DamageContext damageContext, FXContext fxContext)
    {
        TurnState attackIsFromTurn = _battleTurnState;
        _battleTurnState = TurnState.WAITING;

        _battleLog.Log(new(damageContext.SenderName, damageContext.MoveName));
        yield return new WaitForSeconds(_delayBetweenMoveAndEffectInSeconds);

        ActivateFX(fxContext);
        yield return new WaitForSeconds(_delayBetweenEffectAndNextTurnInSeconds);

        _fxRenderer.enabled = false;

        if(attackIsFromTurn == TurnState.HERO)
        {
            _battleTurnState = TurnState.MONSTER;
            BattleEvents.InvokeMonsterWasAttacked(damageContext);
        }
        else
        {
            _battleTurnState = TurnState.HERO;
            BattleEvents.InvokeHeroWasAttacked(damageContext);
        }

        BattleEvents.InvokeTurnStarted();
    }

    private IEnumerator StartNewGameRoutine()
    {
        XpToNextLevel = 10;
        LevelUpStatTokens = 0;
        Gold = 0;

        HeroAttackLevel = 0;
        HeroDefenseLevel = 0;
        HeroMagicLevel = 0;

        HeroHealthLevel = 0;
        HeroManaLevel = 0;

        yield return _network.GetHeroes();
        if(!_network.LastRequestSuccess) yield break;

        var hero = _network.Heroes[1];
        GameEvents.InvokeHeroSelected(hero);

        yield return _network.GetEncounterTree();
        if(!_network.LastRequestSuccess) yield break;

        _encounterTreeRoot = _network.EncounterTree;

        StartEncounter(_encounterTreeRoot);
    }

    public void SkipMove(string senderName, string moveName)
    {
        StartCoroutine(SkipMoveRoutine(senderName, moveName));
    }

    private IEnumerator SkipMoveRoutine(string senderName, string moveName)
    {
        TurnState attackIsFromTurn = _battleTurnState;
        _battleTurnState = TurnState.WAITING;

        var logEntry = new BattleLogEntry(senderName, moveName);

        logEntry.SetFailed();
        _battleLog.Log(logEntry);

        yield return new WaitForSeconds(_delayBetweenMoveAndEffectInSeconds);

        if(attackIsFromTurn == TurnState.HERO)
        {
            BattleEvents.InvokeMonsterWasAttacked(null);
            _battleTurnState = TurnState.MONSTER;
        }
        else
        {
            BattleEvents.InvokeHeroWasAttacked(null);
            _battleTurnState = TurnState.HERO;
        }

        BattleEvents.InvokeTurnStarted();
    }

    private void StartEncounter(EncounterTreeNodeDto encounterTreeCurrentNode)
    {
        _encounterTreeCurrentNode = encounterTreeCurrentNode;

        GameEvents.InvokeEncounterSelected(encounterTreeCurrentNode.Encounter.Enemy);

        _bgController.ShowBackgroundFromDifficulty(encounterTreeCurrentNode.Encounter.Difficulty);
        _bgController.PlayBGMusic();

        _battleTurnState = TurnState.HERO;
        BattleEvents.InvokeBattleStarted();
    }

    private void OnEnable()
    {
        BattleEvents.PlayerDied += OnGameOver;
        BattleEvents.MonsterDied += OnBattleWon;
    }

    private void OnDisable()
    {
        BattleEvents.PlayerDied -= OnGameOver;
        BattleEvents.MonsterDied -= OnBattleWon;
    }
}
