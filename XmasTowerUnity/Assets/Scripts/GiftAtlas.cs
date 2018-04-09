using UnityEngine;
using UnityEngine.U2D;

enum SpriteType
{
    eye1, eye2, eye3, eye4, eye5, eye6, eye7, eye8,
    ribbon1, ribbon2, ribbon3, ribbon4, ribbon5, ribbon6,
    mouth1, mouth2, mouth3, mouth4, mouth5, mouth6, mouth7,
    giftbox_blue_green, giftbox_blue_pink, giftbox_blue_red, giftbox_blue_white, giftbox_blue_yellow,
    giftbox_green_blue, giftbox_green_pink, giftbox_green_red, giftbox_green_white, giftbox_green_yellow,
    giftbox_pink_green, giftbox_pink_blue, giftbox_pink_red, giftbox_pink_white, giftbox_pink_yellow,
    giftbox_red_green, giftbox_red_blue, giftbox_red_pink, giftbox_red_white, giftbox_red_yellow,
    giftbox_white_green, giftbox_white_pink, giftbox_white_red, giftbox_white_blue, giftbox_white_yellow,
    giftbox_yellow_green, giftbox_yellow_pink, giftbox_yellow_red, giftbox_yellow_white, giftbox_yellow_blue
}

public class GiftAtlas : MonoBehaviour {

    [SerializeField]
    private SpriteType currentType;

    [SerializeField]
    private SpriteAtlas atlas;

    private SpriteRenderer renderer;

    void Start () {
        renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = atlas.GetSprite(currentType.ToString());
    }
}
