using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public bool isGameScreen;

    public SpriteAtlas giftSpriteAtlas;
    public Gift giftPrefab;
    public Text scoreText;
    public GameObject ground;
    public MenuManager MenuManager;
    public GameObject HighscoreBar;

    // Camera
    private new Camera camera;
    private Vector3 cameraStartPosition;
    private Vector3 cameraTargetPosition;
    private float cameraInterpolationThreshold = Config.CAMERA_INTERPOLATION_THRESHOLD;
    private float cameraInterpolationSmoothness = Config.CAMERA_INTERPOLATION_SMOOTHNESS;
    private bool cameraIsMoving;

    private List<Gift> gifts;
    private Gift currentGift = null;
    private BoxCollider2D currentGiftCollider = null;
    private float currentHeight = 0f;
    private float groundLevel;
    private bool gameIsOver;

    private float highscore;

    void Start ()
    {
        gifts = new List<Gift>();
        camera = Camera.main;

        highscore = PlayerPrefs.GetFloat("Highscore", 0);

        if (!isGameScreen)
            AddGift();
        else
        {
            if (ground)
            {
                var groundCollider = ground.GetComponentInChildren<BoxCollider2D>();
                groundLevel = groundCollider.bounds.center.y + groundCollider.bounds.extents.y;
            }

            Restart();
        }
    }

    public void Restart()
    {
        foreach (var gift in gifts)
            Destroy(gift.gameObject);

        gifts.Clear();

        currentGift = null;
        currentGiftCollider = null;
        scoreText.text = "0 cm";
        currentHeight = 0f;

        gameIsOver = false;

        MenuManager.ShowGameOverButtons(false);

        if (highscore > 0)
        {
            HighscoreBar.SetActive(true);
            var highscoreBarHeightPosition = highscore + groundLevel;
            HighscoreBar.transform.position = new Vector3(HighscoreBar.transform.position.x, highscoreBarHeightPosition, HighscoreBar.transform.position.z);
        }
        else
            HighscoreBar.SetActive(false);

    }

    public Gift AddGift()
    {
        var gift = Instantiate(giftPrefab) as Gift;
        gift.Initialize(giftSpriteAtlas, isGameScreen);

        gifts.Add(gift);

        return gift;
    }

    // Use by the hidden spawn button on the menu screen
    public void AddDummyGift()
    {
        AddGift();
    }

    void Update ()
    {
        if (!isGameScreen || gameIsOver)
            return;

        if (!currentGift && !cameraIsMoving)
        {
            var cameraBounds = camera.OrthographicBounds();
            currentGift = AddGift();
            currentGiftCollider = currentGift.GetComponent<BoxCollider2D>();
            var giftSpawnOffset = 1 + (currentGiftCollider.bounds.max.y / 2);
            currentGift.transform.position = new Vector2(0, (camera.transform.position.y + cameraBounds.size.y / 2) - giftSpawnOffset);
        }

        if (currentGift && currentGift.GetCurrentState() == Gift.GiftState.SLEEPING)
        {
            var currentGiftHighestPoint = currentGift.GetHighestPoint() - groundLevel;

            if (currentGiftHighestPoint > currentHeight)
            {
                currentHeight = currentGiftHighestPoint;
                scoreText.text = (currentHeight * Config.SCORE_FACTOR).ToString("0.0") + " cm (best: " + (PlayerPrefs.GetFloat("Highscore", 0f) * Config.SCORE_FACTOR).ToString("0.0") + " cm)";

                // Store the highscore
                if (currentHeight > PlayerPrefs.GetFloat("Highscore", 0f))
                {
                    highscore = currentHeight;
                    PlayerPrefs.SetFloat("Highscore", highscore);
                    PlayerPrefs.Save();
                }

                UpdateCameraPosition();
            }

            currentGift = null;
            currentGiftCollider = null;
        }

        CheckGameOver();
    }

    private void CheckGameOver()
    {
        // Check only the current selected gift for now
        if (currentGift)
        {
            var giftScreenPosition = camera.WorldToScreenPoint(currentGift.transform.position);
            var cameraBounds = camera.OrthographicBounds();

            if (currentGift.transform.position.y < -cameraBounds.extents.y || giftScreenPosition.x < 0 || giftScreenPosition.x > camera.pixelWidth)
                GameOver();
        }
    }

    private void GameOver()
    {
        gameIsOver = true;
        currentGift.ForceSleep();

        // Make sure to reset the camera to the origin
        cameraTargetPosition = new Vector3(0f, 0f, camera.transform.position.z);
        cameraIsMoving = true;

        // Stop gifts physics
        foreach (var gift in gifts)
            gift.ForceSleep(true);

        // TODO: When camera has stopped to move
        MenuManager.ShowGameOverButtons();
    }

    private void UpdateCameraPosition()
    {
        if (currentHeight > camera.transform.position.y + camera.OrthographicBounds().extents.y / 4f)
        {
            var cameraOffset = camera.OrthographicBounds().extents.y / 4f;
            var targetPosition = new Vector3(camera.transform.position.x, currentHeight - cameraOffset, camera.transform.position.z);

            cameraStartPosition = camera.transform.position;
            cameraTargetPosition = targetPosition;
            cameraIsMoving = true;
        }
    }

    void FixedUpdate()
    {
        if (!cameraIsMoving)
            return;

        if (Mathf.Abs(camera.transform.position.x - cameraTargetPosition.x) > cameraInterpolationThreshold ||
            Mathf.Abs(camera.transform.position.y - cameraTargetPosition.y) > cameraInterpolationThreshold)
        {
            camera.transform.position += (cameraTargetPosition - camera.transform.position) * cameraInterpolationSmoothness;
        }
        else
            cameraIsMoving = false;
    }
}
