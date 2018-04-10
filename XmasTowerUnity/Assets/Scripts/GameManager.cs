using UnityEngine;
using UnityEngine.U2D;

public class GameManager : MonoBehaviour {

    public bool isGameScreen;

    public SpriteAtlas giftSpriteAtlas;
    public Gift giftPrefab;

    private Gift currentGift = null;


    private Camera camera;

	void Start ()
    {
        Debug.Log("Screen Width : " + Screen.width);
        Debug.Log("Screen Height : " + Screen.height);

        camera = Camera.main;

        if (!isGameScreen)
            AddGift(new Vector2(0, 0));
    }

    public Gift AddGift(Vector2 position)
    {
        var gift = Instantiate(giftPrefab) as Gift;
        gift.Initialize(giftSpriteAtlas);

        gift.transform.position = new Vector2(
            position.x,
            position.y
        );

        return gift;
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
    }
}
