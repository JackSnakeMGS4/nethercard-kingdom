﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] private float lerpFactor = 0.25f;
    [SerializeField] private GameObject hand;

    [Header("Card Draw Conditions")]
    [Tooltip("If card limit <= 0, it means infinite.")]
    [SerializeField] private int cardLimit = 3;
    [Tooltip("Auto draw will be disabled if value <= 0")]
    [SerializeField] private float autoDrawDelay = 0.25f;
    [SerializeField] private int drawCost = 1;
    [SerializeField] private int incrementCostPerCard = 1;

    [Header("New Card Properties")]
    [SerializeField] private Vector3 newCardPositionOffset = Vector3.zero;
    [SerializeField] private Vector3 newCardRotationOffset = Vector3.zero;
    [SerializeField] private GameObject[] cards;

    private Vector3 scaleToLerp = Vector3.one;
    private float autoDrawTimer = 0.0f;

    private void DrawCard()
    {
        if (SummoningManager.Instance.EnoughMana(drawCost + (incrementCostPerCard * hand.transform.childCount)))
        {
            SummoningManager.Instance.UseMana(drawCost + (incrementCostPerCard * hand.transform.childCount));

            GameObject newCard = Instantiate(cards[Random.Range(0, cards.Length)],
            transform.position + newCardPositionOffset,
            Quaternion.Euler(newCardRotationOffset.x, newCardRotationOffset.y, newCardRotationOffset.z));
            newCard.transform.SetParent(hand.transform, true);
            newCard.transform.SetSiblingIndex(0);

            autoDrawTimer = autoDrawDelay;
        }
    }

    void Start()
    {
    }

    void Update()
    {
        if (cardLimit <= 0 || hand.transform.childCount < cardLimit)
            if (autoDrawDelay > 0.0f && autoDrawTimer <= 0.0f)
                DrawCard();
        autoDrawTimer -= Time.deltaTime;

        transform.localScale = Vector3.Lerp(transform.localScale, scaleToLerp, lerpFactor);
    }

    public void OnHoverEnter()
    {
        scaleToLerp = Vector3.one * 1.2f;
    }

    public void OnHoverExit()
    {
        scaleToLerp = Vector3.one;
    }

    public void OnClicked()
    {
        if (cardLimit <= 0 || hand.transform.childCount < cardLimit)
            if (autoDrawDelay <= 0.0f)
                DrawCard();
    }
}