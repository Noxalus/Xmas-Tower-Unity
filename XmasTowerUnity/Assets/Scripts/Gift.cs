﻿using UnityEngine;
using UnityEngine.U2D;

public class Gift : MonoBehaviour
{
    public enum GiftState
    {
        IDLE,
        SELECTED,
        FALLING,
        COLLISIONING,
        SLEEPING,
        SICK,
        ANGRY,
        POUTING,
        HURT
    }

    public static readonly string[] BodySpriteType = new string[]
    {
        "giftbox_blue_green", "giftbox_blue_pink", "giftbox_blue_red", "giftbox_blue_white", "giftbox_blue_yellow",
        "giftbox_green_blue", "giftbox_green_pink", "giftbox_green_red", "giftbox_green_white", "giftbox_green_yellow",
        "giftbox_pink_green", "giftbox_pink_blue", "giftbox_pink_red", "giftbox_pink_white", "giftbox_pink_yellow",
        "giftbox_red_green", "giftbox_red_blue", "giftbox_red_pink", "giftbox_red_white", "giftbox_red_yellow",
        "giftbox_white_green", "giftbox_white_pink", "giftbox_white_red", "giftbox_white_blue", "giftbox_white_yellow",
        "giftbox_yellow_green", "giftbox_yellow_pink", "giftbox_yellow_red", "giftbox_yellow_white", "giftbox_yellow_blue"
    };

    public static readonly string[] EyeSpriteType = new string[]
    {
        "eye1", "eye2", "eye3", "eye4", "eye5", "eye6", "eye7", "eye8"
    };

    public static readonly string[] MouthSpriteType = new string[]
    {
        "mouth1", "mouth2", "mouth3", "mouth4", "mouth5", "mouth6", "mouth7"
    };

    public static readonly string[] RibbonSpriteType = new string[]
    {
        "ribbon1", "ribbon2", "ribbon3", "ribbon4", "ribbon5", "ribbon6"
    };

    private SpriteRenderer Body;
    private SpriteRenderer Ribbon;
    private SpriteRenderer LeftEye;
    private SpriteRenderer RightEye;
    private SpriteRenderer Mouth;

    private Rigidbody2D rigidBody;
    private GiftState currentState;

    void Awake()
    {
        Body = transform.Find("Body").gameObject.GetComponent<SpriteRenderer>();
        Ribbon = transform.Find("Ribbon").gameObject.GetComponent<SpriteRenderer>();
        LeftEye = transform.Find("LeftEye").gameObject.GetComponent<SpriteRenderer>();
        RightEye = transform.Find("RightEye").gameObject.GetComponent<SpriteRenderer>();
        Mouth = transform.Find("Mouth").gameObject.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void Initialize(SpriteAtlas spriteAtlas)
    {
        currentState = GiftState.IDLE;

        var defaultEyeSprite = spriteAtlas.GetSprite(EyeSpriteType[0]);
        var defaultMouthSprite = spriteAtlas.GetSprite(MouthSpriteType[0]);
        var randomBodySpriteName = BodySpriteType[Random.Range(0, BodySpriteType.Length - 1)];

        Body.sprite = spriteAtlas.GetSprite(randomBodySpriteName);
        Ribbon.sprite = spriteAtlas.GetSprite(GetCorrespondingRibbon(randomBodySpriteName));
        LeftEye.sprite = defaultEyeSprite;
        RightEye.sprite = defaultEyeSprite;
        Mouth.sprite = defaultMouthSprite;

        transform.localScale *= Random.Range(1, 5);
    }

    void Update()
    {
        if (currentState == GiftState.COLLISIONING && rigidBody.IsSleeping())
        {
            //Body.color = Color.green;
            currentState = GiftState.SLEEPING;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision!");
        currentState = GiftState.COLLISIONING;
    }

    public GiftState GetCurrentState()
    {
        return currentState;
    }

    #region Utils
    private string GetCorrespondingRibbon(string bodySpriteName)
    {
        if (bodySpriteName.EndsWith("_pink"))
            return "ribbon1";
        else if (bodySpriteName.EndsWith("_yellow"))
            return "ribbon2";
        else if (bodySpriteName.EndsWith("_red"))
            return "ribbon3";
        else if (bodySpriteName.EndsWith("_green"))
            return "ribbon4";
        else if (bodySpriteName.EndsWith("_white"))
            return "ribbon5";
        else if (bodySpriteName.EndsWith("_blue"))
            return "ribbon6";

        return RibbonSpriteType[0];
    }
    #endregion
}
