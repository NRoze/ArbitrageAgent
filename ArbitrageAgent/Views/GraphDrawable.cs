using ArbitrageAgent.Core.Models;
using ArbitrageAgent.ViewModel.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ArbitrageAgent.Views
{
    public class GraphDrawable : IDrawable
    {
        public event EventHandler InvalidateRequested;
        private readonly WeightedRouteViewModel _vm;

        public GraphDrawable(WeightedRouteViewModel vm)
        {
            _vm = vm;
            _vm.Graph.CollectionChanged += (s, e) => InvalidateRequested?.Invoke(this, EventArgs.Empty);
        }


        //public void Draw(ICanvas canvas, RectF dirtyRect)
        //{
        //    if (_vm.Graph.Count == 0)
        //        return;

        //    float x, y;
        //    AssetNode node;
        //    float centerX = dirtyRect.Width / 2;
        //    float centerY = dirtyRect.Height / 2;
        //    float radiusX = dirtyRect.Width / 2.2f;
        //    float radiusY = dirtyRect.Height / 3f;
        //    var nodePositions = new Dictionary<AssetNode, PointF>();

        //    for (int i = 0; i < _vm.Graph.Count; i++)
        //    {
        //        node = _vm.Graph[i];
        //        double angle = 2 * Math.PI * i / _vm.Graph.Count;
        //        x = centerX + (float)(radiusX * Math.Cos(angle));
        //        y = centerY + (float)(radiusY * Math.Sin(angle));
        //        float nodeRadius = 20;
        //        var center = new PointF(x, y);

        //        nodePositions[node] = center;

        //        // Draw node circle
        //        canvas.StrokeColor = Colors.Gray;
        //        canvas.StrokeSize = 1f;
        //        canvas.DrawCircle(center, nodeRadius);

        //        // Draw names or icons
        //        canvas.FontColor = Colors.White;
        //        canvas.FontSize = 10;

        //        string display1 = node.Name;
        //        //string display1 = string.IsNullOrEmpty(node.Icon1) ? node.Name1 : "🔷";
        //        string display2 = node.ExchangeName;
        //        //string display2 = string.IsNullOrEmpty(node.Icon2) ? node.Name2 : "🔸";

        //        canvas.DrawString(display1, x, y - 6, HorizontalAlignment.Center);
        //        canvas.DrawString(display2, x, y + 6, HorizontalAlignment.Center);
        //    }

        //    if (_vm.Routes != null)
        //    {
        //        var r = 0;
        //        foreach (var routeItem in _vm.Routes)
        //        {
        //            var route = routeItem.Route;
        //            var profit = routeItem.ProfitRate;
        //            var color = routeColors[r++ % routeColors.Length];

        //            // Draw each segment in the route
        //            for (int i = 0; i < route.Count - 1; i++)
        //            {
        //                var fromNode = route[i];
        //                var toNode = route[i + 1];

        //                if (nodePositions.TryGetValue(fromNode, out var start) &&
        //                    nodePositions.TryGetValue(toNode, out var end))
        //                {
        //                    // Draw line
        //                    canvas.StrokeColor = color;
        //                    canvas.StrokeSize = 2f;
        //                    canvas.DrawLine(start, end);

        //                    // Draw profit label at midpoint
        //                    var midX = (start.X + end.X) / 2;
        //                    var midY = (start.Y + end.Y) / 2;

        //                    canvas.FontColor = Colors.DarkGreen;
        //                    canvas.FontSize = 10;
        //                    canvas.DrawString($"{profit - 1:P2}", midX, midY - 5, HorizontalAlignment.Center);
        //                }
        //            }
        //        }
        //    }
        //}
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (_vm.Graph.Count == 0)
                return;

            float centerX = dirtyRect.Width / 2;
            float centerY = dirtyRect.Height / 2;

            // Elliptical radii
            float radiusX = dirtyRect.Width / 2.2f;
            float radiusY = dirtyRect.Height / 3f;
            float nodeRadius = 20;

            // Precompute node positions
            var nodePositions = new Dictionary<AssetNode, PointF>();
            for (int i = 0; i < _vm.Graph.Count; i++)
            {
                var node = _vm.Graph[i];
                double angle = 2 * Math.PI * i / _vm.Graph.Count;
                float x = centerX + (float)(radiusX * Math.Cos(angle));
                float y = centerY + (float)(radiusY * Math.Sin(angle));
                nodePositions[node] = new PointF(x, y);
            }

            // Predefined route colors
            Color[] routeColors = new Color[]
            {
                Colors.OrangeRed,
                Colors.MediumSeaGreen,
                Colors.DodgerBlue,
                Colors.Goldenrod,
                Colors.MediumVioletRed,
                Colors.DarkCyan,
                Colors.IndianRed,
                Colors.DarkOrange
            };

            // 1️⃣ Draw routes first (behind nodes)
            if (_vm.Routes != null)
            {
                for (int r = 0; r < _vm.Routes.Count; r++)
                {
                    var routeItem = _vm.Routes[r];
                    var route = routeItem.Route;
                    var profit = routeItem.ProfitRate;

                    // Pick color for this route
                    var color = routeColors[r % routeColors.Length];

                    for (int i = 0; i < route.Count - 1; i++)
                    {
                        var fromNode = route[i];
                        var toNode = route[i + 1];

                        if (nodePositions.TryGetValue(fromNode, out var start) &&
                            nodePositions.TryGetValue(toNode, out var end))
                        {
                            // Draw line
                            canvas.StrokeColor = color;
                            canvas.StrokeSize = 2f;
                            canvas.DrawLine(start, end);

                            // Draw profit label at midpoint
                            var midX = (start.X + end.X) / 2;
                            var midY = (start.Y + end.Y) / 2;
                            canvas.FontColor = Colors.White;
                            canvas.FontSize = 12;
                            canvas.DrawString($"{profit-1:P3}", midX, midY - 5, HorizontalAlignment.Center);
                        }
                    }
                }
            }

            // 2️⃣ Draw nodes on top
            foreach (var node in _vm.Graph)
            {
                var pos = nodePositions[node];

                // Node circle
                canvas.StrokeColor = Colors.Gray;
                canvas.StrokeSize = 1f;
                canvas.FillColor = Colors.DimGray;
                canvas.FillCircle(pos, nodeRadius);
                canvas.DrawCircle(pos, nodeRadius);

                // Node names
                canvas.FontColor = Colors.White;
                canvas.FontSize = 10;
                canvas.DrawString(node.Name, pos.X, pos.Y - 6, HorizontalAlignment.Center);
                canvas.DrawString(node.ExchangeName, pos.X, pos.Y + 6, HorizontalAlignment.Center);
            }
        }

    }
}

