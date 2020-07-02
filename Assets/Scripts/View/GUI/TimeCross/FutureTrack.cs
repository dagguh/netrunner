﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using model;

namespace view.gui.timecross
{
    public class FutureTrack : MonoBehaviour, IClickObserver
    {
        private HorizontalLayoutGroup horizontal;

        private List<GameObject> renderedClicks = new List<GameObject>();

        void Awake()
        {
            horizontal = gameObject.AddComponent<HorizontalLayoutGroup>();
            horizontal.childAlignment = TextAnchor.MiddleLeft;
            horizontal.childControlWidth = false;
            horizontal.childControlHeight = true;
            horizontal.childForceExpandWidth = false;
            horizontal.childForceExpandHeight = true;
        }

        void IClickObserver.NotifyClicks(int spent, int remaining)
        {
            var desired = remaining;
            AddMissing(desired);
            RemoveExtra(desired);
        }

        private void AddMissing(int desired)
        {
            while (renderedClicks.Count < desired)
            {
                Render();
            }
        }

        private void RemoveExtra(int desired)
        {
            var extra = renderedClicks.Count - desired;
            if (extra > 0)
            {
                foreach (var click in renderedClicks.GetRange(0, extra))
                {
                    Destroy(click);
                    renderedClicks.Remove(click);
                }
            }
        }

        private void Render()
        {
            var click = ClickBox.RenderClickBox(transform);
            var aspect = click.AddComponent<AspectRatioFitter>();
            aspect.aspectRatio = 1;
            aspect.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
            horizontal.CalculateLayoutInputHorizontal();
            horizontal.CalculateLayoutInputVertical();
            horizontal.SetLayoutHorizontal();
            horizontal.SetLayoutVertical();
            renderedClicks.Add(click);
        }
    }
}