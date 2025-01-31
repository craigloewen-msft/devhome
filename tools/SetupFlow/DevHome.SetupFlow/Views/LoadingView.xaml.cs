// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using DevHome.SetupFlow.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace DevHome.SetupFlow.Views;

public sealed partial class LoadingView : UserControl
{
    public LoadingView()
    {
        this.InitializeComponent();
    }

    public LoadingViewModel ViewModel => (LoadingViewModel)this.DataContext;
}
