// Copyright (c) Microsoft Corporation and Contributors
// Licensed under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media;

namespace DevHome.Dashboard.ViewModels;

public partial class AddWidgetViewModel : ObservableObject
{
    [ObservableProperty]
    private string _widgetDisplayTitle;

    [ObservableProperty]
    private string _widgetProviderDisplayTitle;

    [ObservableProperty]
    private Brush _widgetScreenshot;

    [ObservableProperty]
    private bool _pinButtonVisibility;

    public AddWidgetViewModel()
    {
    }

    public void Clear()
    {
        WidgetDisplayTitle = string.Empty;
        WidgetProviderDisplayTitle = string.Empty;
        WidgetScreenshot = null;
        PinButtonVisibility = false;
    }
}
