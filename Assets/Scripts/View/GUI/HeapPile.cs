﻿using UnityEngine;
using model.cards;
using model;

namespace view.gui
{
    public class HeapPile : MonoBehaviour, IHeapObserver
    {
        void Start()
        {
            gameObject.AddComponent<CardPrinter>();
        }

        void IHeapObserver.NotifyCardAdded(ICard card)
        {
            GetComponent<CardPrinter>().Print(card);
        }
    }
}