using UnityEngine;
using UnityEngine.U2D;

public class GameManager : MonoBehaviour {

    public bool isGameScreen;

    public SpriteAtlas giftSpriteAtlas;
    public Gift giftPrefab;

    private Gift currentGift = null;

    private new Camera camera;

    void Start ()
    {
        camera = Camera.main;

        if (!isGameScreen)
            AddGift(Vector2.zero);
    }

    public Gift AddGift(Vector2 position)
    {
        var gift = Instantiate(giftPrefab) as Gift;
        gift.Initialize(giftSpriteAtlas, isGameScreen);
        gift.transform.position = position;
        return gift;
    }

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
            var giftCollider = currentGift.GetComponent<BoxCollider2D>();
            var giftSpawnOffset = 1 + (giftCollider.bounds.max.y / 2);
            currentGift.transform.position = new Vector2(0, (cameraBounds.size.y / 2) - giftSpawnOffset);
        }

        if (currentGift.GetCurrentState() == Gift.GiftState.SLEEPING)
            currentGift = null;
    }
}
