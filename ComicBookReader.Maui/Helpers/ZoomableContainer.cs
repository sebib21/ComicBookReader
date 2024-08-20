namespace ComicBookReader.Maui.Helpers
{
    public class ZoomableContainer : ContentView
    {
        private double currentScale = 1;
        private double startScale = 1;
        private double xOffset = 0;
        private double yOffset = 0;

        private const double maxScale = 2.0;

        public ZoomableContainer()
        {
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            GestureRecognizers.Add(panGesture);

            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                var pinchGesture = new PinchGestureRecognizer();
                pinchGesture.PinchUpdated += OnPinchUpdated;
                GestureRecognizers.Add(pinchGesture);
            }

            if (DeviceInfo.Platform == DevicePlatform.WinUI || DeviceInfo.Platform == DevicePlatform.MacCatalyst)
            {
                // todo
            }
        }

        private void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
        {
            if (e.StatusType == GestureStatus.Running)
            {
                double newX = xOffset + e.TotalX;
                double newY = yOffset + e.TotalY;
                Content.TranslationX = Math.Clamp(newX, -Content.Width * (currentScale - 1), 0);
                Content.TranslationY = Math.Clamp(newY, -Content.Height * (currentScale - 1), 0);
            }
            else if (e.StatusType == GestureStatus.Completed)
            {
                xOffset = Content.TranslationX;
                yOffset = Content.TranslationY;
            }
        }

        private void OnPinchUpdated(object? sender, PinchGestureUpdatedEventArgs e)
        {
            if (e.Status == GestureStatus.Started)
            {
                startScale = Content.Scale;
                Content.AnchorX = 0;
                Content.AnchorY = 0;
            }
            else if (e.Status == GestureStatus.Running)
            {
                currentScale += (e.Scale - 1) * startScale;
                currentScale = Math.Clamp(currentScale, 1, maxScale);

                double renderedX = Content.X + xOffset;
                double deltaX = renderedX / Width;
                double deltaWidth = Width / (Content.Width * startScale);
                double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

                double renderedY = Content.Y + yOffset;
                double deltaY = renderedY / Height;
                double deltaHeight = Height / (Content.Height * startScale);
                double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

                double targetX = xOffset - (originX * Content.Width) * (currentScale - startScale);
                double targetY = yOffset - (originY * Content.Height) * (currentScale - startScale);

                Content.TranslationX = Math.Clamp(targetX, -Content.Width * (currentScale - 1), 0);
                Content.TranslationY = Math.Clamp(targetY, -Content.Height * (currentScale - 1), 0);

                Content.Scale = currentScale;
            }
            else if (e.Status == GestureStatus.Completed)
            {
                xOffset = Content.TranslationX;
                yOffset = Content.TranslationY;
            }
        }


    }
}
