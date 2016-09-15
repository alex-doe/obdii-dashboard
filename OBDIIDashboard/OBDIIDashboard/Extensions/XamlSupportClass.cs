using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace OBDIIDashboard.Extensions {
    internal class XamlSupportClass {
    }

    public sealed class MyBooleanToVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var flag = false;
            if (value is bool) {
                flag = (bool) value;
            }
            if (parameter != null) {
                if (bool.Parse((string) parameter) == (bool) value) {
                    flag = true;
                }
                else {
                    flag = false;
                }
            }
            if (flag) {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            var back = ((value is Visibility) && (((Visibility) value) == Visibility.Visible));
            if (parameter != null) {
                if ((bool) parameter) {
                    back = !back;
                }
            }
            return back;
        }
    }

    public class BrushColorConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if ((bool) value) {
                {
                    return new SolidColorBrush(Colors.GreenYellow);
                }
            }
            return new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}