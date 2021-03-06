﻿// **********************************************************************
// 
//   DrawableMapOverlay.cs
//   
//   This file is subject to the terms and conditions defined in
//   file 'LICENSE.txt', which is part of this source code package.
//   
//   Copyright (c) 2017, Sylvain Gravel
// 
// ***********************************************************************

using System;
using Xamarin.Forms.Maps;
using Xamarin.Forms.SKMaps.Extensions;
using Xamarin.Forms.SKMaps.Models;
using Xamarin.Forms.SKMaps.Skia;

namespace Xamarin.Forms.SKMaps
{
    public abstract class SKMapOverlay : BindableObject
    {
        public class MapOverlayInvalidateEventArgs
        {
            public MapSpan GpsBounds { get; set; }
            public bool IsVisible { get; set; }

            internal MapOverlayInvalidateEventArgs(SKMapOverlay overlay)
            {
                GpsBounds = overlay.GpsBounds;
                IsVisible = overlay.IsVisible;
            }
        }

        public event EventHandler<MapOverlayInvalidateEventArgs> RequestInvalidate;

        public static readonly BindableProperty GpsBoundsProperty = BindableProperty.Create(nameof(GpsBounds), typeof(MapSpan), typeof(SKMapOverlay), new MapSpan(new Position(0, 0), 0.1, 0.1), propertyChanged: OnDrawablePropertyChanged);
        public static readonly BindableProperty IsVisibleProperty = BindableProperty.Create(nameof(IsVisible), typeof(bool), typeof(SKMapOverlay), true, propertyChanged: OnDrawablePropertyChanged);

        public MapSpan GpsBounds
        {
            get => (MapSpan)GetValue(GpsBoundsProperty);
            protected set => SetValue(GpsBoundsProperty, value.WrapIfRequired());
        }

        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        protected SKMapOverlay()
        {
            GpsBounds = MapSpanExtensions.WorldSpan;
        }

        private static void OnDrawablePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SKMapOverlay overlay = bindable as SKMapOverlay;

            overlay.Invalidate();
        }

        public void Invalidate()
        {
            RequestInvalidate?.Invoke(this, new MapOverlayInvalidateEventArgs(this));
        }

        public abstract void DrawOnMap(SKMapCanvas canvas, SKMapSpan canvasMapRect, double zoomScale);
    }
}
