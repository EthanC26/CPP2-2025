using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace StateMachine
{
    public abstract class ReactiveContext<T> where T : ReactiveContext<T>
    {
        private readonly Dictionary<string, List<Delegate>> propertyChangedCallbacks = 
            new Dictionary<string, List<Delegate>>();

        /// <summary>
        /// Registers a callback to be invoked when a property changes.
        /// The callback will receive the old and new value of the property.
        /// This overload uses an expression to infer the property name and type.
        /// </summary>
        /// <typeparam name="TPropertyType">The type of the property</typeparam>
        /// <param name="propertyExpression">An expression that accesses the property (e.g., () => PropertyName)</param>
        /// <param name="callback">The callback to invoke when the property changes, accepting (oldValue, newValue)</param>
        /// <returns>The context for method chaining</returns>
        public T OnPropertyChanged<TPropertyType>(
            Expression<Func<TPropertyType>> propertyExpression,
            Action<TPropertyType, TPropertyType> callback)
        {
            if (propertyExpression == null) throw new ArgumentNullException(nameof(propertyExpression));
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            string propertyName;
            if (propertyExpression.Body is MemberExpression memberExpression)
            {
                propertyName = memberExpression.Member.Name;
            }
            else
            {
                throw new ArgumentException("Expression must be a simple property access (e.g., () => PropertyName).", nameof(propertyExpression));
            }

            // Call the existing generic OnPropertyChanged method
            return OnPropertyChanged<TPropertyType>(propertyName, callback);
        }

        /// <summary>
        /// Registers a callback to be invoked when a property changes.
        /// The callback will receive the old and new value of the property.
        /// </summary>
        /// <typeparam name="TValue">The type of the property</typeparam>
        /// <param name="propertyName">The name of the property to monitor</param>
        /// <param name="callback">The callback to invoke when the property changes, accepting (oldValue, newValue)</param>
        /// <returns>The context for method chaining</returns>
        public T OnPropertyChanged<TValue>(string propertyName, Action<TValue, TValue> callback)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            if (!propertyChangedCallbacks.ContainsKey(propertyName))
            {
                propertyChangedCallbacks[propertyName] = new List<Delegate>();
            }

            propertyChangedCallbacks[propertyName].Add(callback);
            return this as T;
        }

        protected bool SetProperty<TValue>(ref TValue field, TValue value, string propertyName)
        {
            if (EqualityComparer<TValue>.Default.Equals(field, value))
            {
                return false;
            }

            TValue oldValue = field;
            field = value;
            NotifyPropertyChanged(propertyName, oldValue, value);
            return true;
        }

        protected void NotifyPropertyChanged<TValue>(string propertyName, TValue oldValue, TValue newValue)
        {
            if (propertyChangedCallbacks.TryGetValue(propertyName, out var callbacks))
            {
                foreach (var callbackDelegate in callbacks)
                {
                    if (callbackDelegate is Action<TValue, TValue> typedCallback)
                    {
                        try
                        {
                            typedCallback(oldValue, newValue);
                        }
                        catch (Exception ex)
                        {
                            // Log or handle exceptions during callback invocation
                            Console.WriteLine($"Error invoking property change callback for {propertyName}: {ex.Message}");
                        }
                    }
                }
            }
        }
    }
}

