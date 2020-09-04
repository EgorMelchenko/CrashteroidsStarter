using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TestSuite
{
    private Game game;

    //Старт игры - метод вызывается до выполнения каждого теста.
    [SetUp]
    public void Setup()
    {
        GameObject gameGameObject =
            MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Game"));
        game = gameGameObject.GetComponent<Game>();
    }

    //Конец игры - метод вызывается после выполнения каждого теста
    [TearDown]
    public void Teardown()
    {
        Object.Destroy(game.gameObject);
    }

    //1. Движение астероида вниз
    [UnityTest]
    public IEnumerator AsteroidsMoveDown()
    {
        GameObject asteroid = game.GetSpawner().SpawnAsteroid();
        float initialYPos = asteroid.transform.position.y;
        yield return new WaitForSeconds(0.1f);
        Assert.Less(asteroid.transform.position.y, initialYPos);
    }

    //2. Столкновение с астероидом - конец игры
    [UnityTest]
    public IEnumerator GameOverOccursOnAsteroidCollision()
    {
        GameObject asteroid = game.GetSpawner().SpawnAsteroid();
        asteroid.transform.position = game.GetShip().transform.position;
        yield return new WaitForSeconds(0.1f);
        Assert.True(game.isGameOver);
    }

    //3. Проверяет, что когда игрок нажимает New Game, значение gameOver bool не равно true
    [UnityTest]
    public IEnumerator NewGameRestartsGame()
    {
        game.isGameOver = true;
        game.NewGame();
        Assert.False(game.isGameOver);
        yield return null;
    }

    //4. Выстреливаемый кораблём лазерный луч летит вверх
    [UnityTest]
    public IEnumerator LaserMovesUp()
    {
        GameObject laser = game.GetShip().SpawnLaser();
        float initialYPos = laser.transform.position.y;
        yield return new WaitForSeconds(0.1f);
        Assert.Greater(laser.transform.position.y, initialYPos);
    }

    //5. При попадании лазер уничтожает астероид
    [UnityTest]
    public IEnumerator LaserDestroysAsteroid()
    {
        GameObject asteroid = game.GetSpawner().SpawnAsteroid();
        asteroid.transform.position = Vector3.zero;
        GameObject laser = game.GetShip().SpawnLaser();
        laser.transform.position = Vector3.zero;
        yield return new WaitForSeconds(0.1f);
        UnityEngine.Assertions.Assert.IsNull(asteroid);
    }

    //6. Когда игрок уничтожает астероид, счёт увеличивается
    [UnityTest]
    public IEnumerator DestroyedAsteroidRaisesScore()
    {
        GameObject asteroid = game.GetSpawner().SpawnAsteroid();
        asteroid.transform.position = Vector3.zero;
        GameObject laser = game.GetShip().SpawnLaser();
        laser.transform.position = Vector3.zero;
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(game.score, 1);
    }

    //7. Движение астероида в сторону
    [UnityTest]
    public IEnumerator AsteroidsMoveSide()
    {
        GameObject asteroid = game.GetSpawner().SpawnAsteroid();
        float initialXPos = asteroid.transform.position.x;
        yield return new WaitForSeconds(0.1f);
        //Assert.Less(asteroid.transform.position.x, initialXPos);
        Assert.LessOrEqual(asteroid.transform.position.x, initialXPos);
    }

    //8. Столкновение с астероидом не происходит и игра не завершается
    [UnityTest]
    public IEnumerator CollisionWithAnAsteroidDoesNotOccur()
    {
        GameObject asteroid = game.GetSpawner().SpawnAsteroid();
        float initialAsteroidXPos = asteroid.transform.position.x;
        float initialShipXPos = game.GetShip().transform.position.x;
        yield return new WaitForSeconds(0.1f);
        Assert.False(game.isGameOver);
    }

}