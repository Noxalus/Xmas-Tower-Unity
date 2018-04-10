using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public bool isGameScreen;

    public SpriteAtlas giftSpriteAtlas;
    public Gift giftPrefab;
    public Text scoreText;
    public GameObject ground;

    private new Camera camera;
    private Gift currentGift = null;
    private BoxCollider2D currentGiftCollider = null;
    private float currentHeight = 0f;
    private float groundLevel;

    void Start ()
    {
        camera = Camera.main;

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

        if (!currentGift)
        {
            var cameraBounds = camera.OrthographicBounds();
            currentGift = AddGift(Vector2.zero);
            currentGiftCollider = currentGift.GetComponent<BoxCollider2D>();
            var giftSpawnOffset = 1 + (currentGiftCollider.bounds.max.y / 2);
            currentGift.transform.position = new Vector2(0, (cameraBounds.size.y / 2) - giftSpawnOffset);
        }

        if (currentGift.GetCurrentState() == Gift.GiftState.SLEEPING)
        {
            var currentGiftHighestPoint = GetHighestPoint(currentGiftCollider) - groundLevel;

            if (currentGiftHighestPoint > currentHeight)
            {
                currentHeight = currentGiftHighestPoint;
                scoreText.text = currentHeight.ToString("0.0") + " cm";
            }

            currentGift = null;
            currentGiftCollider = null;
        }
    }

    private float GetHighestPoint(BoxCollider2D collider)
    {
        // World space corner of the collider
        Vector3 bottomLeftCorner = collider.bounds.center - collider.bounds.extents;
        Vector3 topRightCorner = collider.bounds.center + collider.bounds.extents;
        Vector3 bottomRightCorner = new Vector2(
            collider.bounds.center.x + collider.bounds.extents.x,
            collider.bounds.center.y - collider.bounds.extents.y
        );
        Vector3 topLeftCorner = new Vector2(
            collider.bounds.center.x - collider.bounds.extents.x,
            collider.bounds.center.y + collider.bounds.extents.y
        );

        return Mathf.Max(bottomLeftCorner.y, topRightCorner.y, bottomRightCorner.y, topLeftCorner.y);
    }
}
