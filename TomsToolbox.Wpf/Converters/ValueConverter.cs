﻿namespace TomsToolbox.Wpf.Converters
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    using JetBrains.Annotations;

    /// <summary>
    /// A base class for value converters performing pre-check of value and error handling.
    /// </summary>
    [ContractClass(typeof(ValueConverterContract))]
    public abstract class ValueConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets the <c>null</c> value, which is returned whenever the value to convert is <c>null</c>; the default is <c>null</c>.
        /// </summary>
        public object ConvertNullValue { get; set; } = null;

        /// <summary>
        /// Gets or sets the <c>unset</c> value, which is returned whenever the value to convert is <see cref="DependencyProperty.UnsetValue"/>; the default is <see cref="DependencyProperty.UnsetValue"/>.
        /// </summary>
        public object ConvertUnsetValue { get; set; } = DependencyProperty.UnsetValue;

        /// <summary>
        /// Gets or sets the <c>error</c> value, which is returned whenever the value to convert produces an error; the default is <see cref="DependencyProperty.UnsetValue"/>.
        /// </summary>
        public object ConvertErrorValue { get; set; } = DependencyProperty.UnsetValue;

        /// <summary>
        /// Gets or sets the <c>null</c> value, which is returned whenever the value to convert back is <c>null</c>; the default is <c>null</c>.
        /// </summary>
        public object ConvertBackNullValue { get; set; } = null;

        /// <summary>
        /// Gets or sets the <c>unset</c> value, which is returned whenever the value to convert back is <see cref="DependencyProperty.UnsetValue"/>; the default is <see cref="DependencyProperty.UnsetValue"/>.
        /// </summary>
        public object ConvertBackUnsetValue { get; set; } = DependencyProperty.UnsetValue;

        /// <summary>
        /// Gets or sets the <c>error</c> value, which is returned whenever the value to convert back produces an error; the default is <see cref="DependencyProperty.UnsetValue"/>.
        /// </summary>
        public object ConvertBackErrorValue { get; set; } = DependencyProperty.UnsetValue;

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        protected abstract object Convert([NotNull] object value, [CanBeNull] Type targetType, [CanBeNull] object parameter, [CanBeNull] CultureInfo culture);

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">ConvertBack is not supported by this converter.</exception>
        protected virtual object ConvertBack([NotNull] object value, [CanBeNull] Type targetType, [CanBeNull] object parameter, [CanBeNull] CultureInfo culture)
        {
            Contract.Requires(value != null);

            throw new InvalidOperationException("ConvertBack is not supported by this converter.");
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) 
                return ConvertNullValue;
            if (value == DependencyProperty.UnsetValue)
                return ConvertUnsetValue;

            try
            {
                return Convert(value, targetType, parameter, culture);
            }
            catch (Exception ex)
            {
                this.TraceError(ex.Message, "Convert");

                return ConvertErrorValue;
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return ConvertBackNullValue;
            if (value == DependencyProperty.UnsetValue)
                return ConvertBackUnsetValue;

            try
            {
                return ConvertBack(value, targetType, parameter, culture);
            }
            catch (Exception ex)
            {
                this.TraceError(ex.Message, "ConvertBack");
                return ConvertBackErrorValue;
            }
        }
    }

    [ContractClassFor(typeof(ValueConverter))]
    abstract class ValueConverterContract : ValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        protected override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Contract.Requires(value != null);

            throw new NotImplementedException();
        }
    }
}
