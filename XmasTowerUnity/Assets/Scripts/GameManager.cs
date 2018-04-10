using UnityEngine;
using UnityEngine.U2D;

public class GameManager : MonoBehaviour {

    public bool isGameScreen;

    public SpriteAtlas giftSpriteAtlas;
    public Gift giftPrefab;

	void Start ()
    {
        if (!isGameScreen)
            AddGift(new Vector2(0, 0));
    }

    public void AddGift(Vector2 position)
    {
        var gift = Instantiate(giftPrefab) as Gift;
        gift.Initialize(giftSpriteAtlas);
        gift.transform.position = position;
    }

	void Update ()
    {
	}
}
