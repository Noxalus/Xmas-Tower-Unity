using Assets.Scripts;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public bool isGameScreen;

    public SpriteAtlas giftSpriteAtlas;
    public Gift giftPrefab;
    public Text scoreText;
    public GameObject ground;

    // Camera
    private new Camera camera;
    private Vector3 cameraStartPosition;
    private Vector3 cameraTargetPosition;
    private float cameraInterpolationThreshold = Config.CAMERA_INTERPOLATION_THRESHOLD;
    private float cameraInterpolationSmoothness = Config.CAMERA_INTERPOLATION_SMOOTHNESS;
    private bool cameraIsMoving;

    private Gift currentGift = null;
    private BoxCollider2D currentGiftCollider = null;
    private float currentHeight = 0f;
    private float groundLevel;

    void Start ()
    {
        camera = Camera.main;
        cameraTargetPosition = camera.transform.position;

        if (!isGameScreen)
            AddGift(Vector2.zero);

        if (ground)
        {
            var groundCollider = ground.GetComponentInChildren<BoxCollider2D>();
            groundLevel = groundCollider.bounds.center.y + groundCollider.bounds.extents.y;
        }
    }

    public Gift AddGift(Vector2 position)
    {
        var gift = Instantiate(giftPrefab) as Gift;
        gift.Initialize(giftSpriteAtlas, isGameScreen);
        gift.transform.position = position;
        return gift;
    }

    // Used by hidden spawn button's OnClick event
    public void AddDummyGift()
    {
        AddGift(Vector2.zero);
    }

	void Update ()
    {
        if (!isGameScreen)
            return;

        if (!currentGift && !cameraIsMoving)
        {
            var cameraBounds = camera.OrthographicBounds();
            currentGift = AddGift(Vector2.zero);
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
                scoreText.text = currentHeight.ToString("0.0") + " cm";
                UpdateCameraPosition();
            }

            currentGift = null;
            currentGiftCollider = null;
        }
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
