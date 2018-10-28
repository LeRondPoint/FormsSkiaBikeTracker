﻿// **********************************************************************
// 
//   SkiaMapOverlay.cs
//   
//   This file is subject to the terms and conditions defined in
//   file 'LICENSE.txt', which is part of this source code package.
//   
//   Copyright (c) 2018, Sylvain Gravel
// 
// ***********************************************************************

using CoreLocation;
using MapKit;
using Xamarin.Forms.Maps.Overlays.Platforms.Ios.Extensions;

namespace Xamarin.Forms.Maps.Overlays.Platforms.Ios.UI.Renderers
{
    internal class SkiaMapOverlay : MKOverlay
    {
        public override CLLocationCoordinate2D Coordinate => SharedOverlay.GpsBounds.Center.ToLocationCoordinate();
        public override MKMapRect BoundingMapRect => SharedOverlay.GpsBounds.ToMapRect();

        public SKMapOverlay SharedOverlay { get; }
        private OverlayedMap _SharedControl { get; }

        public SkiaMapOverlay(SKMapOverlay sharedOverlay, OverlayedMap sharedControl)
        {
            SharedOverlay = sharedOverlay;
            _SharedControl = sharedControl;
        }
    }
}