using System.Collections;
using UnityEngine;

using UnityEngine.SceneManagement;


public class BattleManagerScript : MonoBehaviour
{

    public GameObject player;
    public BattleStateEnum state;
    public GameObject battleUIGameObject;

    private BattleUI battleUIScript;
    private PlayerBattle playerBattle;
    private EnemyBattle enemyBattle;

    void Start()
    {
        StartCoroutine(WaitForEnemyToBeReady());
    }

    IEnumerator WaitForEnemyToBeReady()
    {
        // Esperar hasta que el enemigo se haya instanciado
        while (EnemySpawner.currentEnemy == null)
        {
            yield return null;
        }

        playerBattle = player.GetComponent<PlayerBattle>();
        enemyBattle = EnemySpawner.currentEnemy.GetComponent<EnemyBattle>();
        battleUIScript = battleUIGameObject.GetComponent<BattleUI>();
        battleUIScript.Init(enemyBattle);
        battleUIScript.UpdateLifeTexts();
        ChangeToPlayerTurn();
    }

    void Update()
    {
        if (state.Equals(BattleStateEnum.ENEMYTURN))
        {

            state = BattleStateEnum.WAITING;
            StartCoroutine(EnemyAttackPlayerAfterDelay(1.5f));
        }
    }

    void ChangeToPlayerTurn()
    {

        state = BattleStateEnum.PLAYERTURN;
        battleUIScript.UpdateTurnText(BattleStateEnum.PLAYERTURN);
        battleUIScript.ShowButtons(true);
    }

    void ChangeToEnemyTurn()
    {
        battleUIScript.UpdateTurnText(BattleStateEnum.ENEMYTURN);
        state = BattleStateEnum.ENEMYTURN;
        battleUIScript.ShowButtons(false);
    }

    IEnumerator EnemyAttackPlayerAfterDelay(float delay)
    {

        yield return new WaitForSeconds(delay);

        int damage = 1;
        if (enemyBattle.attack > PlayerStats.Instance.getDefense())
        {
            damage = enemyBattle.attack - PlayerStats.Instance.getDefense();
        }
        playerBattle.TakeDamage(damage);
        battleUIScript.UpdateLifeTexts();
        if (PlayerStats.Instance.getCurrentHP() <= 0)
        {
            state = BattleStateEnum.LOST;
            battleUIScript.UpdateTurnText(BattleStateEnum.LOST);
            StartCoroutine(EndBattleAfterDelay("MainMenu"));
        }
        else
        {
            ChangeToPlayerTurn();
        }
    }

    public void PlayerAttackEnemy()
    {
        int damage = 1;

        if (PlayerStats.Instance.getAttack() > enemyBattle.defense)
        {
            damage = PlayerStats.Instance.getAttack() - enemyBattle.defense;
        }

        enemyBattle.TakeDamage(damage);
        battleUIScript.UpdateLifeTexts();

        if (enemyBattle.currentHP <= 0)
        {
            battleUIScript.ShowButtons(false);
            state = BattleStateEnum.WON;
            int beforeLevel = PlayerStats.Instance.getLevel();
            PlayerStats.Instance.GainExpFromEnemy(enemyBattle.exp);
            PlayerStats.Instance.setCurrentHP(PlayerStats.Instance.getMaxHP());
            int afterLevel = PlayerStats.Instance.getLevel();

            if (afterLevel > beforeLevel)
            {
                battleUIScript.UpdateTurnText(BattleStateEnum.LEVELUP);
            }
            else
            {
                battleUIScript.UpdateTurnText(BattleStateEnum.WON);
            }


            StartCoroutine(EndBattleAfterDelay("TownScene"));
        }
        else
        {
            ChangeToEnemyTurn();
        }
    }

    IEnumerator EndBattleAfterDelay(string sceneName)
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneName);
    }

    public void ScapeFromBattle()
    {
        int randomNumber = Random.Range(0, 3);
        if (randomNumber == 0)
        {
            battleUIScript.ShowButtons(false);
            state = BattleStateEnum.WAITING;
            StartCoroutine(EndBattleAfterDelay("TownScene"));

        }
        else
        {
            ChangeToEnemyTurn();
        }
    }



}
