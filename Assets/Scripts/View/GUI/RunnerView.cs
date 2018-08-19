﻿using controller;
using model;
using UnityEngine;

namespace view.gui
{
    public class RunnerView : MonoBehaviour
    {
        public void Display(Game game)
        {
            GripFan grip = FindObjectOfType<GripFan>();
            StackPile stackPile = FindObjectOfType<StackPile>();
            RigGrid rig = FindObjectOfType<RigGrid>();
            var playZone = GameObject.Find("Play").AddComponent<DropZone>();
            var rigZone = GameObject.Find("Rig").AddComponent<DropZone>();
            var heapZone = GameObject.Find("Heap").AddComponent<DropZone>();
            var gripZone = GameObject.Find("Grip").AddComponent<DropZone>();
            grip.Construct(game, playZone, rigZone, heapZone);
            stackPile.Construct(game, gripZone);
            rig.Construct(game, playZone);
            GameObject.Find("Bank/Credit")
                .AddComponent<DroppableAbility>()
                .Represent(
                    game.runner.actionCard.credit,
                    game,
                    GameObject.Find("Runner/Right hand/Credits").AddComponent<DropZone>()
                );
           
            game.runner.credits.Observe(GameObject.Find("Runner/Right hand/Credits/Credits text").AddComponent<CreditPoolText>());
            var zones = game.runner.zones;
            zones.stack.ObserveCount(stackPile);
            zones.stack.ObservePopping(stackPile);
            zones.grip.ObserveAdditions(grip);
            zones.grip.ObserveRemovals(grip);
            zones.heap.Observe(FindObjectOfType<HeapPile>());
        }
    }
}