using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageAgent.Core.Behaviors
{
    public class NaturalNumberBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.Keyboard = Keyboard.Numeric;
            entry.TextChanged += OnTextChanged;
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnTextChanged;
        }

        private void OnTextChanged(object? sender, TextChangedEventArgs e)
        {
            if (sender is not Entry entry)
                return;

            var text = e.NewTextValue?.Trim() ?? "";

            // Allow empty temporarily
            if (string.IsNullOrEmpty(text))
                return;

            // Try to parse numeric input
            if (int.TryParse(text, out int number))
            {
                if (number <= 0)
                    entry.Text = e.OldTextValue; // reject 0 or negative
            }
            else
            {
                entry.Text = e.OldTextValue; // reject invalid text
            }
        }
    }
}
