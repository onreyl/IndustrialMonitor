using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IndustrialMonitor.ViewModels
{
    /// <summary>
    /// Base class for ViewModels that implementing INotifyPropertyChanged interface.
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event for a specific property.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed. Automatically captured if not provided.</param>
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the value of a property and raises the PropertyChanged event if the value has changed.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="field">The backing field for the property.</param>
        /// <param name="value">The new value.</param>
        /// <param name="propertyName">The name of the property. Automatically captured.</param>
        /// <returns><c>true</c> if the value was changed; otherwise, <c>false</c>.</returns>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value)) return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
